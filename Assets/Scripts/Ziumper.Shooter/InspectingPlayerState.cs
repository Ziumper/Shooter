using UnityEngine;

namespace Ziumper.Shooter
{
    public class InspectingPlayerState : PlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            data.CharacterAnimator.CrossFade("Inspect", 0.0f, data.LayerActions, 0);
            context.StateEvents.OnInspectEnd.AddListener(OnInspectEnd);
        }

        private void OnInspectEnd()
        {
            context.StateEvents.OnInspectEnd.RemoveListener(OnInspectEnd);
            context.ChangeStateTo(context.Default, data);
        }
        
    }

}

