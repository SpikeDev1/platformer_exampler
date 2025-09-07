using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Systems.Combat
{
    public class CombatProxy : MonoBehaviour, IDamageable
    {
        [Inject] private IDamageable impl;

        public bool IsAlive => impl.IsAlive;
        public IObservable<Unit> Died => impl.Died;

        public void Damage(int amount)
        {
            impl.Damage(amount);
        }

        public void Kill()
        {
            impl.Kill();
        }
    }
}