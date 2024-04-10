using UnityEngine;

namespace Ziumper.Shooter
{
    public class InspectingPlayerState : PlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            data.CharacterAnimator.CrossFade("Inspect", 0.0f, data.LayerActions, 0);
            context.PlayerEvents.OnInspectEnd.AddListener(OnInspectEnd);
        }

        private void OnInspectEnd()
        {
            context.PlayerEvents.OnInspectEnd.RemoveListener(OnInspectEnd);
            context.ChangeStateTo(context.PlayerStates.Default, data);
        }
        
    }

}

