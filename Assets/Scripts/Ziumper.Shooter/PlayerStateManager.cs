using Codice.Client.Common;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ziumper.Shooter
{
    public class PlayerStateManager : MonoBehaviour
    {
        public class PlayerStateEvents
        {
            public UnityEvent<Vector2> OnMove = new();
            public UnityEvent<Vector2> OnLook = new();
            public UnityEvent OnJump = new();

            public UnityEvent OnInspectStart = new();
            public UnityEvent OnInspectEnd = new();
            public UnityEvent OnReloadStart = new();
            public UnityEvent OnReloadEnd = new();
            public UnityEvent<float> OnInventoryNext = new();
            public UnityEvent OnHolsteringEnd = new();
            public UnityEvent<int> OnSetActiveMagazine = new();
            public UnityEvent<int> OnFillAmmunniton = new();
            public UnityEvent OnEjectCasing = new();
            public UnityEvent OnSingleFire = new();
            public UnityEvent OnSingleFireCancel = new();

            public UnityEvent OnCursorUpdate = new();
        }

        public class StatesContainer
        {
            public StartPlayerState Start = new();
            public AwakePlayerState Awake = new();
            public DefaultPlayerState Default = new();
            public RunningPlayerState Running = new();
            public AimingPlayerState Aiming = new();
            public InspectingPlayerState Inspecting = new();
            public NextInventoryPlayerState NextWeapon = new();
            public ReloadingPlayerState Reloading = new();
            public FiringPlayerState Firing = new();
            public AimingFirePlayerState AimingFire = new();
        }

        private PlayerState current;
        private PlayerState previous;
     
        public PlayerState Previous { get { return previous; } }

        public PlayerStateEvents PlayerEvents = new PlayerStateEvents();
        public StatesContainer PlayerStates = new StatesContainer();
        
        public void ChangeStateTo(PlayerState newState, PlayerData data)
        {
            if (current != null) { current.ExitState(); }
            
            previous = current;
            current = newState;

            current.EnterState(this, data);
        }
        
        private void Update()
        {
            current.Update();
        }

        private void LateUpdate()
        {
            current.LateUpdate();
        }

        private void FixedUpdate()
        {
            current.FixedUpdate();
        }
    }

}

