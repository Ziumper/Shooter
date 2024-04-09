using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ziumper.Shooter
{
    public class PlayerCharacter : CharacterBehaviour
    {
        [SerializeField] private PlayerData data;
        private PlayerStateManager stateManager;
        
        protected override void Awake()
        {
            stateManager = GetComponent<PlayerStateManager>();
            stateManager.ChangeStateTo(stateManager.Awake, data);
        }

        protected override void Start()
        {
            stateManager.ChangeStateTo(stateManager.Start, data);
        }

        public override Camera GetCameraWorld() => data.CameraWorld;

        public override InventoryBehaviour GetInventory() => data.Inventory;

        public override bool IsCrosshairVisible() => !data.IsAiming && !data.IsHolstered;
        public override bool IsRunning() => data.IsRunning;

        public override bool IsAiming() => data.IsAiming;
        public override bool IsCursorLocked() => data.CursorsLocked;

        public override bool IsTutorialTextVisible() => false;

        public override Vector2 GetInputMovement() => data.AxisMovement;
        public override Vector2 GetInputLook() => data.AxisLook;

        /// <summary>
        /// Fire.
        /// </summary>
        public void OnTryFire(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            //Switch.
            switch (context)
            {
                //Started.
                case { phase: InputActionPhase.Started }:
                    //Hold.
                    data.IsHoldingButtonFire = true;
                    break;
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    stateManager.StateEvents.OnSingleFire.Invoke();
                    break;
                //Canceled.
                case { phase: InputActionPhase.Canceled }:
                    //Stop Hold.
                    data.IsHoldingButtonFire = false;
                    stateManager.StateEvents.OnSingleFireCancel.Invoke();
                    break;
            }
        }
        /// <summary>
        /// Reload.
        /// </summary>
        public void OnTryPlayReload(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            ////Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Play Animation.
                    stateManager.StateEvents.OnReloadStart.Invoke();                    
                    break;
            }
        }

        /// <summary>
        /// Inspect.
        /// </summary>
        public void OnTryInspect(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            
            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Play Animation.
                    stateManager.StateEvents.OnInspectStart.Invoke();
                    break;
            }
        }
        /// <summary>
        /// Aiming.
        /// </summary>
        public void OnTryAiming(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            //Switch.
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    //Started.
                    data.IsHoldingButtonAim = true;
                    break;
                case InputActionPhase.Canceled:
                    //Canceled.
                    data.IsHoldingButtonAim = false;
                    break;
            }
        }

        /// <summary>
        /// Holster.
        /// </summary>
        public void OnTryHolster(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            //Switch.
            switch (context.phase)
            {
                //Performed.
                case InputActionPhase.Performed:
                    //Check.
                    
                    break;
            }
        }
        /// <summary>
        /// Run. 
        /// </summary>
        public void OnTryRun(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            //Switch.
            switch (context.phase)
            {
                //Started.
                case InputActionPhase.Started:
                    //Start.
                    data.IsHoldingButtonRun = true;
                    break;
                //Canceled.
                case InputActionPhase.Canceled:
                    //Stop.
                    data.IsHoldingButtonRun = false;
                    break;
            }
        }
        /// <summary>
        /// Next Inventory Weapon.
        /// </summary>
        public void OnTryInventoryNext(InputAction.CallbackContext context)
        {
            //Block while the cursor is unlocked.
            if (!data.CursorsLocked)
                return;

            //Null Check.
            if (data.Inventory == null)
                return;

            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;
                    stateManager.StateEvents.OnInventoryNext.Invoke(scrollValue);
                    break;
            }
        }

        public void OnLockCursor(InputAction.CallbackContext context)
        {
            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Update the cursor's state.
                    stateManager.StateEvents.OnCursorUpdate.Invoke();
                    break;
            }
        }

        /// <summary>
        /// Movement.
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            //Read.
            stateManager.StateEvents.OnMove.Invoke(context.ReadValue<Vector2>());
        }
        /// <summary>
        /// Look.
        /// </summary>
        public void OnLook(InputAction.CallbackContext context)
        {
            //Read.
            stateManager.StateEvents.OnLook.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            //Switch.
            switch (context)
            {
                //Performed.
                case { phase: InputActionPhase.Performed }:
                    //Update the cursor's state.
                    stateManager.StateEvents.OnJump.Invoke();
                    break;
            }
        }

        public override void EjectCasing()
        {
            //Notify the weapon.
            if (data.EquippedWeapon != null)
            {
                stateManager.StateEvents.OnEjectCasing.Invoke();
            }
        }

        public override void FillAmmunition(int amount)
        {
            //Notify the weapon to fill the ammunition by the amount.
            if (data.EquippedWeapon != null)
            {
                stateManager.StateEvents.OnFillAmmunniton.Invoke(amount);
            }
        }

        public override void SetActiveMagazine(int active)
        {
            stateManager.StateEvents.OnSetActiveMagazine.Invoke(active);
        }

        public override void AnimationEndedReload()
        {
            stateManager.StateEvents.OnReloadEnd.Invoke();
        }

        public override void AnimationEndedInspect()
        {
            stateManager.StateEvents.OnInspectEnd.Invoke();
        }
        public override void AnimationEndedHolster()
        {
            stateManager.StateEvents.OnHolsteringEnd.Invoke();
        }

        
      
    }

}

