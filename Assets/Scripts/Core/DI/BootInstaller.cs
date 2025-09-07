using Core.Startup;
using Core.Utilities;
using Gameplay.Round;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Core.DI
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private GameConfigSO gameConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(gameConfig).AsSingle();
            Container.Bind<SceneLoader>().AsSingle();

            Container.Bind<RoundInfo>().AsSingle();

            Container.BindInterfacesTo<GameStarterService>().AsSingle();
            Container.BindInterfacesTo<NextLevelService>().AsSingle();
        }
    }
}