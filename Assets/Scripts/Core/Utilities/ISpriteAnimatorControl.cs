using UnityEngine;

namespace Core.Utilities
{
    public interface ISpriteAnimatorControl
    {
        void Play(Sprite[] sequence, float frameRate, int startFrame = 0);
        void PlayLoop(Sprite[] sequence, float frameRate, int startFrame = 0);
        void PlayLoopRandom(Sprite[] sequence, float frameRate, bool randomStart);
        void Stop();
    }
}