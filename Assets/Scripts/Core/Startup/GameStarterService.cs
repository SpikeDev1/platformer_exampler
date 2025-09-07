using Core.Utilities;
using Cysharp.Threading.Tasks;
using Gameplay.Round;
using Model.Configs;
using Zenject;

namespace Core.Startup
{
    public class GameStarterService : IInitializable
    {
        private readonly GameConfigSO config;
        private readonly SceneLoader loader;
        private readonly RoundInfo round;

        public GameStarterService(SceneLoader loader, GameConfigSO config, RoundInfo round)
        {
            this.loader = loader;
            this.config = config;
            this.round = round;
        }

        public void Initialize()
        {
            Run().Forget();
        }

        private async UniTaskVoid Run()
        {
            round.InitFor(config.gameplayScene);
            await loader.LoadSingleAsync(config.gameplayScene);
        }
    }
}