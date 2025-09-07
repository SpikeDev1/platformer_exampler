using System;
using UniRx;
using UnityEngine;

namespace Gameplay.Players
{
    public class PlayerAnimatorAdapter : MonoBehaviour, IPlayerAnimator
    {
        private readonly Subject<Unit> deathFinished = new Subject<Unit>();
        private readonly Subject<Unit> hurtFinished = new Subject<Unit>();
        private readonly Subject<Unit> jumpFinished = new Subject<Unit>();
        private readonly Subject<Unit> landFinished = new Subject<Unit>();
        private readonly Subject<Unit> respawnFinished = new Subject<Unit>();
        [SerializeField] private Animator animator;
        public IObservable<Unit> HurtFinished => hurtFinished;
        public IObservable<Unit> JumpFinished => jumpFinished;
        public IObservable<Unit> LandFinished => landFinished;

        public IObservable<Unit> DeathFinished => deathFinished;
        public IObservable<Unit> RespawnFinished => respawnFinished;

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

        public void TriggerHurt()
        {
            if (animator) animator.SetTrigger("hurt");
        }

        public void TriggerJump()
        {
        }

        public void TriggerRespawn()
        {
            if (animator)
            {
                SetVelocityX(0f);
                SetVelocityY(0f);
                SetLand(true);

                animator.SetBool("dead", false);
                animator.SetTrigger("spawn");
            }
        }

        public void TriggerDie()
        {
            SetDead(true);
        }

        public void SetDead(bool v)
        {
            if (animator) animator.SetBool("dead", v);
        }

        public void TriggerVictory()
        {
            if (animator) animator.SetTrigger("victory");
        }

        public void AnimEvent_DeathFinished()
        {
            deathFinished.OnNext(Unit.Default);
        }

        public void AnimEvent_HurtFinished()
        {
            hurtFinished.OnNext(Unit.Default);
        }

        public void AnimEvent_RespawnFinished()
        {
            respawnFinished.OnNext(Unit.Default);
        }

        public void AnimEvent_JumpFinished()
        {
            jumpFinished.OnNext(Unit.Default);
        }

        public void AnimEvent_LandFinished()
        {
            landFinished.OnNext(Unit.Default);
        }
    }
}