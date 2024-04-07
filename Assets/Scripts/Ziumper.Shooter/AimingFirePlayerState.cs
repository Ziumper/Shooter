namespace Ziumper.Shooter
{
    public class AimingFirePlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            context.Firing.FireSingle();
            data.IsAiming = true;
        }

        public override void Update()
        {
            if (data.IsHoldingButtonFire && data.IsHoldingButtonAim)
            {
                //Check.
                context.Firing.FireSingle();
                context.Aiming.UpdateAiming(true);
            } else if (data.IsHoldingButtonAim)
            {
                context.ChangeStateTo(context.Aiming,data);
            }
            else
            {
                context.ChangeStateTo(context.Default, data);
            }
        }
    }

}

