using UniRx;

namespace Systems.Input
{
    public interface IMovementControl : IActorControl
    {
        IReadOnlyReactiveProperty<float> MoveAxis { get; }
        IReadOnlyReactiveProperty<bool> JumpPressed { get; }
        IReadOnlyReactiveProperty<bool> JumpReleased { get; }
    }
}