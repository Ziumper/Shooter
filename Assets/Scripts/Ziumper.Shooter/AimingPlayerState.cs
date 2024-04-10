using System;
using UnityEngine;

namespace Ziumper.Shooter
{
    public class AimingPlayerState : MovingPlayerState
    {
        protected static readonly int HashAimingAlpha = Animator.StringToHash("Aiming");
        private const string boolNameAim = "Aim";

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            SetAimingAnimationCondition(true);
         
            context.PlayerEvents.OnJump.RemoveAllListeners();
            context.PlayerEvents.OnSingleFire.AddListener(() => { 
                context.ChangeStateTo(context.PlayerStates.AimingFire, data); 
            });
        }

        public override void ExitState()
        {
            context.PlayerEvents.OnSingleFire.RemoveAllListeners();
        }

        public override void Update()
        {
            if (!data.Input.IsHoldingButtonAim)
            {
                SetAimingAnimationCondition(false);
                context.ChangeStateTo(context.PlayerStates.Default, data);
                return;
            }

            UpdateAiming(true);
            UpdateMovement();
        }

        public void SetAimingAnimationCondition(bool aiming)
        {
            data.CharacterAnimator.SetBool(boolNameAim, aiming);
        }

        public void UpdateAimingAnimatorValue(bool aiming)
        {
            ////Update the aiming value, but use interpolation. This makes sure that things like firing can transition properly.
            data.CharacterAnimator.SetFloat(HashAimingAlpha, Convert.ToSingle(aiming), 0.25f / 1.0f * data.DampTimeAiming, Time.deltaTime);
        }

        public void UpdateAiming(bool aiming)
        {
            UpdateMovementAnimatorValue();
            UpdateAimingAnimatorValue(aiming);
        }
    }
}

