using UnityEngine;

namespace Ziumper.Shooter
{
    public class AimingFirePlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            if (!data.Weapon.EquippedWeapon.IsAutomatic())
            {
                if(data.Weapon.EquippedWeapon.HasAmmunition())
                {
                    data.CameraRecoil.RecoilShot(data.Weapon.EquippedWeaponSettings.AimingRecoil);
                    context.PlayerStates.Firing.FireSingle();
                    context.PlayerEvents.OnSingleFireCancel.AddListener(() =>
                    {
                        context.PlayerEvents.OnSingleFire.RemoveAllListeners();
                        context.ChangeStateTo(context.PlayerStates.Aiming, data);
                    });
                } else
                {
                    //no ammunition therefore just recoil and don't switch states we gona reload afterwards!
                    data.CameraRecoil.RecoilShot(data.Weapon.EquippedWeaponSettings.AimingRecoil);
                    context.PlayerStates.Firing.FireSingle();
                }
            }

            context.PlayerEvents.OnJump.RemoveAllListeners();
        }

        public override void ExitState()
        {
            base.ExitState();
            context.PlayerEvents.OnSingleFireCancel.RemoveAllListeners();
        }

        public override void Update()
        {
            if(data.IsRunning)
            {
                data.Move.CurrentSpeed = data.SpeedWalking;
            }

            if(data.Weapon.EquippedWeapon.IsAutomatic())
            {
                if (data.Input.IsHoldingButtonFire && data.Input.IsHoldingButtonAim)
                {
                    data.CameraRecoil.RecoilShot(data.Weapon.EquippedWeaponSettings.AimingRecoil);
                    context.PlayerStates.Firing.FireSingle();
                    context.PlayerStates.Aiming.UpdateAiming(true);
                    UpdateMovement();
                    CalculateJump(); //we going down
                }
                else
                {
                    context.ChangeStateTo(context.PlayerStates.Default, data);
                }
            }
        }
    }

}

