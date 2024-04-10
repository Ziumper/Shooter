using UnityEngine;

namespace Ziumper.Shooter
{
    public class ReloadingPlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            Debug.Log("Entering Reload state");

            //Get the name of the animation state to play, which depends on weapon settings, and ammunition!
            string stateName = data.EquippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty";
            //Play the animation state!
            data.CharacterAnimator.Play(stateName, data.LayerActions, 0.0f);

            //Reload.
            data.EquippedWeapon.Reload();
            context.PlayerEvents.OnReloadEnd.AddListener(OnReloadEnd);
        }

        private void OnReloadEnd()
        {
            context.PlayerEvents.OnReloadEnd.RemoveListener(OnReloadEnd);
            context.ChangeStateTo(context.Previous, data);
        }
    }

}

