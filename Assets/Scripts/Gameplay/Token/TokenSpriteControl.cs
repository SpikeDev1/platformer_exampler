using Core.Utilities;
using UnityEngine;
using Zenject;

namespace Gameplay.Token
{
    internal class TokenSpriteControl : ISpriteAnimatorControl
    {
        private readonly ISpriteAnimatorModel anim;

        [Inject]
        public TokenSpriteControl(ISpriteAnimatorModel anim)
        {
            this.anim = anim;
        }

        public void Play(Sprite[] sequence, float frameRate, int startFrame = 0)
        {
            var len = sequence != null ? sequence.Length : 0;
            anim.Length = len;
            anim.FrameRate = frameRate;
            anim.Loop = false;
            anim.ResetFrame(startFrame, Time.time);
        }

        public void PlayLoop(Sprite[] sequence, float frameRate, int startFrame = 0)
        {
            var len = sequence != null ? sequence.Length : 0;
            anim.Length = len;
            anim.FrameRate = frameRate;
            anim.Loop = true;
            anim.ResetFrame(startFrame, Time.time);
        }

        public void PlayLoopRandom(Sprite[] sequence, float frameRate, bool randomStart)
        {
            var len = sequence != null ? sequence.Length : 0;
            var start = randomStart && len > 0 ? Random.Range(0, len) : 0;
            anim.Length = len;
            anim.FrameRate = frameRate;
            anim.Loop = true;
            anim.ResetFrame(start, Time.time);
        }

        public void Stop()
        {
            anim.Length = 0;
        }
    }
}