using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UsefulThings.Direction
{
    public enum OctoDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public static class OctoDirectionExtensions
    {
        public static OctoDirection Opposite(this OctoDirection direction)
        {
            return (OctoDirection) (((int) direction + 4) % 8);
        }

        public static OctoDirection RotateClockwise(this OctoDirection direction)
        {
            return (OctoDirection) (((int) direction + 1) % 8);
        }

        public static OctoDirection RotateAntiClockwise(this OctoDirection direction)
        {
            int nextDirection = (int) direction - 1;
            // Custom modulo function to handle negative numbers
            return (OctoDirection) (nextDirection - 8 * Mathf.FloorToInt(nextDirection / 8f));
        }

        public static Vector2Int ToVector2Int(this OctoDirection direction)
        {
            switch (direction)
            {
                case OctoDirection.North:
                    return Vector2Int.up;
                case OctoDirection.NorthEast:
                    return Vector2Int.up + Vector2Int.right;
                case OctoDirection.East:
                    return Vector2Int.right;
                case OctoDirection.SouthEast:
                    return Vector2Int.down + Vector2Int.right;
                case OctoDirection.South:
                    return Vector2Int.down;
                case OctoDirection.SouthWest:
                    return Vector2Int.down + Vector2Int.left;
                case OctoDirection.West:
                    return Vector2Int.left;
                case OctoDirection.NorthWest:
                    return Vector2Int.up + Vector2Int.left;
            }

            throw new ArgumentOutOfRangeException($"{nameof(OctoDirection)} {direction} is unhandled!");
        }

        public static Vector2 ToVector2(this OctoDirection direction)
        {
            switch (direction)
            {
                case OctoDirection.North:
                    return Vector2.up;
                case OctoDirection.NorthEast:
                    return Vector2.up + Vector2.right;
                case OctoDirection.East:
                    return Vector2.right;
                case OctoDirection.SouthEast:
                    return Vector2.down + Vector2.right;
                case OctoDirection.South:
                    return Vector2.down;
                case OctoDirection.SouthWest:
                    return Vector2.down + Vector2.left;
                case OctoDirection.West:
                    return Vector2.left;
                case OctoDirection.NorthWest:
                    return Vector2.up + Vector2.left;
            }

            throw new ArgumentOutOfRangeException($"{nameof(OctoDirection)} {direction} is unhandled!");
        }

        public static OctoDirectionMask ToMask(this OctoDirection direction)
        {
            switch (direction)
            {
                case OctoDirection.North:
                    return OctoDirectionMask.North;
                case OctoDirection.NorthEast:
                    return OctoDirectionMask.NorthEast;
                case OctoDirection.East:
                    return OctoDirectionMask.East;
                case OctoDirection.SouthEast:
                    return OctoDirectionMask.SouthEast;
                case OctoDirection.South:
                    return OctoDirectionMask.South;
                case OctoDirection.SouthWest:
                    return OctoDirectionMask.SouthWest;
                case OctoDirection.West:
                    return OctoDirectionMask.West;
                case OctoDirection.NorthWest:
                    return OctoDirectionMask.NorthWest;
            }

            throw new ArgumentOutOfRangeException($"{nameof(OctoDirection)} {direction} is unhandled!");
        }

        public static QuadDirection ToQuadDirection(this OctoDirection direction)
        {
            if (direction.IsDiagonal())
            {
                throw new InvalidOperationException($"Cannot turn {direction} into a {nameof(QuadDirection)}");
            }

            switch (direction)
            {
                case OctoDirection.East:
                    return QuadDirection.East;
                case OctoDirection.South:
                    return QuadDirection.South;
                case OctoDirection.West:
                    return QuadDirection.West;
                case OctoDirection.North:
                    return QuadDirection.North;
            }

            throw new ArgumentOutOfRangeException($"{nameof(OctoDirection)} {direction} is unhandled!");
        }

        public static bool IsHorizontal(this OctoDirection direction)
        {
            return direction == OctoDirection.East || direction == OctoDirection.West;
        }

        public static bool IsVertical(this OctoDirection direction)
        {
            return direction == OctoDirection.North || direction == OctoDirection.South;
        }

        public static bool IsDiagonal(this OctoDirection direction)
        {
            return direction == OctoDirection.NorthEast 
                   || direction == OctoDirection.NorthWest 
                   || direction == OctoDirection.SouthWest 
                   || direction == OctoDirection.SouthEast;
        }
    }

    [Flags]
    public enum OctoDirectionMask
    {
        None = 0,
        North = 1,
        NorthEast = 2,
        East = 4,
        SouthEast = 8,
        South = 16,
        SouthWest = 32,
        West = 64,
        NorthWest = 128
    }

    public static class OctoDirectionMaskExtensions
    {
        public static bool Contains(this OctoDirectionMask mask, OctoDirection direction) =>
            (mask & direction.ToMask()) != 0;

        public static IEnumerable<OctoDirection> Split(this OctoDirectionMask mask)
        {
            if ((mask & OctoDirectionMask.North) != 0)
            {
                yield return OctoDirection.North;
            }

            if ((mask & OctoDirectionMask.NorthEast) != 0)
            {
                yield return OctoDirection.NorthEast;
            }

            if ((mask & OctoDirectionMask.East) != 0)
            {
                yield return OctoDirection.East;
            }

            if ((mask & OctoDirectionMask.SouthEast) != 0)
            {
                yield return OctoDirection.SouthEast;
            }

            if ((mask & OctoDirectionMask.South) != 0)
            {
                yield return OctoDirection.South;
            }

            if ((mask & OctoDirectionMask.SouthWest) != 0)
            {
                yield return OctoDirection.SouthWest;
            }

            if ((mask & OctoDirectionMask.West) != 0)
            {
                yield return OctoDirection.West;
            }

            if ((mask & OctoDirectionMask.NorthWest) != 0)
            {
                yield return OctoDirection.NorthWest;
            }
        }

        public static bool HasVertical(this OctoDirectionMask mask)
        {
            return (mask & OctoDirectionMask.North) != 0 || (mask & OctoDirectionMask.South) != 0;
        }

        public static bool HasHorizontal(this OctoDirectionMask mask)
        {
            return (mask & OctoDirectionMask.East) != 0 || (mask & OctoDirectionMask.West) != 0;
        }

        public static bool HasDiagonal(this OctoDirectionMask mask)
        {
            return (mask & OctoDirectionMask.NorthEast) != 0 
                   || (mask & OctoDirectionMask.NorthWest) != 0 
                   || (mask & OctoDirectionMask.SouthEast) != 0 
                   || (mask & OctoDirectionMask.SouthWest) != 0;
        }
    }

    public static class OctoDirectionUtil
    {
        public static OctoDirection From(Vector2 from, Vector2 to)
        {
            float angle = Vector2.SignedAngle(from, to);

            if (angle >= 22.5f && angle < 67.5f)
            {
                return OctoDirection.NorthWest;
            }

            if (angle >= 67.5f && angle < 112.5f)
            {
                return OctoDirection.West;
            }

            if (angle >= 112.5f && angle < 157.5f)
            {
                return OctoDirection.SouthWest;
            }

            if (angle < -22.5f && angle >= -67.5f)
            {
                return OctoDirection.NorthEast;
            }

            if (angle < -67.5f && angle >= -112.5f)
            {
                return OctoDirection.East;
            }

            if (angle < -112.5f && angle >= -157.5f)
            {
                return OctoDirection.SouthEast;
            }

            if (angle >= -22.5f && angle < 22.5f)
            {
                return OctoDirection.North;
            }

            return OctoDirection.South;
        }

        public static OctoDirectionMask GetMaskFrom(params OctoDirection[] directions)
        {
            return GetMaskFrom(directions.AsEnumerable());
        }

        public static OctoDirectionMask GetMaskFrom(IEnumerable<OctoDirection> directions)
        {
            OctoDirectionMask mask = OctoDirectionMask.None;

            foreach (OctoDirection direction in directions)
            {
                mask |= direction.ToMask();
            }

            return mask;
        }
    }
}