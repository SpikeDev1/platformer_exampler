using Systems.Movement;
using Gameplay.Players.FSM;
using UnityEngine;

namespace Gameplay.Players
{
    public readonly struct PlayerDiedSignal
    {
        public readonly IRigidbody2DAdapter Body;
        public readonly Transform Fallback;
        public readonly IPlayerSpawnerView Spawner;
        public readonly IPlayerStateInfo PlayerStateInfo;

        public PlayerDiedSignal(IRigidbody2DAdapter body, Transform fallback, IPlayerSpawnerView spawner,
            IPlayerStateInfo playerStateInfo)
        {
            Body = body;
            Fallback = fallback;
            Spawner = spawner;
            PlayerStateInfo = playerStateInfo;
        }
    }
}