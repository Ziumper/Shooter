using UnityEngine;


namespace Ziumper.Shooter
{
    public class InventoryNextPlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            Debug.Log("Entering Holstering!");

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

        private void OnHolsteringEnd()
        {
            context.PlayerEvents.OnHolsteringEnd.RemoveListener(OnHolsteringEnd);
            context.ChangeStateTo(context.States.Default, data);
        }

        public override void ExitState()
        {
            data.IsHolstered = false;
            data.CharacterAnimator.SetBool("Holstered", data.IsHolstered);

            data.CharacterAnimator.Play("Unholster", data.LayerHolster, 0);

            data.Inventory.Equip(data.NextWeaponIndex);
            context.States.Firing.RefreshWeaponSetup();
        }
        
    }

}

