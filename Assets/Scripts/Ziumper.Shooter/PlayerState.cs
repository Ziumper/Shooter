using UnityEngine;

namespace Ziumper.Shooter
{
    public abstract class PlayerState
    {
        protected PlayerData data;
        protected PlayerStateManager context;

        public PlayerData Data { set { data = value; } }
        public PlayerStateManager Context { set { context = value; } }

        public virtual void EnterState(PlayerStateManager context, PlayerData data)
        {
            this.data = data;
            this.context = context;
        }

        public virtual void ExitState() { }
        public virtual void FixedUpdate() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
    }
}

