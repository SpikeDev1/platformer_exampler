using System;
using UniRx;

namespace Gameplay.Actors
{
    public interface IDamageAnimator
    {
        IObservable<Unit> DeathFinished { get; }
        void TriggerHurt();
        void TriggerDie();
    }

    public interface IMovementAnimator : IJumpAnimator
    {
        void SetLand(bool v);
        void SetVelocityX(float v);
        void SetVelocityY(float v);
    }

    public interface IJumpAnimator
    {
        void TriggerJump();
    }
}