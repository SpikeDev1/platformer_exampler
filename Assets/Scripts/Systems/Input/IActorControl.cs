using UniRx;

namespace Systems.Input
{
    public interface IActorControl
    {
        bool Enabled { get; }
        void Enable();
        void Disable();
    }
}