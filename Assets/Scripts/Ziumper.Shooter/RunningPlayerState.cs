using System;
using UnityEngine;

namespace Ziumper.Shooter
{
    public class RunningPlayerState : MovingPlayerState
    {
        private const string boolNameRun = "Running";

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            data.Move.CurrentSpeed = data.SpeedRunning;
            data.Move.FootstepsAudio = data.AudioClipRunning;

            //weapon events
            context.PlayerEvents.OnInventoryNext.AddListener((scrollValue) => context.PlayerStates.NextWeapon.NextInventory(scrollValue));
            context.PlayerEvents.OnReloadStart.AddListener(() => context.ChangeStateTo(context.PlayerStates.Reloading, data));
            context.PlayerEvents.OnSingleFire.AddListener(() => context.ChangeStateTo(context.PlayerStates.Firing, data));

            SetRunningAnimationCondition(true);
        }

        public override void ExitState()
        {
            SetRunningAnimationCondition(false);
            context.PlayerEvents.OnSingleFire.RemoveAllListeners();
            context.PlayerEvents.OnInventoryNext.RemoveAllListeners();
            context.PlayerEvents.OnReloadStart.RemoveAllListeners();
        }

        public override void Update()
        {
            base.Update();

            if (data.Input.IsHoldingButtonAim && !data.Input.IsHoldingButtonFire && data.IsGrounded)
            {
                context.ChangeStateTo(context.PlayerStates.Aiming, data);
                return;
            }

            Vector2 frameInput = character.GetInputMovement();
            bool isNoInput = frameInput.x == 0 && frameInput.y == 0;
            bool shouldChangeToDefault = (!data.Input.IsHoldingButtonRun || isNoInput || IsMovingSideWays()) && data.IsGrounded;
            if (shouldChangeToDefault)
            {
                context.ChangeStateTo(context.PlayerStates.Default, data);
                return;
            }

            SetRunningAnimationCondition(data.IsGrounded);
        }

        public bool IsMovingSideWays()
        {
            return data.Input.AxisMovement.y <= 0 || Math.Abs(Mathf.Abs(data.Input.AxisMovement.x) - 1) < 0.01f;
        }

        private void SetRunningAnimationCondition(bool running)
        {
            data.CharacterAnimator.SetBool(boolNameRun, running);
        }
    }

}

