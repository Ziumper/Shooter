namespace Ziumper.Shooter
{
    public class AimingFirePlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            if (!data.EquippedWeapon.IsAutomatic())
            {
                context.States.Firing.FireSingle();
                context.PlayerEvents.OnSingleFireCancel.AddListener(() =>
                {
                    context.PlayerEvents.OnSingleFire.RemoveAllListeners();
                    context.ChangeStateTo(context.States.Aiming, data);
                });
            }

            context.PlayerEvents.OnJump.RemoveAllListeners();
        }

        public override void Update()
        {
            if(data.EquippedWeapon.IsAutomatic())
            {
                if (data.Input.IsHoldingButtonFire && data.Input.IsHoldingButtonAim)
                {
                    context.States.Firing.FireSingle();
                    context.States.Aiming.UpdateAiming(true);
                    UpdateMovement();
                }
                else
                {
                    context.ChangeStateTo(context.States.Aiming, data);
                }
            }
        }
    }

}

