using UnityEngine;
using Zenject;

namespace Core.Utilities
{
    public class SpriteAnimatorService : ITickable
    {
        private readonly ISpriteAnimatorModel model;

        public SpriteAnimatorService(ISpriteAnimatorModel model)
        {
            this.model = model;
        }

        public void Tick()
        {
            if (model.Length <= 0) return;
            if (Time.time < model.NextTime) return;

            var next = model.Frame.Value + 1;
            if (next >= model.Length)
            {
                if (model.Loop) next = 0;
                else next = model.Length - 1;
            }

            model.SetFrame(next);
            var rate = model.FrameRate <= 0f ? 0.0001f : model.FrameRate;
            model.NextTime += 1f / rate;
        }
    }
}