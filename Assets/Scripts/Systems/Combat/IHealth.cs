using System;
using UniRx;

namespace Systems.Combat
{
    public interface IHealth
    {
        int Max { get; }
        int Current { get; }
        IObservable<Unit> Died { get; }
        void Init(int max);
        void Decrease(int amount);
        void HealFull();
    }
}