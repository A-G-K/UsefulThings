using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UsefulThings.Direction
{
    public enum QuadDirection
    {
        North, East, South, West
    }

    public static class QuadDirectionExtensions
    {
        public static QuadDirection Opposite(this QuadDirection direction)
        {
            return (QuadDirection) (((int) direction + 2) % 4);
        }

        public static QuadDirection RotateClockwise(this QuadDirection direction)
        {
            return (QuadDirection) (((int) direction + 1) % 4);
        }
        
        public static QuadDirection RotateAntiClockwise(this QuadDirection direction)
        {
            int nextDirection = (int) direction - 1;
            // Custom modulo function to handle negative numbers
            return (QuadDirection) (nextDirection - 4 * Mathf.FloorToInt(nextDirection / 4f));
        }
        
        public static Vector2Int ToVector2Int(this QuadDirection direction)
        {
            switch (direction)
            {
                case QuadDirection.East:
                    return Vector2Int.right;
                case QuadDirection.South:
                    return Vector2Int.down;
                case QuadDirection.West:
                    return Vector2Int.left;
                case QuadDirection.North:
                    return Vector2Int.up;
            }
            
            throw new ArgumentOutOfRangeException($"{nameof(QuadDirection)} {direction} is unhandled!");
        }

        public static Vector2 ToVector2(this QuadDirection direction)
        {
            switch (direction)
            {
                case QuadDirection.East:
                    return Vector2.right;
                case QuadDirection.South:
                    return Vector2.down;
                case QuadDirection.West:
                    return Vector2.left;
                case QuadDirection.North:
                    return Vector2.up;
            }
            
            throw new ArgumentOutOfRangeException($"{nameof(QuadDirection)} {direction} is unhandled!");
        }

        public static QuadDirectionMask ToMask(this QuadDirection direction)
        {
            switch (direction)
            {
                case QuadDirection.East:
                    return QuadDirectionMask.East;
                case QuadDirection.South:
                    return QuadDirectionMask.South;
                case QuadDirection.West:
                    return QuadDirectionMask.West;
                case QuadDirection.North:
                    return QuadDirectionMask.North;
            }
            
            throw new ArgumentOutOfRangeException($"{nameof(QuadDirection)} {direction} is unhandled!");
        }
        
        public static OctoDirection ToOctoDirection(this QuadDirection direction)
        {
            switch (direction)
            {
                case QuadDirection.East:
                    return OctoDirection.East;
                case QuadDirection.South:
                    return OctoDirection.South;
                case QuadDirection.West:
                    return OctoDirection.West;
                case QuadDirection.North:
                    return OctoDirection.North;
            }

            throw new ArgumentOutOfRangeException($"{nameof(QuadDirection)} {direction} is unhandled!");
        }

        public static bool IsVertical(this QuadDirection direction)
        {
            return direction == QuadDirection.North || direction == QuadDirection.South;
        }

        public static bool IsHorizontal(this QuadDirection direction)
        {
            return direction == QuadDirection.East || direction == QuadDirection.West;
        }
    }

    [Flags]
    public enum QuadDirectionMask
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8,
    }

    public static class QuadDirectionMaskExtensions
    {
        public static bool Contains(this QuadDirectionMask mask, QuadDirection direction)
        {
            return (mask & direction.ToMask()) != 0;
        }
        
        public static IEnumerable<QuadDirectionMask> Split(this QuadDirectionMask mask)
        {
            if ((mask & QuadDirectionMask.North) != 0)
            {
                yield return QuadDirectionMask.North;
            }
            
            if ((mask & QuadDirectionMask.East) != 0)
            {
                yield return QuadDirectionMask.East;
            }
            
            if ((mask & QuadDirectionMask.West) != 0)
            {
                yield return QuadDirectionMask.West;
            }
            
            if ((mask & QuadDirectionMask.South) != 0)
            {
                yield return QuadDirectionMask.South;
            }
        }
    }

    public static class QuadDirectionUtil
    {
        public static QuadDirection From(Vector2 from, Vector2 to)
        {
            float angle = Vector2.SignedAngle(from, to);

            if (angle >= 45 && angle < 135)
            {
                return QuadDirection.West;
            }
            
            if (angle < -45 && angle >= -135)
            {
                return QuadDirection.East;
            } 
            
            if (angle >= -45 && angle < 45)
            {
                return QuadDirection.North;
            }

            return QuadDirection.South;
        }

        public static QuadDirectionMask GetMaskFrom(params QuadDirection[] directions)
        {
            return GetMaskFrom(directions.AsEnumerable());
        }

        public static QuadDirectionMask GetMaskFrom(IEnumerable<QuadDirection> directions)
        {
            QuadDirectionMask mask = QuadDirectionMask.None;

            foreach (QuadDirection direction in directions)
            {
                mask |= direction.ToMask();
            }

            return mask;
        }
    }
}