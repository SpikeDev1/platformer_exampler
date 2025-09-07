using System;
using Systems.Camera;
using Systems.Movement;
using Core.Utilities;
using Gameplay.Actors;
using Model.Configs;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gameplay.Players
{
    public class PlayerFacade : IPlayerDamageFacade, IDisposable, IPlayerSpawnerView
    {
        private readonly IPlayerAnimator anim;
        private readonly IRigidbody2DAdapter body;
        private readonly CompositeDisposable disposable = new CompositeDisposable();
        private readonly HealthConfig healthConfig;
        protected IAudioPlayer audio;
        protected IMoveableActorView moveableView;
        private readonly ITargetCamera targetCamera;

        public PlayerFacade(
            IAudioPlayer audio,
            IPlayerAnimator animator,
            IMoveableActorView moveableView,
            HealthConfig healthConfig,
            Transform root,
            ITargetCamera targetCamera,
            IRigidbody2DAdapter body)
        {
            this.audio = audio;
            anim = animator;
            this.moveableView = moveableView;
            this.healthConfig = healthConfig;
            Transform = root;
            this.targetCamera = targetCamera;
            this.body = body;
        }

        public Transform FollowTarget => targetCamera.Target;

        public Transform Target => targetCamera.Target;

        public void Dispose()
        {
            disposable.Dispose();
        }

        public Transform Transform { get; }

        public IObservable<Unit> RespawnFinished => anim.RespawnFinished;
        public IObservable<Unit> DeathFinished => anim.DeathFinished;

        public void Respawn()
        {
            body.Stop();
            anim.TriggerRespawn();
        }

        public void Respawn(Vector3 newPos)
        {
            body.Teleport(newPos);
            body.Stop();
            anim.TriggerRespawn();
        }

        public void Hurt()
        {
            anim.TriggerHurt();
        }

        public void Death()
        {
            body.Stop();
            audio.PlayOneShot(healthConfig.deathAudio);
            anim.TriggerDie();
        }

        public void Destroy()
        {
            Object.Destroy(Transform.gameObject);
        }
    }
}