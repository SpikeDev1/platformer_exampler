using NUnit.Framework;
using Zenject;
using Gameplay.Round;
using Model.Signals;

namespace Assets.Tests.EditMode
{
    public class TokensCollectorHandlerTests
    {
        private class FakeLevelTokensModel : ILevelTokensModel
        {
            public int TotalTokens { get; set; }
        }

        private static (TokensCollectorHandler handler, TokensCollectorInfo info, SignalBus bus) NewSut(int total)
        {
            var container = new DiContainer();
            SignalBusInstaller.Install(container);
            container.DeclareSignal<TokenCollectedSignal>();
            var bus = container.Resolve<SignalBus>();

            var info = new TokensCollectorInfo();
            var setup = new FakeLevelTokensModel { TotalTokens = total };
            var sut = new TokensCollectorHandler(info, bus, setup);
            return (sut, info, bus);
        }

        [Test]
        public void Initialize_SetsTotal_AndResetsCollected()
        {
            var (sut, info, _) = NewSut(total: 5);
            sut.Initialize();

            Assert.AreEqual(5, info.Total);
            Assert.AreEqual(0, info.Collected);
        }

        [Test]
        public void Collect_IncrementsUntilTotal_ThenClamps()
        {
            var (sut, info, bus) = NewSut(total: 2);
            sut.Initialize();

            bus.Fire(new TokenCollectedSignal());
            Assert.AreEqual(1, info.Collected);
            bus.Fire(new TokenCollectedSignal());
            Assert.AreEqual(2, info.Collected);
            // extra fires do not exceed Total
            bus.Fire(new TokenCollectedSignal());
            Assert.AreEqual(2, info.Collected);
        }

        [Test]
        public void BeforeInitialize_FiresIgnored()
        {
            var (sut, info, bus) = NewSut(total: 3);
            bus.Fire(new TokenCollectedSignal());
            Assert.AreEqual(0, info.Collected);
        }

        [Test]
        public void AfterDispose_Unsubscribed_NoMoreIncrements()
        {
            var (sut, info, bus) = NewSut(total: 3);
            sut.Initialize();
            bus.Fire(new TokenCollectedSignal());
            Assert.AreEqual(1, info.Collected);

            sut.Dispose();
            bus.Fire(new TokenCollectedSignal());
            Assert.AreEqual(1, info.Collected);
        }
    }
}

