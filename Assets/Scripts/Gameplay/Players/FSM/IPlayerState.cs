using System.Threading;
using Cysharp.Threading.Tasks;

namespace Gameplay.Players.FSM
{
    public interface IPlayerState
    {
        PlayerStateId Id { get; }
        void Setup(PlayerStateMachine fsm, CancellationToken ct);
        UniTask Enter();
        void Tick();
    }

    public interface IPlayerStateInfo
    {
        PlayerStateId Id { get; set; }
    }
}