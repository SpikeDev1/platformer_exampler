using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Gameplay.Players.FSM
{
    public abstract class PlayerStateBase : IPlayerState
    {
        protected readonly CompositeDisposable d = new CompositeDisposable();
        protected CancellationToken ct;
        protected PlayerStateMachine fsm;

        public abstract PlayerStateId Id { get; }

        public virtual void Setup(PlayerStateMachine f, CancellationToken token)
        {
            fsm = f;
            ct = token;
        }

        public virtual UniTask Enter()
        {
            fsm.Id = Id;
            return UniTask.CompletedTask;
        }

        public virtual void Tick()
        {
        }

        internal void ClearSubscriptions()
        {
            d.Clear();
        }

        protected UniTask TransitTo(PlayerStateId id)
        {
            return fsm.Set(id);
        }
    }
}