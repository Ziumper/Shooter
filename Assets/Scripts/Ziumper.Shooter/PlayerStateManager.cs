using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ziumper.Shooter
{
    public class PlayerStateManager : MonoBehaviour
    {
        [Serializable]
        public class PlayerStateEvents
        {
            [Header("Moving Events")]
            public UnityEvent<Vector2> OnMove;
            public UnityEvent<Vector2> OnLook;

            [Header("Weapon Events")]
            public UnityEvent OnInspectStart;
            public UnityEvent OnInspectEnd;
            public UnityEvent OnReloadStart;
            public UnityEvent OnReloadEnd;
            public UnityEvent<float> OnInventoryNext;
            public UnityEvent OnHolsteringEnd;
            public UnityEvent<int> OnSetActiveMagazine;
            public UnityEvent<int> OnFillAmmunniton;
            public UnityEvent OnEjectCasing;
            public UnityEvent OnSingleFire;
            public UnityEvent OnSingleFireCancel;

            [Header("Utility Events")]
            public UnityEvent OnCursorUpdate;
        }

        public PlayerStateEvents StateEvents;

        public StartPlayerState Start = new();
        public AwakePlayerState Awake = new();
        public DefaultPlayerState Default = new();
        public RunningPlayerState Running = new();
        public AimingPlayerState Aiming = new();
        public InspectingPlayerState Inspecting = new();
        public InventoryNextPlayerState Holstering = new();
        public ReloadingPlayerState Reloading = new();
        public FiringPlayerState Firing = new();
        public AimingFirePlayerState AimingFire = new();

        private PlayerState state;
        private PlayerData data;

        public void ChangeStateTo(PlayerState state, PlayerData data)
        {
            this.data = data;

            if (this.state != null) { this.state.ExitState(); }
            
            this.state = state;
            this.state.EnterState(this, this.data);
        }
        
        private void Update()
        {
            state.Update();
        }

        private void LateUpdate()
        {
            state.LateUpdate();
        }

        private void OnCollisionStay()
        {
            state.OnCollisitonStay();
        }

        private void FixedUpdate()
        {
            state.FixedUpdate();
        }
    }

}

