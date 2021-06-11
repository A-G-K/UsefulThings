using NUnit.Framework;
using UnityEngine;
using UsefulThings.Direction;

namespace UsefulThings.Tests.Runtime
{
    public class QuadDirectionTests
    {
        [Test]
        public void Clockwise()
        {
            Assert.AreEqual(QuadDirection.South, QuadDirection.East.RotateClockwise());
            Assert.AreEqual(QuadDirection.North, QuadDirection.West.RotateClockwise());
        }
        
        [Test]
        public void AntiClockwise()
        {
            Assert.AreEqual(QuadDirection.South, QuadDirection.West.RotateAntiClockwise());
            Assert.AreEqual(QuadDirection.West, QuadDirection.North.RotateAntiClockwise());
        }

        [Test]
        public void Opposite()
        {
            Assert.AreEqual(QuadDirection.South, QuadDirection.North.Opposite());
            Assert.AreEqual(QuadDirection.East, QuadDirection.West.Opposite());
            Assert.AreEqual(QuadDirection.West, QuadDirection.East.Opposite());
            Assert.AreEqual(QuadDirection.North, QuadDirection.South.Opposite());
        }

        [Test]
        public void MaskContains()
        {
            QuadDirectionMask mask = QuadDirection.East.ToMask();
            mask |= QuadDirectionMask.South;
            
            Assert.IsTrue(mask.Contains(QuadDirection.South));
            Assert.IsTrue(mask.Contains(QuadDirection.East));
        }

        [Test]
        public void VectorToQuadDirection()
        {
            Assert.AreEqual(QuadDirection.East, QuadDirectionUtil.From(Vector2.up, new Vector2(1f, 0f)));
            Assert.AreEqual(QuadDirection.North, QuadDirectionUtil.From(Vector2.up, new Vector2(0.4f, 4f)));
            Assert.AreEqual(QuadDirection.West, QuadDirectionUtil.From(Vector2.up, new Vector2(-15f, 5f)));
        }
    }
}