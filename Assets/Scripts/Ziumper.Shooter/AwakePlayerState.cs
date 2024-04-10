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

            data.CharacterKinematics = context.gameObject.GetComponent<CharacterKinematics>();
            data.Inventory.Init();

            context.States.Firing.Data = data;
            context.States.Aiming.Data = data;
            context.States.Running.Data = data;

            context.States.Firing.RefreshWeaponSetup();
        }

        public override void LateUpdate() { }
        public override void Update() { }
    }

}

