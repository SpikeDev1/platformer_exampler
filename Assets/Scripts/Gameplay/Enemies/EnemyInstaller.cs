using Systems.Combat;
using Systems.Movement;
using Core.Utilities;
using Gameplay.Actors;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // bind date for Enemy
            Container.Bind<MoveConfig>().FromResolve("Enemy").AsCached();
            Container.Bind<HealthConfig>().FromResolve("Enemy").AsCached();
            Container.Bind<DamageConfig>().FromResolve("Enemy").AsCached();
            Container.Bind<PatrolConfig>().FromResolve("Enemy").AsCached();

            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<SpriteRenderer>().FromComponentOnRoot().AsSingle();

            //health and damage
            Container.Bind<IHealth>().To<HealthAbility>().AsSingle();
            Container.Bind<TouchDamageHandler>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyDamageHandler>().AsSingle();

            // animation
            Container.Bind<IRigidbody2DAdapter>().To<KinematicMotor2D>().FromComponentOnRoot().AsSingle();
            Container.Bind<IAudioPlayer>().To<AudioSourceAdapter>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<EnemyAnimatorAdapter>().FromComponentOnRoot().AsSingle();

            // movement
            Container.Bind<IMovement2D>().To<Movement2DAbility>().AsSingle();
            Container.Bind<IJump>().To<Jump2DGroundedAbility>().AsSingle();
            Container.Bind<MoveableActor>().AsSingle();
            Container.BindInterfacesTo<MovementController>().AsSingle();

            // view enemy
            Container.Bind<IMoveableActorView>().To<MoveableActorView>().AsSingle();
            Container.BindInterfacesTo<EnemyFacade>().AsSingle();

            // behaviour
            Container.Bind<IPatrolPath>().To<PatrolPathAdapter>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesTo<EnemyPatrolBehavior>().AsSingle();
            Container.BindInterfacesTo<EnemyDeath>().AsSingle();

            Container.Bind<EnemyInfo>().FromComponentOnRoot().AsSingle();
        }
    }
}