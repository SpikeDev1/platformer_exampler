using Core.Utilities;
using Model.Configs;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    public class Jump2DGroundedAbility : IJump
    {
        private readonly IAudioPlayer audio;
        private readonly IRigidbody2DAdapter body;

        private readonly BoolReactiveProperty enabled = new BoolReactiveProperty(true);
        private readonly MoveConfig move;

        public Jump2DGroundedAbility(IRigidbody2DAdapter body, MoveConfig move, IAudioPlayer audio)
        {
            this.body = body;
            this.move = move;
            this.audio = audio;
        }

        public bool Enabled => enabled.Value;

        public void Enable()
        {
            enabled.Value = true;
        }

        public void Disable()
        {
            enabled.Value = false;
        }

        public bool TryJump(float dir)
        {
            if (!body.IsGrounded) return false;

            var v = body.Velocity;
            if (v.y > 0f) v.y = 0f;
            body.Velocity = v;
            body.AddImpulse(Vector2.up * move.jumpTakeOffSpeed);

            audio.PlayOneShot(move.jumpAudio);
            return true;
        }
    }
}