﻿using InfimaGames.LowPolyShooterPack;
using System;
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
            Vector2 frameInput = character.GetInputMovement();
            bool isNoInput = frameInput.x == 0 && frameInput.y == 0;
            
            if ((!data.Input.IsHoldingButtonRun || isNoInput || IsMovingSideWays()) && data.IsGrounded)
            {
                context.ChangeStateTo(context.Default, data);
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

