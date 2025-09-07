using System;
using UniRx;
using UnityEngine;

namespace Gameplay.Players
{
    public interface IPlayerDamageFacade : IPlayerSpawnerView
    {
        IObservable<Unit> DeathFinished { get; }
        Transform Transform { get; }
        void Hurt();
        void Death();
        void Destroy();
    }
}