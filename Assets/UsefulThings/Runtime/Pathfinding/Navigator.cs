using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UsefulThings.Struct.Vector2IntExtension;

namespace UsefulThings.Pathfinding
{
    public class Navigator
    {
        private Grid grid;

        private Vector2Int CurrentCell { get; }
        private Vector2 CellSize = Vector2.zero;
        private Func<Vector2Int, Vector2Int, bool> isMovementValidFunc;

        public Navigator(Grid grid, Vector2Int currentCell)
        {
            this.grid = grid;
            this.CurrentCell = currentCell;
        }

        public void SetMovementValidation(Func<Vector2Int, Vector2Int, bool> func)
        {
            isMovementValidFunc = func;
        }

        public List<Vector2> CalculateNavigationPoints(Vector2 targetPos)
        {
            Vector2 offsetInCell = new Vector2
            {
                x = CellSize.x / 2,
                y = CellSize.y / 2
            };

            var navigationCells = NavigateToTarget(WorldToCell(targetPos));

            return navigationCells.Any() ? navigationCells
                .Select(c => CellToWorld(c) + offsetInCell)
                .Append(SnapToFloor(targetPos))
                .ToList() : new List<Vector2>();
        }
        
        public List<Vector2> CalculateNavigationPoints(Vector2Int targetCell)
        {
            Vector2 offsetInCell = new Vector2
            {
                x = CellSize.x / 2,
                y = CellSize.y / 2
            };
            
            var navigationCells = NavigateToTarget(targetCell);

            return navigationCells.Any() ? navigationCells
                .Append(targetCell)
                .Select(c => CellToWorld(c) + offsetInCell)
                .ToList() : new List<Vector2>();
        }

        /// <summary>
        /// Finds a path of cells towards some targetCell. Returns empty list if there is no path.
        /// The return includes the starting and target cells.
        /// </summary>
        public List<Vector2Int> CalculateNavigationCells(Vector2Int targetCell)
        {
            var navigationCells = NavigateToTarget(targetCell);
            return navigationCells.Any() ? navigationCells.Append(targetCell).ToList() : new List<Vector2Int>();
        }
        
        /// <summary>
        /// Finds a path of cells towards some targetPosition. Returns empty list if there is no path.
        /// The return includes the starting and target positions.
        /// </summary>
        public List<Vector2Int> CalculateNavigationCells(Vector2 targetPos)
        {
            Vector2Int targetCell = WorldToCell(targetPos);
            var navigationCells = NavigateToTarget(targetCell);
            return navigationCells.Any() ? navigationCells.Append(targetCell).ToList() : new List<Vector2Int>();
        }
        
        private IEnumerable<Vector2Int> NavigateToTarget(Vector2Int targetCell)
        {
            Vector2Int startCell = CurrentCell;
            if (startCell == targetCell) return Enumerable.Empty<Vector2Int>().Append(targetCell);
            
            const int maxDistance = 100;
            var knownCells = new Dictionary<Vector2Int, Tuple<int, int, int, Vector2Int>>();
            var unvisited = new HashSet<Vector2Int>();
            bool isPathFound = false;
            
            int startHeuristic = Vector2IntUtils.ManhattanDistance(startCell, targetCell);
            var startData = new Tuple<int, int, int, Vector2Int>(startHeuristic, 0, startHeuristic, startCell);
            knownCells[startCell] = startData;
            unvisited.Add(startCell);

            while (unvisited.Count > 0)
            {
                Vector2Int currentCell = unvisited.Aggregate((left, right) =>
                    knownCells[left].Item1 < knownCells[right].Item1 ? left : right);
                unvisited.Remove(currentCell);
                var data = knownCells[currentCell];

                // If we have reached the destination, stop and return the results.
                if (data.Item3 == 0)
                {
                    isPathFound = true;
                    break;
                }

                foreach (Vector2Int neighbourCell in currentCell.GetNeighbouring())
                {
                    // Here we can do any check to see if the neighbouring cell is valid to move to
                    bool canMoveTo = isMovementValidFunc == null || isMovementValidFunc(currentCell, neighbourCell);

                    if (canMoveTo)
                    {
                        int travelledDistance = data.Item2 + 1;
                        int heuristic = Vector2IntUtils.ManhattanDistance(neighbourCell, targetCell);
                        int f = travelledDistance + heuristic;
                    
                        if (travelledDistance < maxDistance)
                        {
                            if (!knownCells.ContainsKey(neighbourCell) 
                                || (knownCells.ContainsKey(neighbourCell) && f < knownCells[neighbourCell].Item1))
                            {
                                knownCells[neighbourCell] = new Tuple<int, int, int, Vector2Int>(f, travelledDistance, heuristic, currentCell);
                                unvisited.Add(neighbourCell);
                            }
                        }
                    }
                }
            }
            
            if (!isPathFound)
                return new List<Vector2Int>();

            var result = new LinkedList<Vector2Int>();
            Vector2Int cell = targetCell;

            while (cell != startCell)
            {
                if (knownCells.ContainsKey(cell))
                {
                    cell = knownCells[cell].Item4;
                    result.AddFirst(cell);
                }
                else
                {
                    return Enumerable.Empty<Vector2Int>();
                }
            }

            return result;
        }
        
        private Vector2 SnapToFloor(Vector2 pos)
        {
            Vector2Int cell = WorldToCell(pos);
            return new Vector2(pos.x, cell.y + CellSize.y / 2);
        }

        private Vector2Int WorldToCell(Vector2 pos)
        {
            return (Vector2Int) grid.WorldToCell(pos);
        }

        private Vector2 CellToWorld(Vector2Int cell)
        {
            return grid.CellToWorld((Vector3Int) cell);
        }
    }
}
