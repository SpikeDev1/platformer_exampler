using Model.Signals;
using Zenject;

namespace Gameplay.Token
{
    public class TokenCollector : ITokenCollector
    {
        private readonly SignalBus bus;

        public TokenCollector(SignalBus bus)
        {
            this.bus = bus;
        }

        public void CollectToken()
        {
            bus.Fire(new TokenCollectedSignal());
        }
    }
}