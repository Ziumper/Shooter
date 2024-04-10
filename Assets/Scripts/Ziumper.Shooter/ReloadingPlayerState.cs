using UnityEngine;

namespace Ziumper.Shooter
{
    public class ReloadingPlayerState : MovingPlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            //Get the name of the animation state to play, which depends on weapon settings, and ammunition!
            string stateName = data.EquippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty";
            //Play the animation state!
            data.CharacterAnimator.Play(stateName, data.LayerActions, 0.0f);

            //Reload.
            data.EquippedWeapon.Reload();
            context.PlayerEvents.OnReloadEnd.AddListener(OnReloadEnd);
        }

        public override void Update()
        {
            base.Update();

            bool wasPreivouslyAiming = context.PreviousState == context.PlayerStates.Aiming;
            bool reloadingAndWasPreivouslyRunning = context.PreviousState == context.PlayerStates.Running && data.IsRunning; //handle when player is running on ground and still reloading
            if (reloadingAndWasPreivouslyRunning || wasPreivouslyAiming)
            {
                data.Move.CurrentSpeed = data.SpeedWalking;
            }  
        }

        private void OnReloadEnd()
        {
            context.PlayerEvents.OnReloadEnd.RemoveListener(OnReloadEnd);
            context.ChangeStateTo(context.PlayerStates.Default, data);
        }
    }

}

