using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Ziumper.Shooter
{
    public class AwakePlayerState : PlayerState
    {
        public override void EnterState(PlayerStateManager context, PlayerData data)
        {
            base.EnterState(context, data);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            data.CursorsLocked = true;
            data.CameraRecoil = context.GetComponentInChildren<CameraRecoil>();
            data.CharacterKinematics = context.gameObject.GetComponent<CharacterKinematics>();
            data.Inventory.Init();

            InitalizeCoreStatesData();
            
            context.PlayerStates.Firing.RefreshWeaponSetup();
        }

        private void InitalizeCoreStatesData()
        {
            context.PlayerStates.Firing.Data = data;
            context.PlayerStates.Aiming.Data = data;
            context.PlayerStates.Running.Data = data;
            context.PlayerStates.NextWeapon.Data = data;
            context.PlayerStates.NextWeapon.Context = context;
        }
    }

}

