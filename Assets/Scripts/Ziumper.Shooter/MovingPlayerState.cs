using InfimaGames.LowPolyShooterPack;
using System.Linq;
using UnityEngine;

namespace Ziumper.Shooter
{
    public abstract class MovingPlayerState : PlayerState
    {
        protected static readonly int HashMovement = Animator.StringToHash("Movement");
        protected float movingSpeed;
        private readonly RaycastHit[] groundHits = new RaycastHit[8];

        protected AudioClip footstepsClip;
        protected CharacterBehaviour character;
        protected Rigidbody rigidbody;

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

            if (rigidbody == null)
            {
                rigidbody = context.GetComponent<Rigidbody>();
            }
        }

        public override void Update()
        {
            UpdateMovementAnimatorValue();
            context.Aiming.UpdateAimingAnimatorValue(false);
            PlayFootstepSounds(footstepsClip);
        }

        public override void FixedUpdate()
        {
            Vector2 frameInput = character.GetInputMovement();
            UpdateMovement(frameInput, movingSpeed);
            data.IsGrounded = true;
        }

        protected void UpdateMovement(Vector2 frameInput, float speed)
        {
            var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);
            movement *= speed;
            movement = character.transform.TransformDirection(movement);

            rigidbody.velocity = movement;
        }

        protected void PlayFootstepSounds(AudioClip footstepClip)
        {
            //Check if we're moving on the ground. We don't need footsteps in the air.
            if (data.IsGrounded && rigidbody.velocity.sqrMagnitude > 0.1f)
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

        public override void OnCollisitonStay()
        {
            //Bounds.
            Bounds bounds = data.Capsule.bounds;
            //Extents.
            Vector3 extents = bounds.extents;
            //Radius.
            float radius = extents.x - 0.01f;

            //Cast. This checks whether there is indeed ground, or not.
            Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
                groundHits, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);

            //We can ignore the rest if we don't have any proper hits.
            if (!groundHits.Any(hit => hit.collider != null && hit.collider != data.Capsule))
                return;

            //Store RaycastHits.
            for (var i = 0; i < groundHits.Length; i++)
                groundHits[i] = new RaycastHit();

            //Set grounded. Now we know for sure that we're grounded.
            data.IsGrounded = true;
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

        
    }

}

