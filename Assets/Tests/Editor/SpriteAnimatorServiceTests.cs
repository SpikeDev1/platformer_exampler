using NUnit.Framework;
using Core.Utilities;
using UnityEngine;

namespace Assets.Tests.EditMode
{
    public class SpriteAnimatorServiceTests
    {
        private static SpriteAnimatorModel NewModel(
            int length,
            int startFrame,
            float frameRate = 12f,
            bool loop = true,
            float? nextTime = null)
        {
            var m = new SpriteAnimatorModel
            {
                Length = length,
                FrameRate = frameRate,
                Loop = loop,
            };
            m.ResetFrame(startFrame, Time.time);
            if (nextTime.HasValue) m.NextTime = nextTime.Value;
            return m;
        }

        [Test]
        public void Tick_NoFrames_DoesNothing()
        {
            var model = NewModel(length: 0, startFrame: 2, frameRate: 12f, loop: true, nextTime: Time.time - 1f);
            var svc = new SpriteAnimatorService(model);

            var beforeFrame = model.Frame.Value;
            var beforeNext = model.NextTime;
            svc.Tick();

            Assert.AreEqual(beforeFrame, model.Frame.Value);
            Assert.AreEqual(beforeNext, model.NextTime);
        }

        [Test]
        public void Tick_BeforeNextTime_DoesNothing()
        {
            var model = NewModel(length: 5, startFrame: 2, frameRate: 10f, loop: true, nextTime: Time.time + 10f);
            var svc = new SpriteAnimatorService(model);

            var beforeFrame = model.Frame.Value;
            var beforeNext = model.NextTime;
            svc.Tick();

            Assert.AreEqual(beforeFrame, model.Frame.Value);
            Assert.AreEqual(beforeNext, model.NextTime);
        }

        [Test]
        public void Tick_AdvanceFrame_UpdatesNextTime_ByRate()
        {
            var model = NewModel(length: 5, startFrame: 2, frameRate: 10f, loop: true, nextTime: Time.time - 0.01f);
            var svc = new SpriteAnimatorService(model);

            var beforeNext = model.NextTime;
            svc.Tick();

            Assert.AreEqual(3, model.Frame.Value);
            Assert.AreEqual(beforeNext + 0.1f, model.NextTime, 1e-5f);
        }

        [Test]
        public void Tick_LoopsAtEnd_WhenLoopTrue()
        {
            var model = NewModel(length: 5, startFrame: 4, frameRate: 12f, loop: true, nextTime: Time.time - 1f);
            var svc = new SpriteAnimatorService(model);

            svc.Tick();

            Assert.AreEqual(0, model.Frame.Value);
        }

        [Test]
        public void Tick_ClampsAtEnd_WhenLoopFalse()
        {
            var model = NewModel(length: 5, startFrame: 4, frameRate: 12f, loop: false, nextTime: Time.time - 1f);
            var svc = new SpriteAnimatorService(model);

            var beforeNext = model.NextTime;
            svc.Tick();

            Assert.AreEqual(4, model.Frame.Value);
            Assert.Greater(model.NextTime, beforeNext);
        }

        [Test]
        public void Tick_ZeroFrameRate_UsesMinimumRate()
        {
            var model = NewModel(length: 3, startFrame: 1, frameRate: 0f, loop: true, nextTime: Time.time - 1f);
            var svc = new SpriteAnimatorService(model);

            var beforeNext = model.NextTime;
            svc.Tick();

            Assert.AreEqual(2, model.Frame.Value);
            // 1 / 0.0001f == 10000f
            Assert.AreEqual(beforeNext + 10000f, model.NextTime, 1e-3f);
        }
    }
}

