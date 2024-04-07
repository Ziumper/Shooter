namespace Ziumper.Shooter
{
    public class AimingFirePlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            if (!data.EquippedWeapon.IsAutomatic())
            {
                context.Firing.FireSingle();
                context.StateEvents.OnSingleFireCancel.AddListener(() =>
                {
                    context.StateEvents.OnSingleFire.RemoveAllListeners();
                    context.ChangeStateTo(context.Aiming, data);
                });
            }
        }

        public override void Update()
        {
            if(data.EquippedWeapon.IsAutomatic())
            {
                if (data.IsHoldingButtonFire && data.IsHoldingButtonAim)
                {
                    context.Firing.FireSingle();
                    context.Aiming.UpdateAiming(true);
                    PlayFootstepSounds(data.AudioClipWalking);
                }
                else
                {
                    context.ChangeStateTo(context.Aiming, data);
                }
            }

           
        }
    }

}

