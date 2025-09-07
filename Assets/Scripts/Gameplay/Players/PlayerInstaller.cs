using Systems.Camera;
using Systems.Combat;
using Systems.Input;
using Systems.Movement;
using Core.Utilities;
using Gameplay.Actors;
using Gameplay.Players.FSM;
using Gameplay.Token;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // bind date for Player
            Container.Bind<MoveConfig>().FromResolve("Player").AsCached();
            Container.Bind<HealthConfig>().FromResolve("Player").AsCached();
            Container.Bind<DamageConfig>().FromResolve("Player").AsCached();

            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<SpriteRenderer>().FromComponentOnRoot().AsSingle();

            //health and damage
            Container.Bind<IHealth>().To<HealthAbility>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerDamageHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerControlService>().AsSingle();

            // animation and audio
            Container.Bind<IAudioPlayer>().To<AudioSourceAdapter>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<PlayerAnimatorAdapter>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<KinematicMotor2D>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<MoveableActor>().AsSingle();

            // movement
            Container.Bind<IMovement2D>().To<Movement2DAbility>().AsSingle();
            Container.Bind<IJump>().To<Jump2DGroundedAbility>().AsSingle();
            Container.BindInterfacesTo<MovementController>().AsSingle();

            //view
            Container.Bind<IMoveableActorView>().To<MoveableActorView>().AsSingle();
            Container.BindInterfacesTo<TargetForCamera>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<PlayerFacade>().AsSingle();

            // behaviour
            Container.Bind<ITokenCollector>().To<TokenCollector>().AsSingle();

            Container.Bind<PlayerInfo>().FromComponentOnRoot().AsSingle();

            //control states
            Container.Bind<IPlayerState>().To<SpawnState>().AsSingle();
            Container.Bind<IPlayerState>().To<FallState>().AsSingle();
            Container.Bind<IPlayerState>().To<StayState>().AsSingle();
            Container.Bind<IPlayerState>().To<RunState>().AsSingle();
            Container.Bind<IPlayerState>().To<DeathState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle();
        }
    }
}