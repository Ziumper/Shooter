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
            context.PlayerEvents.OnInventoryNext.AddListener((scrollValue) => {
                int indexNext = scrollValue > 0 ? data.Inventory.GetNextIndex() : data.Inventory.GetLastIndex();
                //Get the current weapon's index.
                int indexCurrent = data.Inventory.GetEquippedIndex();

                //Make sure we're allowed to change, and also that we're not using the same index, otherwise weird things happen!
                if ((indexCurrent != indexNext))
                {
                    data.NextWeaponIndex = indexNext;
                    context.ChangeStateTo(context.States.NextWeapon, data);
                }
            });

            context.PlayerEvents.OnReloadStart.AddListener(() => context.ChangeStateTo(context.States.Reloading, data));
            context.PlayerEvents.OnInspectStart.AddListener(() => context.ChangeStateTo(context.States.Inspecting, data));
            context.PlayerEvents.OnSingleFire.AddListener(ChangeToFire);
         
        }

        private void ChangeToFire()
        {
            context.ChangeStateTo(context.States.Firing, data);
        }

        public override void Update()
        {
            base.Update();

            if (data.Input.IsHoldingButtonAim)
            {
                context.ChangeStateTo(context.States.Aiming, data);
                return;
            }
          
            if (data.Input.IsHoldingButtonRun && !context.States.Running.IsMovingSideWays())
            {
                context.ChangeStateTo(context.States.Running, data);
                return;
            }

            if (data.Input.IsHoldingButtonFire)
            {
                context.ChangeStateTo(context.States.Firing, data);
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

