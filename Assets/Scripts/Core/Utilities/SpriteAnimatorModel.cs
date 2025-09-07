using UniRx;
using UnityEngine;

namespace Core.Utilities
{
    public class SpriteAnimatorModel : ISpriteAnimatorModel
    {
        private readonly IntReactiveProperty frame = new IntReactiveProperty(0);
        public IReadOnlyReactiveProperty<int> Frame => frame;
        public int Length { get; set; }
        public float FrameRate { get; set; } = 12f;
        public bool Loop { get; set; } = true;
        public float NextTime { get; set; }

        public void ResetFrame(int startFrame, float now)
        {
            frame.Value = Mathf.Max(0, startFrame);
            NextTime = now;
        }

        public void SetFrame(int value)
        {
            frame.Value = value;
        }
    }
}