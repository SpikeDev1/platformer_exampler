using UniRx;

namespace Gameplay.Round
{
    public class RoundInfo
    {
        public readonly BoolReactiveProperty Completed = new BoolReactiveProperty(false);
        public readonly ReactiveProperty<string> LevelName = new ReactiveProperty<string>(null);

        public void InitFor(string level)
        {
            LevelName.Value = level;
            Completed.Value = false;
        }

        public void Complete()
        {
            Completed.Value = true;
        }
    }
}