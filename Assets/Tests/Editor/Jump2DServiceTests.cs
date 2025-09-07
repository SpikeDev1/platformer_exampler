
using Systems.Movement;
using NUnit.Framework;

namespace Assets.Tests.EditMode
{
    using UnityEngine;
    using Model.Configs;

    public class Jump2DAbilityTests
    {
        class FakeBody : IRigidbody2DAdapter
        {
            public bool IsGrounded { get; set; }
            public Vector2 Velocity { get; set; }
            public Vector3 Position { get; set; }
            public void AddImpulse(Vector2 impulse) { Velocity += impulse; }
            public void Teleport(Vector3 pos) { Position = pos; }
            public void Stop()
            {
                Velocity = Vector2.zero;
            }
        }

        class FakeAudio : Core.Utilities.IAudioPlayer
        {
            public AudioClip LastClip;
            public string LastId;
            public void PlayOneShot(AudioClip clip) { LastClip = clip; }
            public void PlayOneShot(string idAudio) { LastId = idAudio; }
        }

        MoveConfig NewMove(float jumpSpeed, string id = null)
        {
            return new MoveConfig { jumpTakeOffSpeed = jumpSpeed, jumpAudio = id ?? "jump_sfx" };
        }

        [Test]
        public void TryJump_NotGrounded_False_NoChanges()
        {
            var body = new FakeBody { IsGrounded = false, Velocity = new Vector2(1f, -0.5f) };
            var audio = new FakeAudio();
            var svc = new Jump2DGroundedAbility(body, NewMove(10f, "jump_sfx"), audio);

            var ok = svc.TryJump(1f);

            Assert.False(ok);
            Assert.AreEqual(new Vector2(1f, -0.5f), body.Velocity);
            Assert.IsNull(audio.LastId);
            Assert.IsNull(audio.LastClip);
        }

        [Test]
        public void TryJump_Grounded_UpwardClamped_ThenImpulse()
        {
            var body = new FakeBody { IsGrounded = true, Velocity = new Vector2(0.2f, 3f) };
            var audio = new FakeAudio();
            var svc = new Jump2DGroundedAbility(body, NewMove(8f, "jump_sfx"), audio);

            var ok = svc.TryJump(1f);

            Assert.True(ok);
            Assert.AreEqual(8f, body.Velocity.y, 1e-5f);
            Assert.AreEqual("jump_sfx", audio.LastId);
        }

        [Test]
        public void TryJump_Grounded_DownwardPlusImpulse()
        {
            var body = new FakeBody { IsGrounded = true, Velocity = new Vector2(-0.1f, -2f) };
            var audio = new FakeAudio();
            var svc = new Jump2DGroundedAbility(body, NewMove(6f, "jump_sfx"), audio);

            var ok = svc.TryJump(1f);

            Assert.True(ok);
            Assert.AreEqual(4f, body.Velocity.y, 1e-5f);
            Assert.AreEqual("jump_sfx", audio.LastId);
        }
    }
}
