using InfimaGames.LowPolyShooterPack;
using System;
using UnityEngine;
using Ziumper.Shooter.Weapons;

namespace Ziumper.Shooter
{
    [Serializable]
    public class PlayerData
    {
        public class  InputData
        {
            public bool IsHoldingButtonRun;
            public bool IsHoldingButtonAim;
            public bool IsHoldingButtonFire;
            public Vector2 AxisMovement;
            public Vector2 AxisLook;
        }

        public class MovingData
        {
            public float CurrentSpeed;
            public float JumpDirectionValue;
            public AudioClip FootstepsAudio;
            public Vector3 JumpingForce;
            public Vector3 JumpingVelocity;
        }

        public class WeaponData
        {
            public int NextWeaponIndex;
            public float LastShotTime;
            //Weapon data
            public WeaponBehaviour EquippedWeapon;
            public CameraRecoil CameraRecoil;
            public WeaponAttachmentManagerBehaviour WeaponAttachmentManager;
            public ScopeBehaviour EquippedWeaponScope;
            public MagazineBehaviour EquippedWeaponMagazine;
            public WeaponSettings EquippedWeaponSettings;
        }

        public InputData Input;
        public MovingData Move;
        public WeaponData Weapon;

        public PlayerData()
        {
            Input = new InputData();
            Move = new MovingData();
            Weapon = new WeaponData();
        }

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
        public float SpeedWalking = 4f;
        public float SpeedRunning = 8f;
        public float SpeedAiming = 2f;

        [Header("Gravity")]
        public float GravityAmount = 0.1f;
        public float GravityMin = -3;
        public float PlayerGravity { get; set; }

        [Header("Jumping")]
        public float JumpingHeight;
        [Tooltip("Falloff speed dump, how fast player should start fall of")]
        public float JumpingFalloff;
        
        public bool CursorsLocked { get; set; }

        public AudioSource AudioSource { get; set; }
        public CharacterKinematics CharacterKinematics { get; set; }

        //Weapon data
        public CameraRecoil CameraRecoil { get; set; }

        public bool IsGrounded { get; set; }
        public bool IsHolstered { get; set; }
        public bool IsAiming { get; set; }
        public bool IsRunning { get { return SpeedRunning == Move.CurrentSpeed && IsGrounded; } }

        public int LayerHolster { get; set; }
        public int LayerActions { get; set; }
        public int LayerOverlay { get; set; }
    }

}

