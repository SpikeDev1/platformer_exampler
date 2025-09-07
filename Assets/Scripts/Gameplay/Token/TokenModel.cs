using UniRx;

namespace Gameplay.Token
{
    public class TokenModel : IPickupModel
    {
        private readonly BoolReactiveProperty collected = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> Collected => collected;

        public void SetCollected(bool value)
        {
            collected.Value = value;
        }

        public void Reset()
        {
            collected.Value = false;
        }
    }
}