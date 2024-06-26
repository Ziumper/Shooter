﻿using UnityEngine;


namespace Ziumper.Shooter
{
    public class NextInventoryPlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            if(!data.IsHolstered)
            {
                data.IsHolstered = true;
                data.CharacterAnimator.SetBool("Holstered", data.IsHolstered);
                context.PlayerEvents.OnHolsteringEnd.AddListener(OnHolsteringEnd);
            } else
            {
                OnHolsteringEnd();
            }
        }

        public void NextInventory(float scrollValue)
        {
            int indexNext = scrollValue > 0 ? data.Inventory.GetNextIndex() : data.Inventory.GetLastIndex();
            //Get the current weapon's index.
            int indexCurrent = data.Inventory.GetEquippedIndex();

            //Make sure we're allowed to change, and also that we're not using the same index, otherwise weird things happen!
            if ((indexCurrent != indexNext))
            {
                data.Weapon.NextWeaponIndex = indexNext;
                context.ChangeStateTo(context.PlayerStates.NextWeapon, data);
            }
        }

        private void OnHolsteringEnd()
        {
            context.PlayerEvents.OnHolsteringEnd.RemoveListener(OnHolsteringEnd);
            context.ChangeStateTo(context.PreviousState, data);
        }

        public override void ExitState()
        {
            base.ExitState();
            data.IsHolstered = false;
            data.CharacterAnimator.SetBool("Holstered", data.IsHolstered);

            data.CharacterAnimator.Play("Unholster", data.LayerHolster, 0);

            data.Inventory.Equip(data.Weapon.NextWeaponIndex);
            context.PlayerStates.Firing.RefreshWeaponSetup();
        }
        
    }

}

