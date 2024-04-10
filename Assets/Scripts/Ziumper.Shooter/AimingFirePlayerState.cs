namespace Ziumper.Shooter
{
    public class AimingFirePlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            if (!data.EquippedWeapon.IsAutomatic())
            {
                context.PlayerStates.Firing.FireSingle();
                context.PlayerEvents.OnSingleFireCancel.AddListener(() =>
                {
                    context.PlayerEvents.OnSingleFire.RemoveAllListeners();
                    context.ChangeStateTo(context.PlayerStates.Aiming, data);
                });
            }

            context.PlayerEvents.OnJump.RemoveAllListeners();
        }

        public override void Update()
        {
            if(data.IsRunning)
            {
                data.Move.CurrentSpeed = data.SpeedWalking;
            }

            if(data.EquippedWeapon.IsAutomatic())
            {
                if (data.Input.IsHoldingButtonFire && data.Input.IsHoldingButtonAim)
                {
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

