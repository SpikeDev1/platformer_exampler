using Systems.Movement;
using Model.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Actors
{
    public class MoveableActor : IFixedTickable
    {
        private IRigidbody2DAdapter body;
        private MoveConfig move;
        private IMoveableActorView view;

        private float vxSmoothed;

        public void FixedTick()
        {
            var vx = body.Velocity.x;

            vxSmoothed = SmoothNextPos(vx);

            if (vxSmoothed > move.sensitivityOffset)
            {
                view.LookLeft();
            }
            else if (vxSmoothed < -move.sensitivityOffset)
            {
                view.LookRight();
            }

            var smoothHorizontalMove = SmoothVelocity();

            view.Present(body.IsGrounded, smoothHorizontalMove, body.Velocity.y);
        }

        [Inject]
        protected void Construct(IRigidbody2DAdapter body, MoveConfig move, IMoveableActorView actorView)
        {
            this.body = body;
            this.move = move;
            view = actorView;
        }

        public float SmoothNextPos(float nextPos)
        {
            return Mathf.Lerp(vxSmoothed, nextPos, Time.deltaTime * move.smooth);
        }

        public float SmoothVelocity()
        {
            return Mathf.Abs(vxSmoothed) / Mathf.Max(0.0001f, move.maxSpeed);
        }
    }
}