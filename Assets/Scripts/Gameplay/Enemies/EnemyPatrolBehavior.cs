using System;
using Systems.Input;
using Systems.Movement;
using Core.Utilities;
using Model.Configs;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class EnemyPatrolBehavior : ITickable, IMovementControl, IDisposable
    {
        private readonly ReactiveProperty<float> axis = new ReactiveProperty<float>(0f);

        private readonly BoolReactiveProperty enabledProp = new BoolReactiveProperty(true);
        private readonly ReactiveProperty<bool> jumpDown = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> jumpUp = new ReactiveProperty<bool>(false);

        private IRigidbody2DAdapter body;

        private float lastDir;
        private MoveConfig move;
        private PatrolPath.Mover mover;
        private IPatrolPath path;
        private PatrolConfig patrol = new PatrolConfig();

        public void Dispose()
        {
            enabledProp.Dispose();
            axis.Dispose();
            jumpDown.Dispose();
            jumpUp.Dispose();
        }

        public bool Enabled => enabledProp.Value;

        public void Enable()
        {
            enabledProp.Value = true;
        }

        public void Disable()
        {
            enabledProp.Value = false;
        }

        public IReadOnlyReactiveProperty<float> MoveAxis => axis;
        public IReadOnlyReactiveProperty<bool> JumpPressed => jumpDown;
        public IReadOnlyReactiveProperty<bool> JumpReleased => jumpUp;

        public void Tick()
        {
            if (!Enabled)
            {
                ResetInput();
                return;
            }

            var dir = ComputeDirection();
            axis.Value = dir;

            var needJump = ShouldJump(dir, body.IsGrounded, body.Velocity.x);
            jumpDown.Value = needJump;
            jumpUp.Value = false;

            lastDir = dir;
        }

        [Inject]
        private void Construct(IRigidbody2DAdapter body, MoveConfig move, PatrolConfig patrolCfg, IPatrolPath path)
        {
            this.body = body;
            this.move = move;
            if (patrolCfg != null) patrol = patrolCfg;
            this.path = path;
        }

        private float ComputeDirection()
        {
            if (path == null) return 0f;
            EnsureMover();

            if (mover == null) return 0f;

            var dx = mover.Position.x - body.Position.x;
            if (dx > patrol.turnDistance) return 1f;
            if (dx < -patrol.turnDistance) return -1f;
            if (Mathf.Abs(dx) <= patrol.stopDistance) return 0f;
            return lastDir;
        }

        private void EnsureMover()
        {
            if (mover == null) mover = path.CreateMover(move.maxSpeed);
        }

        private bool ShouldJump(float dir, bool isGrounded, float velocityX)
        {
            var groundedOk = !patrol.requireGroundedForJump || isGrounded;
            return Mathf.Abs(dir) > patrol.minMoveIntent && groundedOk &&
                   Mathf.Abs(velocityX) < patrol.maxGroundSpeedForJump;
        }

        private void ResetInput()
        {
            axis.Value = 0f;
            jumpDown.Value = false;
            jumpUp.Value = false;
        }
    }
}