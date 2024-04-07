using InfimaGames.LowPolyShooterPack;
using UnityEngine;

namespace Ziumper.Shooter
{
    public interface IPlayerStateManager
    { 
        void ChangeState(PlayerState state, PlayerData data);
    }
}

