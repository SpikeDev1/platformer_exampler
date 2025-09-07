using System;
using Gameplay.Actors;
using UniRx;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyAnimatorAdapter : MonoBehaviour, IMovementAnimator, IDamageAnimator
    {
        private readonly Subject<Unit> deathFinished = new Subject<Unit>();
        [SerializeField] private Animator animator;

        public IObservable<Unit> DeathFinished => deathFinished;

        public void TriggerHurt()
        {
            if (animator) animator.SetTrigger("hurt");
        }

        public void TriggerDie()
        {
            if (animator) animator.SetTrigger("death");
        }

        public void SetLand(bool v)
        {
            if (animator) animator.SetBool("grounded", v);
        }

        public void SetVelocityX(float v)
        {
            if (animator) animator.SetFloat("velocityX", v);
        }

        public void SetVelocityY(float v)
        {
            if (animator) animator.SetFloat("velocityY", v);
        }

        public void TriggerJump()
        {
        }

        public void AnimEvent_DeathFinished()
        {
            deathFinished.OnNext(Unit.Default);
        }
    }
}