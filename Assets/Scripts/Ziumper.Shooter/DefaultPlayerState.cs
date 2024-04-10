using System;
using UnityEngine;

namespace Ziumper.Shooter
{
    public class DefaultPlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            //weapon events
            context.StateEvents.OnInventoryNext.AddListener((scrollValue) => {
                int indexNext = scrollValue > 0 ? data.Inventory.GetNextIndex() : data.Inventory.GetLastIndex();
                //Get the current weapon's index.
                int indexCurrent = data.Inventory.GetEquippedIndex();

                //Make sure we're allowed to change, and also that we're not using the same index, otherwise weird things happen!
                if ((indexCurrent != indexNext))
                {
                    data.NextWeaponIndex = indexNext;
                    context.ChangeStateTo(context.Holstering, data);
                }
            });

            context.StateEvents.OnReloadStart.AddListener(() => context.ChangeStateTo(context.Reloading, data));
            context.StateEvents.OnInspectStart.AddListener(() => context.ChangeStateTo(context.Inspecting, data));
            context.StateEvents.OnSingleFire.AddListener(ChangeToFire);
         
        }

        private void ChangeToFire()
        {
            context.ChangeStateTo(context.Firing, data);
        }

        public override void Update()
        {
            base.Update();

            if (data.Input.IsHoldingButtonAim)
            {
                context.ChangeStateTo(context.Aiming, data);
                return;
            }
          
            if (data.Input.IsHoldingButtonRun && !context.Running.IsMovingSideWays())
            {
                context.ChangeStateTo(context.Running, data);
                return;
            }

            if (data.Input.IsHoldingButtonFire)
            {
                context.ChangeStateTo(context.Firing, data);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
            context.StateEvents.OnSingleFire.RemoveAllListeners();
            context.StateEvents.OnInventoryNext.RemoveAllListeners();
            context.StateEvents.OnReloadStart.RemoveAllListeners();
            context.StateEvents.OnInspectStart.RemoveAllListeners();
        }


    }

}

