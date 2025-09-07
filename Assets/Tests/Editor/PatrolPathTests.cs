using NUnit.Framework;
using UnityEngine;
using Core.Utilities;

namespace Assets.Tests.EditMode
{
    public class PatrolPathTests
    {
        [Test]
        public void Mover_AtCreation_EqualsStartWorldPosition()
        {
            var go = new GameObject("PatrolPathTest");
            var path = go.AddComponent<PatrolPath>();
            path.startPosition = new Vector2(-2f, 1f);
            path.endPosition = new Vector2(3f, 1f);
            go.transform.position = new Vector3(5f, 0f, 0f);

            var mover = path.CreateMover(2f);
            var pos = mover.Position; // near t=0 -> should be start

            var expected = go.transform.TransformPoint(path.startPosition);
            Assert.AreEqual(expected.x, pos.x, 1e-3f);
            Assert.AreEqual(expected.y, pos.y, 1e-3f);
        }

        [Test]
        public void Mover_ZeroLengthPath_IsConstantAtStart()
        {
            var go = new GameObject("PatrolPathZero");
            var path = go.AddComponent<PatrolPath>();
            path.startPosition = new Vector2(1f, 2f);
            path.endPosition = path.startPosition; // zero distance

            var mover = path.CreateMover(5f);
            var p1 = mover.Position;
            var p2 = mover.Position;

            var expected = go.transform.TransformPoint(path.startPosition);
            Assert.AreEqual(expected.x, p1.x, 1e-3f);
            Assert.AreEqual(expected.y, p1.y, 1e-3f);
            Assert.AreEqual(p1.x, p2.x, 1e-3f);
            Assert.AreEqual(p1.y, p2.y, 1e-3f);
        }
    }
}

