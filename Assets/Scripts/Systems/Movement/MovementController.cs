using Systems.Input;
using Gameplay.Actors;
using UnityEngine;
using Zenject;

namespace Systems.Movement
{
    public class MovementController : ITickable
    {
        private const float dead = 0.01f;
        private readonly IMovementAnimator anim;
        private readonly IRigidbody2DAdapter body;
        private readonly IMovementControl control;
        private readonly IJump jump;
        private readonly IMovement2D movement;

        public MovementController(
            IRigidbody2DAdapter body,
            IMovementControl control,
            IMovement2D movement,
            IJump jump,
            IMovementAnimator anim)
        {
            this.body = body;
            this.control = control;
            this.movement = movement;
            this.jump = jump;
            this.anim = anim;
        }

        public void Tick()
        {
            var enabled = control.Enabled;
            var axis = enabled ? control.MoveAxis.Value : 0f;

            if (axis > dead) movement.Right();
            else if (axis < -dead) movement.Left();
            else movement.Stop();

            if (enabled && control.JumpPressed.Value && jump.TryJump(axis))
                anim.TriggerJump();

            if (enabled && control.JumpReleased.Value && body.Velocity.y > 0f)
                body.Velocity = new Vector2(body.Velocity.x, body.Velocity.y * 0.5f);
        }
    }
}