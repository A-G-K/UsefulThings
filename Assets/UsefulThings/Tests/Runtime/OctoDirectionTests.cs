using NUnit.Framework;
using UnityEngine;
using UsefulThings.Direction;

namespace UsefulThings.Tests.Runtime
{
    public class OctoDirectionTests
    {
        [Test]
        public void Clockwise()
        {
            Assert.AreEqual(OctoDirection.South, OctoDirection.SouthEast.RotateClockwise());
            Assert.AreEqual(OctoDirection.North, OctoDirection.NorthWest.RotateClockwise());
        }
        
        [Test]
        public void AntiClockwise()
        {
            Assert.AreEqual(OctoDirection.SouthWest, OctoDirection.West.RotateAntiClockwise());
            Assert.AreEqual(OctoDirection.NorthWest, OctoDirection.North.RotateAntiClockwise());
        }

        [Test]
        public void Opposite()
        {
            Assert.AreEqual(OctoDirection.South, OctoDirection.North.Opposite());
            Assert.AreEqual(OctoDirection.East, OctoDirection.West.Opposite());
            Assert.AreEqual(OctoDirection.NorthEast, OctoDirection.SouthWest.Opposite());
            Assert.AreEqual(OctoDirection.NorthWest, OctoDirection.SouthEast.Opposite());
        }

        [Test]
        public void MaskContains()
        {
            var mask = OctoDirection.East.ToMask();
            mask |= OctoDirectionMask.South;
            mask |= OctoDirectionMask.NorthEast;
            
            Assert.IsTrue(mask.Contains(OctoDirection.South));
            Assert.IsTrue(mask.Contains(OctoDirection.East));
            Assert.IsTrue(mask.Contains(OctoDirection.NorthEast));
        }

        [Test]
        public void VectorToOctoDirection()
        {
            Assert.AreEqual(OctoDirection.East, OctoDirectionUtil.From(Vector2.up, new Vector2(1f, 0f)));
            Assert.AreEqual(OctoDirection.North, OctoDirectionUtil.From(Vector2.up, new Vector2(0.4f, 4f)));
            Assert.AreEqual(OctoDirection.West, OctoDirectionUtil.From(Vector2.up, new Vector2(-15f, 5f)));
        }
    }
}