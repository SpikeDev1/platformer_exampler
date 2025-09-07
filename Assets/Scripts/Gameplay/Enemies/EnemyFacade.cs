using Core.Utilities;
using Gameplay.Actors;
using Model.Configs;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyFacade : IEnemyView
    {
        private readonly IDamageAnimator anim;
        private readonly HealthConfig healthConfig;
        protected IAudioPlayer audio;
        protected IMoveableActorView moveableActor;
        private readonly Transform root;

        private EnemyFacade(IAudioPlayer audio, IDamageAnimator animator, IMoveableActorView movableActor,
            HealthConfig healthConfig, Transform root)
        {
            this.audio = audio;
            moveableActor = moveableActor;
            this.healthConfig = healthConfig;
            this.root = root;
            anim = animator;
        }

        public void Death()
        {
            anim.TriggerDie();
            audio.PlayOneShot(healthConfig.deathAudio);
        }

        public void Destroy()
        {
            Object.Destroy(root.gameObject);
        }
    }
}