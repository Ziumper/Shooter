using InfimaGames.LowPolyShooterPack;
using System;
using System.Linq;
using UnityEngine;

namespace Ziumper.Shooter
{
    public abstract class MovingPlayerState : PlayerState
    {
        protected static readonly int HashMovement = Animator.StringToHash("Movement");
        protected float movingSpeed;

        protected AudioClip footstepsClip;
        protected CharacterBehaviour character;
        protected CharacterController controller;

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            //set up defaults
            movingSpeed = data.SpeedWalking;
            footstepsClip = data.AudioClipWalking;

            //cache 
            if (character == null)
            {
                character = context.GetComponent<CharacterBehaviour>();
            }

            if (controller == null)
            {
                controller = context.GetComponent<CharacterController>();
            }

            context.StateEvents.OnJump.AddListener(Jump);
        }

        protected virtual void Jump()
        {
            if (data.IsGrounded)
            {
                data.JumpingForce = Vector3.up * data.JumpingHeight;
            }
        }

        public override void Update()
        {
            UpdateMovementAnimatorValue();
            context.Aiming.UpdateAimingAnimatorValue(false);
            UpdateMovement();
            CalculateJump();
        }

        protected void UpdateMovement()
        {
            Vector2 frameInput = character.GetInputMovement();
            var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
            movement *= movingSpeed;
            movement = character.transform.TransformDirection(movement);

            if(data.PlayerGravity > data.GravityMin && data.JumpingForce.y < 0.1f)
            {
                data.PlayerGravity -= data.GravityAmount * Time.deltaTime; 
            }

            if(data.PlayerGravity < -1 && controller.isGrounded)
            {
                data.PlayerGravity = -1;
            }

            if(data.JumpingForce.y > 0.1f)
            {
                data.PlayerGravity = 0;
            }

            movement.y += data.PlayerGravity;
            movement += data.JumpingForce * Time.deltaTime;

            controller.Move(movement);

            data.IsGrounded = controller.isGrounded;
            PlayFootstepSounds(footstepsClip);
        }

        protected void PlayFootstepSounds(AudioClip footstepClip)
        {
            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (data.IsGrounded && controller.velocity.sqrMagnitude > 0.1f)
            {
                //Select the correct audio clip to play.
                data.AudioSource.clip = footstepClip;
                //Play it!
                if (!data.AudioSource.isPlaying)
                    data.AudioSource.Play();
            }
            //Pause it if we're doing something like flying, or not moving!
            else if (data.AudioSource.isPlaying)
                data.AudioSource.Pause();
        }

        public void UpdateMovementAnimatorValue()
        {
            //Movement Value. This value affects absolute movement. Aiming movement uses this, as opposed to per-axis movement.
            data.CharacterAnimator.SetFloat(HashMovement, Mathf.Clamp01(Mathf.Abs(data.AxisMovement.x) + Mathf.Abs(data.AxisMovement.y)), data.DampTimeLocomotion, Time.deltaTime);
        }

        public override void LateUpdate()
        {
            //We need a weapon for this!
            if (data.EquippedWeapon == null)
                return;

            //Weapons without a scope should not be a thing! Ironsights are a scope too!
            if (data.EquippedWeaponScope == null)
                return;

            //Make sure that we have a kinematics component!
            if (data.CharacterKinematics != null)
            {
                //Compute.
                data.CharacterKinematics.Compute();
            }
        }

        public override void ExitState()
        {
            context.StateEvents.OnJump.RemoveAllListeners();
        }

        protected virtual void CalculateJump()
        {
            data.JumpingForce = Vector3.SmoothDamp(data.JumpingForce, Vector3.zero, ref data.JumpingVelocity, data.JumpingFalloff);
        }

    }

}

