using InfimaGames.LowPolyShooterPack;
using System;
using UnityEngine;

namespace Ziumper.Shooter
{
    [Serializable]
    public class PlayerData
    {
        [Header("Inventory")]
        public InventoryBehaviour Inventory;
        [Header("Cameras")]
        [Tooltip("Normal Camera")]
        public Camera CameraWorld;

        [Tooltip("Determines how smooth the locomotion blendspace is.")]
        public float DampTimeLocomotion;

        [Tooltip("How smoothly we play aiming transitions. Beware that this affects lots of things!")]
        public float DampTimeAiming;

        [Header("Animation Procedural")]
        [Tooltip("Character Animator.")]
        public Animator CharacterAnimator;

        [Header("Audio Clips")]

        [Tooltip("The audio clip that is played while walking.")]
        public AudioClip AudioClipWalking;

        [Tooltip("The audio clip that is played while running.")]
        public AudioClip AudioClipRunning;

        [Header("Speeds")]
        public float SpeedWalking = 0.05f;

        [Tooltip("How fast the player moves while running."), SerializeField]
        public float SpeedRunning = 0.15f;

        [Header("Gravity")]
        public float GravityAmount = 0.1f;
        public float GravityMin = -3;
        public float PlayerGravity { get; set; }

        [Header("Jumping")]
        public float JumpingHeight;
        [Tooltip("Falloff speed dump, how fast player should start fall of")]
        public float JumpingFalloff;
        public Vector3 JumpingForce { get; set; }
        [NonSerialized] public Vector3 JumpingVelocity;
      
       

        public bool CursorsLocked { get; set; }

        //Input
        public bool IsHoldingButtonRun { get; set; }
        public bool IsHoldingButtonAim { get; set; }
        public bool IsHoldingButtonFire { get; set; }
        public Vector2 AxisMovement { get; set; }
        public Vector2 AxisLook { get; set; }
        public AudioSource AudioSource { get; set; }
        public CharacterKinematics CharacterKinematics { get; set; }

        //Weapon data
        public WeaponBehaviour EquippedWeapon { get; set; }
        public WeaponAttachmentManagerBehaviour WeaponAttachmentManager { get; set; }
        public ScopeBehaviour EquippedWeaponScope { get; set; }
        public MagazineBehaviour EquippedWeaponMagazine { get; set; }
        public float LastShotTime { get; set; }
        public int NextWeaponIndex { get; set; }

        public bool IsGrounded { get; set; }
        public bool IsHolstered { get; set; }
        public bool IsAiming { get; set; }
        public bool IsRunning { get; set; }

        public int LayerHolster { get; set; }
        public int LayerActions { get; set; }
        public int LayerOverlay { get; set; }
    }

}

