using UniRx;

namespace Core.Utilities
{
    public interface ISpriteAnimatorModel
    {
        IReadOnlyReactiveProperty<int> Frame { get; }
        int Length { get; set; }
        float FrameRate { get; set; }
        bool Loop { get; set; }
        float NextTime { get; set; }
        void ResetFrame(int startFrame, float now);
        void SetFrame(int value);
    }
}