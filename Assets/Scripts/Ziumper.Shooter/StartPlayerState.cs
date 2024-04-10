using UnityEngine;

namespace Ziumper.Shooter
{
    public class StartPlayerState : PlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            //Cache a reference to the holster layer's index.
            data.LayerHolster = data.CharacterAnimator.GetLayerIndex("Layer Holster");
            //Cache a reference to the action layer's index.
            data.LayerActions = data.CharacterAnimator.GetLayerIndex("Layer Actions");
            //Cache a reference to the overlay layer's index.
            data.LayerOverlay = data.CharacterAnimator.GetLayerIndex("Layer Overlay");
            data.AudioSource = context.GetComponent<AudioSource>();

            context.PlayerEvents.OnCursorUpdate.AddListener(() =>
            {
                data.CursorsLocked = !data.CursorsLocked;

                //Update cursor visibility.
                Cursor.visible = !data.CursorsLocked;
                //Update cursor lock state.
                Cursor.lockState = data.CursorsLocked ? CursorLockMode.Locked : CursorLockMode.None;
            });

            context.PlayerEvents.OnMove.AddListener((moveVector) => data.Input.AxisMovement = data.CursorsLocked ? moveVector : default);
            context.PlayerEvents.OnLook.AddListener((lookVector) => data.Input.AxisLook = data.CursorsLocked ? lookVector : default);
            context.PlayerEvents.OnFillAmmunniton.AddListener((amount) => data.EquippedWeapon.FillAmmunition(amount));
            context.PlayerEvents.OnSetActiveMagazine.AddListener((active) => data.EquippedWeaponMagazine.gameObject.SetActive(active != 0));
            context.PlayerEvents.OnEjectCasing.AddListener(() => data.EquippedWeapon.EjectCasing());

            context.ChangeStateTo(context.States.Default, data);
        }

    }

}

