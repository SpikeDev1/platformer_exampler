using System;
using Model.Signals;
using UnityEngine;
using Zenject;

namespace Gameplay.Round
{
    public class TokensCollectorHandler : IInitializable, IDisposable
    {
        private readonly SignalBus bus;
        private readonly TokensCollectorInfo info;
        private readonly ILevelTokensModel setup;
        private bool inited;

        public TokensCollectorHandler(TokensCollectorInfo info, SignalBus bus, [InjectOptional] ILevelTokensModel setup)
        {
            this.info = info;
            this.bus = bus;
            this.setup = setup;
        }

        public void Dispose()
        {
            bus.Unsubscribe<TokenCollectedSignal>(OnCollected);
        }

        public void Initialize()
        {
            if (setup.TotalTokens == 0) Debug.LogWarning("TokensCollectorHandler: tokenZones not set and none found");
            info.SetTotal(setup.TotalTokens);
            bus.Subscribe<TokenCollectedSignal>(OnCollected);
            inited = true;
        }

        private void OnCollected()
        {
            if (inited) info.Inc();
        }
    }
}