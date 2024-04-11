using InfimaGames.LowPolyShooterPack;
using System;
using System.Linq;
using UnityEngine;

namespace Ziumper.Shooter
{
    public abstract class MovingPlayerState : PlayerState
    {
        protected static readonly int HashMovement = Animator.StringToHash("Movement");
        protected CharacterBehaviour character;
        protected CharacterController controller;

        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);

            //cache 
            if (character == null)
            {
                character = context.GetComponent<CharacterBehaviour>();
            }

            if (controller == null)
            {
                controller = context.GetComponent<CharacterController>();
            }

            context.PlayerEvents.OnJump.AddListener(Jump);
        }

        protected virtual void Jump()
        {
            if (data.IsGrounded)
            {
                Vector2 frameInput = character.GetInputMovement();
                data.Move.JumpDirectionValue = frameInput.y;
                data.Move.JumpingForce = Vector3.up * data.JumpingHeight;
            }
        }

        public override void Update()
        {
            UpdateMovement();
            PlayFootstepSounds();
            CalculateJump();

            UpdateMovementAnimatorValue();
            context.PlayerStates.Aiming.UpdateAimingAnimatorValue(false);
        }

        protected void UpdateMovement()
        {
            Vector2 frameInput = character.GetInputMovement();

            var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
            if (!data.IsGrounded)
            {
                movement.z = data.Move.JumpDirectionValue;
            } 
            
            movement *= data.Move.CurrentSpeed * Time.deltaTime;
            movement = character.transform.TransformDirection(movement);

          

            if(data.PlayerGravity > data.GravityMin && data.Move.JumpingForce.y < 0.1f)
            {
                data.PlayerGravity -= data.GravityAmount * Time.deltaTime; 
            }

            if(data.PlayerGravity < -1 && controller.isGrounded)
            {
                data.PlayerGravity = -1;
            }

            if(data.Move.JumpingForce.y > 0.1f)
            {
                data.PlayerGravity = 0;
            }

            movement.y += data.PlayerGravity;
            movement += data.Move.JumpingForce * Time.deltaTime;

            controller.Move(movement);

            data.IsGrounded = controller.isGrounded;
        }

        protected void PlayFootstepSounds()
        {
            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (data.IsGrounded && controller.velocity.sqrMagnitude > 0.1f)
            {
                //Select the correct audio clip to play.
                data.AudioSource.clip = data.Move.FootstepsAudio;
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
            float movementAnimator = Mathf.Clamp01(Mathf.Abs(data.Input.AxisMovement.x) + Mathf.Abs(data.Input.AxisMovement.y));

            if(!data.IsGrounded)
            {
                movementAnimator = 0f;
            }

            //Movement Value. This value affects absolute movement. Aiming movement uses this, as opposed to per-axis movement.
            data.CharacterAnimator.SetFloat(HashMovement,movementAnimator, data.DampTimeLocomotion, Time.deltaTime);
        }

        public override void LateUpdate()
        {
            //We need a weapon for this!
            if (data.Weapon.EquippedWeapon == null)
                return;

            //Weapons without a scope should not be a thing! Ironsights are a scope too!
            if (data.Weapon.EquippedWeaponScope == null)
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
            context.PlayerEvents.OnJump.RemoveAllListeners();
        }

        protected virtual void CalculateJump()
        {
            data.Move.JumpingForce = Vector3.SmoothDamp(data.Move.JumpingForce, Vector3.zero, ref data.Move.JumpingVelocity, data.JumpingFalloff);
        }

    }

}

