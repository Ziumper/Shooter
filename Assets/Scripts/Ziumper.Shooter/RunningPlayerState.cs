using InfimaGames.LowPolyShooterPack;
using System.Data.Common;
using UnityEngine;

namespace Ziumper.Shooter
{
    public class RunningPlayerState : MovingPlayerState
    {
        private const string boolNameRun = "Running";

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            Debug.Log("Entering running state");
            base.EnterState(context, data);
            movingSpeed = data.SpeedRunning;
            footstepsClip = data.AudioClipRunning;
            SetRunningAnimationCondition(true);
        }

        public override void ExitState()
        {
            SetRunningAnimationCondition(false);
            data.AudioSource.Stop();
        }

        public override void Update()
        {
            base.Update();

            if(!data.IsHoldingButtonRun)
            {
                context.ChangeStateTo(context.Default, data);
                return;
            }
        }

     
        private void SetRunningAnimationCondition(bool running)
        {
            data.CharacterAnimator.SetBool(boolNameRun, running);
        }
    }

}

