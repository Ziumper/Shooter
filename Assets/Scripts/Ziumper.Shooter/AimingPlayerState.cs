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

            Debug.Log("Entering Aiming State");
            SetAimingAnimationCondition(true);

            context.StateEvents.OnSingleFire.AddListener(() => { 
                context.ChangeStateTo(context.AimingFire, data); 
            });
        }

        public override void ExitState()
        {
            context.StateEvents.OnSingleFire.RemoveAllListeners();
        }

        public override void Update()
        {
            if (!data.IsHoldingButtonAim)
            {
                SetAimingAnimationCondition(false);
                context.ChangeStateTo(context.Default, data);
                return;
            }

            UpdateAiming(true);
            PlayFootstepSounds(data.AudioClipWalking);
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

