using NUnit.Framework;
using UnityEngine;
using UniRx;
using Systems.Movement;
using Systems.Input;
using Gameplay.Actors;

namespace Assets.Tests.EditMode
{
    public class MovementControllerTests
    {
        private class FakeBody : IRigidbody2DAdapter
        {
            public bool IsGrounded { get; set; }
            public Vector2 Velocity { get; set; }
            public Vector3 Position => Vector3.zero;
            public void AddImpulse(Vector2 force) { Velocity += force; }
            public void Teleport(Vector3 position) { }
            public void Stop() { Velocity = Vector2.zero; }
        }

        private class FakeControl : IMovementControl
        {
            public bool Enabled { get; private set; } = true;
            public readonly ReactiveProperty<float> moveAxis = new ReactiveProperty<float>(0f);
            public readonly ReactiveProperty<bool> jumpPressed = new ReactiveProperty<bool>(false);
            public readonly ReactiveProperty<bool> jumpReleased = new ReactiveProperty<bool>(false);
            public IReadOnlyReactiveProperty<float> MoveAxis => moveAxis;
            public IReadOnlyReactiveProperty<bool> JumpPressed => jumpPressed;
            public IReadOnlyReactiveProperty<bool> JumpReleased => jumpReleased;
            public void Enable() { Enabled = true; }
            public void Disable() { Enabled = false; }
        }

        private class FakeMovement : IMovement2D
        {
            public float Speed => 1f;
            public int LeftCalls { get; private set; }
            public int RightCalls { get; private set; }
            public int StopCalls { get; private set; }
            public void Left() { LeftCalls++; }
            public void Right() { RightCalls++; }
            public void Stop() { StopCalls++; }
        }

        private class FakeJump : IJump
        {
            public bool Enabled { get; private set; } = true;
            public bool NextTryJumpResult = false;
            public float LastPower { get; private set; }
            public void Enable() { Enabled = true; }
            public void Disable() { Enabled = false; }
            public bool TryJump(float power) { LastPower = power; return NextTryJumpResult; }
        }

        private class FakeAnim : IMovementAnimator
        {
            public int TriggerJumpCalls { get; private set; }
            public void TriggerJump() { TriggerJumpCalls++; }
            public void SetLand(bool v) { }
            public void SetVelocityX(float v) { }
            public void SetVelocityY(float v) { }
        }

        [Test]
        public void DisabledControl_StopsMovement()
        {
            var body = new FakeBody();
            var control = new FakeControl();
            control.Disable();
            var movement = new FakeMovement();
            var jump = new FakeJump();
            var anim = new FakeAnim();
            var sut = new MovementController(body, control, movement, jump, anim);

            sut.Tick();

            Assert.AreEqual(1, movement.StopCalls);
            Assert.AreEqual(0, movement.LeftCalls + movement.RightCalls);
        }

        [Test]
        public void AxisRight_CallsRight()
        {
            var body = new FakeBody();
            var control = new FakeControl();
            control.moveAxis.Value = 0.5f;
            var movement = new FakeMovement();
            var jump = new FakeJump();
            var anim = new FakeAnim();

            var sut = new MovementController(body, control, movement, jump, anim);
            sut.Tick();

            Assert.AreEqual(1, movement.RightCalls);
            Assert.AreEqual(0, movement.LeftCalls);
        }

        [Test]
        public void AxisLeft_CallsLeft()
        {
            var body = new FakeBody();
            var control = new FakeControl();
            control.moveAxis.Value = -0.5f;
            var movement = new FakeMovement();
            var jump = new FakeJump();
            var anim = new FakeAnim();

            var sut = new MovementController(body, control, movement, jump, anim);
            sut.Tick();

            Assert.AreEqual(1, movement.LeftCalls);
            Assert.AreEqual(0, movement.RightCalls);
        }

        [Test]
        public void AxisDeadZone_CallsStop()
        {
            var body = new FakeBody();
            var control = new FakeControl();
            control.moveAxis.Value = 0.005f; // below deadzone 0.01
            var movement = new FakeMovement();
            var jump = new FakeJump();
            var anim = new FakeAnim();

            var sut = new MovementController(body, control, movement, jump, anim);
            sut.Tick();

            Assert.AreEqual(1, movement.StopCalls);
        }

        [Test]
        public void JumpPressed_TryJumpTrue_TriggersAnim()
        {
            var body = new FakeBody();
            var control = new FakeControl();
            control.jumpPressed.Value = true;
            control.moveAxis.Value = 0.25f;
            var movement = new FakeMovement();
            var jump = new FakeJump { NextTryJumpResult = true };
            var anim = new FakeAnim();

            var sut = new MovementController(body, control, movement, jump, anim);
            sut.Tick();

            Assert.AreEqual(1, anim.TriggerJumpCalls);
            Assert.AreEqual(0.25f, jump.LastPower, 1e-5f);
        }

        [Test]
        public void JumpReleased_WhileRising_CutsVelocityYInHalf()
        {
            var body = new FakeBody { Velocity = new Vector2(1f, 4f) };
            var control = new FakeControl();
            control.jumpReleased.Value = true;
            var movement = new FakeMovement();
            var jump = new FakeJump();
            var anim = new FakeAnim();

            var sut = new MovementController(body, control, movement, jump, anim);
            sut.Tick();

            Assert.AreEqual(2f, body.Velocity.y, 1e-5f);
            Assert.AreEqual(1f, body.Velocity.x, 1e-5f);
        }
    }
}

