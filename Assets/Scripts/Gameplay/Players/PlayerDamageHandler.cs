using System;
using Systems.Combat;
using UniRx;
using Zenject;

namespace Gameplay.Players
{
    public class PlayerDamageHandler : IDamageable, IInitializable, IDisposable
    {
        private readonly CompositeDisposable d = new CompositeDisposable();
        private readonly Subject<Unit> died = new Subject<Unit>();
        private readonly IHealth health;
        private readonly IPlayerDamageFacade _damageView;

        public PlayerDamageHandler(IHealth health, IPlayerDamageFacade damageView)
        {
            this.health = health;
            this._damageView = damageView;
        }

        public bool IsAlive => health.Current > 0;
        public IObservable<Unit> Died => died;

        public void Damage(int amount)
        {
            var willDie = health.Current - amount <= 0;
            health.Decrease(amount);
            if (!willDie)
            {
                _damageView.Hurt();
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
            health.Died.Subscribe(_ =>
            {
                _damageView.Death();
                died.OnNext(Unit.Default);
            }).AddTo(d);
        }
    }
}