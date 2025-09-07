using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Gameplay.Players.FSM
{
    public class PlayerStateMachine : ITickable, IInitializable, IDisposable, IPlayerStateInfo
    {
        private readonly Dictionary<PlayerStateId, IPlayerState> map;
        private CancellationTokenSource cts;
        private IPlayerState current;

        public PlayerStateMachine(List<IPlayerState> states)
        {
            map = states.ToDictionary(s => s.Id, s => s);
        }

        public void Dispose()
        {
            (current as PlayerStateBase)?.ClearSubscriptions();
            cts?.Cancel();
            cts?.Dispose();
        }

        public void Initialize()
        {
            Set(PlayerStateId.Spawn).Forget();
        }

        public PlayerStateId Id { get; set; }

        public void Tick()
        {
            current?.Tick();
        }

        public async UniTask Set(PlayerStateId id)
        {
            var next = map[id];
            if (current == next) return;

            (current as PlayerStateBase)?.ClearSubscriptions();

            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();
            var token = cts.Token;

            current = next;
            current.Setup(this, token);
            await current.Enter();
        }
    }
}