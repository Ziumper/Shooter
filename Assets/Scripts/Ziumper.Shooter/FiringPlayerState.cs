using UnityEngine;

namespace Ziumper.Shooter
{
    public class FiringPlayerState : MovingPlayerState
    {
        protected const string fireStateName = "Fire";
        protected const string fireEmptyStateName = "Fire Empty";

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            Debug.Log("Entering single");
            if(!data.EquippedWeapon.IsAutomatic()) 
            {
                FireSingle();

                context.StateEvents.OnSingleFireCancel.AddListener(() => 
                {
                    context.StateEvents.OnSingleFire.RemoveAllListeners();
                    context.ChangeStateTo(context.Default, data);
                });
            }
        }

        public override void Update()
        {
            base.Update();

            if(data.EquippedWeapon.IsAutomatic())
            {
                if (data.Input.IsHoldingButtonFire)
                {
                    FireSingle();
                }
                else
                {
                    context.ChangeStateTo(context.Default, data);
                }
            }
        }

        public void FireSingle()
        {
            if (data.EquippedWeapon.HasAmmunition())
            {
                if (HasFireRatePassed())
                {
                    Fire();
                }
            }
            //Fire Empty.
            else
            {
                FireEmpty();
            }
        }

        public void Fire()
        {
            //Save the shot time, so we can calculate the fire rate correctly.
            data.LastShotTime = Time.time;
            //Fire the weapon! Make sure that we also pass the scope's spread multiplier if we're aiming.
            data.EquippedWeapon.Fire();

            //Play firing animation.
            data.CharacterAnimator.CrossFade(fireStateName, 0.05f, data.LayerOverlay, 0);
        }

        public override void ExitState()
        {
            base.ExitState();
            context.StateEvents.OnSingleFireCancel.RemoveAllListeners();
        }

        public void FireEmpty()
        {
            /*
			 * Save Time. Even though we're not actually firing, we still need this for the fire rate between
			 * empty shots.
			 */
            data.LastShotTime = Time.time;
            //Play.
            data.CharacterAnimator.CrossFade(fireEmptyStateName, 0.05f, data.LayerOverlay, 0);
        }

        public void RefreshWeaponSetup()
        {
            //Make sure we have a weapon. We don't want errors!
            if ((data.EquippedWeapon = data.Inventory.GetEquipped()) == null)
                return;

            //Update Animator Controller. We do this to update all animations to a specific weapon's set.
            data.CharacterAnimator.runtimeAnimatorController = data.EquippedWeapon.GetAnimatorController();

            //Get the attachment manager so we can use it to get all the attachments!
            data.WeaponAttachmentManager = data.EquippedWeapon.GetAttachmentManager();
            if (data.WeaponAttachmentManager == null)
                return;

            //Get equipped scope. We need this one for its settings!
            data.EquippedWeaponScope = data.WeaponAttachmentManager.GetEquippedScope();
            //Get equipped magazine. We need this one for its settings!
            data.EquippedWeaponMagazine = data.WeaponAttachmentManager.GetEquippedMagazine();
        }

        private bool HasFireRatePassed()
        {
            return Time.time - data.LastShotTime > 60.0f / data.EquippedWeapon.GetRateOfFire();
        }
    }

}

