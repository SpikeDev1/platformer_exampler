using System;
using Model.Configs;
using UniRx;

namespace Systems.Combat
{
    public class HealthAbility : IHealth
    {
        protected readonly ReactiveProperty<int> current = new ReactiveProperty<int>(0);
        protected readonly Subject<Unit> died = new Subject<Unit>();
        protected int max;

        public HealthAbility(HealthConfig cfg)
        {
            Init(cfg.amount);
        }

        public int Max => max;
        public int Current => current.Value;
        public IObservable<Unit> Died => died;

        public void Init(int value)
        {
            max = value;
            current.Value = max;
        }

        public void Decrease(int amount)
        {
            if (current.Value <= 0) return;
            var v = current.Value - amount;
            if (v <= 0)
            {
                current.Value = 0;
                died.OnNext(Unit.Default);
            }
            else current.Value = v;
        }

        public void HealFull()
        {
            current.Value = max;
        }
    }
}