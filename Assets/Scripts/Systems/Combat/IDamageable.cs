using System;
using UniRx;

namespace Systems.Combat
{
    public interface IDamageable : IDeath
    {
        void Damage(int amount);
        void Kill();
    }

    public interface IDeath
    {
        bool IsAlive { get; }
        IObservable<Unit> Died { get; }
    }
}