using UniRx;

namespace Gameplay.Token
{
    public interface IPickupModel
    {
        IReadOnlyReactiveProperty<bool> Collected { get; }
        void SetCollected(bool value);
        void Reset();
    }
}