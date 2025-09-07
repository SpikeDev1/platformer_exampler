using Systems.Combat;
using Gameplay.Enemies;
using Gameplay.Players;
using Gameplay.Round;
using Gameplay.Token;
using Model.Signals;
using Zenject;

namespace Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            //signals
            Container.DeclareSignal<PlayerReachedEndLevel>();
            Container.DeclareSignal<TokenCollectedSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();

            //data
            Container.Bind<PlayerFactory>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle();
            Container.Bind<ICombatResolver>().To<CombatResolver>().AsSingle();
            Container.Bind<TokensCollectorInfo>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoundSetup>().FromComponentInHierarchy().AsSingle().IfNotBound();

            // handlers
            Container.BindInterfacesTo<TokensCollectorHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoundManager>().AsSingle();
            Container.BindInterfacesTo<TokenBootstrapper>().AsSingle();
            Container.BindSignal<PlayerDiedSignal>().ToMethod<RoundManager>(x => x.OnPlayerDied).FromResolve();
        }
    }
}