using System;
using Systems.Combat;
using Core.Utilities;
using Gameplay.Actors;
using Model.Configs;
using UniRx;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyDamageHandler : IDamageable, IInitializable, IDisposable
    {
        private readonly IDamageAnimator anim;
        private readonly IAudioPlayer audio;
        private readonly CompositeDisposable d = new CompositeDisposable();
        private readonly Subject<Unit> died = new Subject<Unit>();
        private readonly IHealth health;
        private readonly HealthConfig healthCfg;

        public EnemyDamageHandler(IHealth health, IAudioPlayer audio, HealthConfig healthCfg,
            InjectOptionalAttribute opt = null, IDamageAnimator anim = null)
        {
            this.health = health;
            this.audio = audio;
            this.healthCfg = healthCfg;
            this.anim = anim;
        }

        public bool IsAlive => health.Current > 0;
        public IObservable<Unit> Died => died;

        public void Damage(int amount)
        {
            health.Decrease(amount);
            if (health.Current > 0)
            {
                audio.PlayOneShot(healthCfg.ouchAudio);
            }
        }

        public void Kill()
        {
            Damage(health.Current);
        }

        public void Dispose()
        {
            d.Dispose();
        }

        public void Initialize()
        {
            health.Died.Subscribe(_ => { died.OnNext(Unit.Default); }).AddTo(d);
        }
    }
}