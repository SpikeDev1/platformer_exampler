using UnityEngine;
using Zenject;

namespace Gameplay.Actors
{
    public class MoveableActorView : IMoveableActorView
    {
        protected IMovementAnimator anim;
        protected SpriteRenderer sprite;

        public void LookLeft()
        {
            sprite.flipX = false;
        }

        public void LookRight()
        {
            sprite.flipX = true;
        }

        public void Present(bool status, float normalizedSpeedHorz, float velocityVertc)
        {
            anim.SetLand(status);

            anim.SetVelocityX(normalizedSpeedHorz);
            anim.SetVelocityY(velocityVertc);
        }

        [Inject]
        protected void Construct(SpriteRenderer sprite, IMovementAnimator anim)
        {
            this.sprite = sprite;
            this.anim = anim;
        }
    }
}