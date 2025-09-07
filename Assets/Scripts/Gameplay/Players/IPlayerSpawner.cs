using System;
using UniRx;
using UnityEngine;

namespace Gameplay.Players
{
    public interface IPlayerSpawnerView
    {
        IObservable<Unit> RespawnFinished { get; }
        void Respawn();
        void Respawn(Vector3 newPos);
    }
}