using UnityEngine;
using Ziumper.Shooter.Weapons;

namespace Ziumper.Shooter
{
    public class FiringPlayerState : MovingPlayerState
    {
        protected const string fireStateName = "Fire";
        protected const string fireEmptyStateName = "Fire Empty";

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
        
            if(!data.Weapon.EquippedWeapon.IsAutomatic()) 
            {
                data.CameraRecoil.RecoilShot(data.Weapon.EquippedWeaponSettings.DefaultRecoil);
                FireSingle();

                context.PlayerEvents.OnSingleFireCancel.AddListener(() => 
                {
                    context.PlayerEvents.OnSingleFire.RemoveAllListeners();
                    context.ChangeStateTo(context.PlayerStates.Default, data);
                });
            }
        }

        public override void Update()
        {
            base.Update();

            if (data.Input.IsHoldingButtonAim)
            {
                context.ChangeStateTo(context.PlayerStates.AimingFire, data);
                return;
            }

            if (context.PlayerStates.Running == context.PreviousState && data.IsRunning)
            {
                data.Move.CurrentSpeed = data.SpeedWalking;   
            }

            if(data.Weapon.EquippedWeapon.IsAutomatic())
            {
                if (data.Input.IsHoldingButtonFire)
                {
                    data.CameraRecoil.RecoilShot(data.Weapon.EquippedWeaponSettings.DefaultRecoil);
                    FireSingle();
                }
                else
                {
                    context.ChangeStateTo(context.PlayerStates.Default, data);
                }
            }
        }

        public void FireSingle()
        {
           
            if (data.Weapon.EquippedWeapon.HasAmmunition())
            {
                if (HasFireRatePassed())
                {
                    Fire();
                }
            }
            //Fire Empty.
            else if (HasFireRatePassed())
            {
                FireEmpty();
                context.ChangeStateTo(context.PlayerStates.Reloading, data);
            }
        }

        public void Fire()
        {
            //Save the shot time, so we can calculate the fire rate correctly.
            data.Weapon.LastShotTime = Time.time;
            //Fire the weapon! Make sure that we also pass the scope's spread multiplier if we're aiming.
            data.Weapon.EquippedWeapon.Fire();

            //Play firing animation.
            data.CharacterAnimator.CrossFade(fireStateName, 0.05f, data.LayerOverlay, 0);
        }

        public override void ExitState()
        {
            base.ExitState();
            context.PlayerEvents.OnSingleFireCancel.RemoveAllListeners();
        }

        public void FireEmpty()
        {
            /*
			 * Save Time. Even though we're not actually firing, we still need this for the fire rate between
			 * empty shots.
			 */
            data.Weapon.LastShotTime = Time.time;
            //Play.
            data.CharacterAnimator.CrossFade(fireEmptyStateName, 0.05f, data.LayerOverlay, 0);
        }

        public void RefreshWeaponSetup()
        {
            //Make sure we have a weapon. We don't want errors!
            if ((data.Weapon.EquippedWeapon = data.Inventory.GetEquipped()) == null)
                return;

            //Update Animator Controller. We do this to update all animations to a specific weapon's set.
            data.CharacterAnimator.runtimeAnimatorController = data.Weapon.EquippedWeapon.GetAnimatorController();

            //Get the attachment manager so we can use it to get all the attachments!
            data.Weapon.WeaponAttachmentManager = data.Weapon.EquippedWeapon.GetAttachmentManager();
            if (data.Weapon.WeaponAttachmentManager == null)
                return;

            //Get equipped scope. We need this one for its settings!
            data.Weapon.EquippedWeaponScope = data.Weapon.WeaponAttachmentManager.GetEquippedScope();
            //Get equipped magazine. We need this one for its settings!
            data.Weapon.EquippedWeaponMagazine = data.Weapon.WeaponAttachmentManager.GetEquippedMagazine();
            data.Weapon.EquippedWeaponSettings = data.Weapon.EquippedWeapon.GetComponent<WeaponSettings>();
        }

        private bool HasFireRatePassed()
        {
            return Time.time - data.Weapon.LastShotTime > 60.0f / data.Weapon.EquippedWeapon.GetRateOfFire();
        }
    }

}

