using System;
using Gameplay.Actors;
using UniRx;

namespace Gameplay.Players
{
    public interface IPlayerAnimator : IMovementAnimator, IJumpAnimator, IDamageAnimator
    {
        IObservable<Unit> RespawnFinished { get; }
        void TriggerRespawn();
    }
}