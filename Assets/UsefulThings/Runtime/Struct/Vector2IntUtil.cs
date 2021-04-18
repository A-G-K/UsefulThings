using System;
using System.Collections.Generic;
using UnityEngine;
using UsefulThings.Struct.Vector2IntExtension;

namespace UsefulThings.Struct
{
    public static class Vector2IntUtils
    {
        public static int ManhattanDistance(Vector2Int from, Vector2Int to)
        {
            return Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);
        }

        public static IEnumerable<Vector2Int> GetNearbyCells(Vector2Int selectedCell, int limit)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            queue.Enqueue(selectedCell);
            visited.Add(selectedCell);

            while (queue.Count != 0)
            {
                Vector2Int current = queue.Dequeue();
                visited.Add(current);
            
                foreach (Vector2Int neighbourCell in current.GetNeighbouring())
                {
                    if (!visited.Contains(neighbourCell) && ManhattanDistance(selectedCell, neighbourCell) <= limit)
                    {
                        queue.Enqueue(neighbourCell);
                    }
                }
            }

            visited.Remove(selectedCell);
            return visited;
        }
    }
    
    namespace Vector2IntExtension
    {
        public static class Vector2IntExtensions
        {
            public static IEnumerable<Vector2Int> GetNeighbouring(this Vector2Int vector)
            {
                yield return new Vector2Int(vector.x, vector.y + 1);
                yield return new Vector2Int(vector.x + 1, vector.y);
                yield return new Vector2Int(vector.x, vector.y - 1);
                yield return new Vector2Int(vector.x - 1, vector.y);
            }
        }
    }
}