
namespace Ziumper.Shooter
{
    public class DefaultPlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            //set up defaults if not set 
            data.Move.CurrentSpeed = data.SpeedWalking;
            data.Move.FootstepsAudio = data.AudioClipWalking;

            //weapon events
            context.PlayerEvents.OnInventoryNext.AddListener((scrollValue) => context.PlayerStates.NextWeapon.NextInventory(scrollValue));

            context.PlayerEvents.OnReloadStart.AddListener(() => context.ChangeStateTo(context.PlayerStates.Reloading, data));
            context.PlayerEvents.OnInspectStart.AddListener(() => context.ChangeStateTo(context.PlayerStates.Inspecting, data));
            context.PlayerEvents.OnSingleFire.AddListener(() => context.ChangeStateTo(context.PlayerStates.Firing, data));
        }
        
        public override void Update()
        {
            base.Update();

            if (data.Input.IsHoldingButtonAim)
            {
                context.ChangeStateTo(context.PlayerStates.Aiming, data);
                return;
            }
          
            if (data.Input.IsHoldingButtonRun && !context.PlayerStates.Running.IsMovingSideWays())
            {
                context.ChangeStateTo(context.PlayerStates.Running, data);
                return;
            }

            if (data.Input.IsHoldingButtonFire)
            {
                context.ChangeStateTo(context.PlayerStates.Firing, data);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            context.PlayerEvents.OnSingleFire.RemoveAllListeners();
            context.PlayerEvents.OnInventoryNext.RemoveAllListeners();
            context.PlayerEvents.OnReloadStart.RemoveAllListeners();
            context.PlayerEvents.OnInspectStart.RemoveAllListeners();
        }


    }

}

