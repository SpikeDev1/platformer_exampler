using System;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Gameplay.Round;
using Model.Configs;
using UniRx;
using Zenject;

namespace Core.Startup
{
    public class NextLevelService : IInitializable, IDisposable
    {
        private readonly GameConfigSO config;
        private readonly CompositeDisposable d = new CompositeDisposable();
        private readonly SceneLoader loader;
        private readonly RoundInfo round;

        public NextLevelService(SceneLoader loader, GameConfigSO config, RoundInfo round)
        {
            this.loader = loader;
            this.config = config;
            this.round = round;
        }

        public void Dispose()
        {
            d.Dispose();
        }

        public void Initialize()
        {
            round.Completed.Where(v => v).Subscribe(_ => LoadNext().Forget()).AddTo(d);
        }

        public async UniTask LoadNext()
        {
            round.InitFor(config.gameplayScene);
            await loader.LoadSingleAsync(config.gameplayScene);
        }
    }
}