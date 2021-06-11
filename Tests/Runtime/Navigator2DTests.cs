using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UsefulThings.Pathfinding;
using UsefulThings.Struct;

namespace UsefulThings.Tests.Runtime
{
    public class Navigator2DTests
    {
        private void AssertPathfinding(Vector2Int start, Vector2Int target, IEnumerable<Vector2Int> foundCells, IEnumerable<Vector2Int> exceptCells)
        {
            Vector2Int current = start;
            
            var exceptCellsSet = new HashSet<Vector2Int>(exceptCells);

            CollectionAssert.IsNotEmpty(foundCells, "No path found!");
            Assert.AreEqual(start, foundCells.First());

            // Skip the first cell because the first cell should be the start cell
            foreach (Vector2Int nextCell in foundCells.Skip(1))
            {
                CollectionAssert.DoesNotContain(exceptCellsSet, nextCell, $"Should not have traversed blocked cell {nextCell}");
                
                int distance = Vector2IntUtils.ManhattanDistance(current, nextCell);
                Assert.AreEqual(1, distance, $"Found gap in path between current {current} and next {nextCell}");
                current = nextCell;
            }
            
            Assert.AreEqual(current, target);
        }
        
        [Test]
        public void SimplePath()
        {
            GameObject go = new GameObject();
            Grid grid = go.AddComponent<Grid>();

            Vector2Int startCell = Vector2Int.zero;
            Vector2Int targetCell = new Vector2Int(3, -3);
            Navigator2D navigator2D = new Navigator2D(grid, startCell);

            TestContext.WriteLine($"Find path from {startCell} to {targetCell}");
            
            var foundCells = navigator2D.CalculateNavigationCells(targetCell);
            
            TestContext.WriteLine($"Found path [ {string.Join(", ", foundCells)} ]");
            AssertPathfinding(Vector2Int.zero, targetCell, foundCells, Enumerable.Empty<Vector2Int>());
        }
        
        [Test]
        public void BlockedPath()
        {
            GameObject go = new GameObject();
            Grid grid = go.AddComponent<Grid>();

            var blockedCells = new Vector2Int[]
            {
                new Vector2Int(3, 1),
                new Vector2Int(4, 1),
                new Vector2Int(3, 2),
                new Vector2Int(4, 0),
                new Vector2Int(4, -1),
                new Vector2Int(3, -2),
                new Vector2Int(2, -3),
            }.AsEnumerable();

            Vector2Int startCell = Vector2Int.zero;
            Vector2Int targetCell = new Vector2Int(7, -2);
            Navigator2D navigator2D = new Navigator2D(grid, startCell);
            navigator2D.SetMovementValidation((currentCell, nextCell) => !blockedCells.Contains(nextCell));
            
            TestContext.WriteLine($"Find path from {startCell} to {targetCell}");
            TestContext.WriteLine($"Blocking cells {string.Join(", ", blockedCells)}");
            
            var foundCells = navigator2D.CalculateNavigationCells(targetCell);
            
            TestContext.WriteLine($"Found path [ {string.Join(", ", foundCells)} ]");
            AssertPathfinding(Vector2Int.zero, targetCell, foundCells, blockedCells);
        }
        
        [Test, MaxTime(50)]
        public void NoPath()
        {
            GameObject go = new GameObject();
            Grid grid = go.AddComponent<Grid>();

            // Make a box around the start cell
            var blockedCells = new List<Vector2Int>();
            const int boxSize = 12;
            const int startBox = -boxSize / 2;
            const int finalBox = boxSize / 2;

            for (int i = startBox; i <= finalBox; i++)
            {
                if (i == startBox || i == finalBox)
                {
                    for (int j = startBox + 1; j < finalBox; j++)
                    {
                        blockedCells.Add(new Vector2Int(i, j));
                    }
                }
                
                blockedCells.Add(new Vector2Int(i, startBox));
                blockedCells.Add(new Vector2Int(i, finalBox));
            }

            Vector2Int startCell = Vector2Int.zero;
            Vector2Int targetCell = new Vector2Int(-20, -20);
            Navigator2D navigator2D = new Navigator2D(grid, startCell);
            navigator2D.SetMovementValidation((currentCell, nextCell) => !blockedCells.Contains(nextCell));
            
            TestContext.WriteLine($"Find path from {startCell} to {targetCell}");
            TestContext.WriteLine($"Blocking cells {string.Join(", ", blockedCells)}");
            
            var foundCells = navigator2D.CalculateNavigationCells(targetCell);
            
            TestContext.WriteLine($"Found path [ {string.Join(", ", foundCells)} ]");
            CollectionAssert.IsEmpty(foundCells, "Expected no path, but somehow found one!");
        }
        
        [Test, MaxTime(500), Timeout(1000), Repeat(5)]
        public void PathPerformanceTest()
        {
            GameObject go = new GameObject();
            Grid grid = go.AddComponent<Grid>();

            var blockedCells = new Vector2Int[]
            {
                // Right wall
                new Vector2Int(4, 1),
                new Vector2Int(4, 2),
                new Vector2Int(4, 0),
                new Vector2Int(4, -1),
                new Vector2Int(4, -2),
                new Vector2Int(3, -3),
                
                // Bottom wall
                new Vector2Int(-4, -5),
                new Vector2Int(-3, -5),
                new Vector2Int(-2, -5),
                new Vector2Int(-1, -5),
                new Vector2Int(0, -5),
            }.AsEnumerable();

            // The test will run much faster with this, this is how you're expected to set this up.
            var blockedCellsSet = new HashSet<Vector2Int>(blockedCells);

            Vector2Int startCell = new Vector2Int(-3, 1);
            Vector2Int targetCell = new Vector2Int(Random.Range(-10, 150), Random.Range(-150, 7));
            Navigator2D navigator2D = new Navigator2D(grid, startCell, 300);
            navigator2D.SetMovementValidation((currentCell, nextCell) => !blockedCellsSet.Contains(nextCell));
            
            TestContext.WriteLine($"Find path from {startCell} to {targetCell}");
            TestContext.WriteLine($"Blocking cells {string.Join(", ", blockedCellsSet)}");
            
            var foundCells = navigator2D.CalculateNavigationCells(targetCell);
            
            TestContext.WriteLine($"Found path [ {string.Join(", ", foundCells)} ]");
            AssertPathfinding(startCell, targetCell, foundCells, blockedCellsSet);
        }
        
        [Test, MaxTime(1000)]
        public void NoPathPerformanceTest()
        {
            GameObject go = new GameObject();
            Grid grid = go.AddComponent<Grid>();

            // Make a box around the start cell
            var blockedCells = new List<Vector2Int>();
            const int boxSize = 150;
            const int startBox = -boxSize / 2;
            const int finalBox = boxSize / 2;

            for (int i = startBox; i <= finalBox; i++)
            {
                if (i == startBox || i == finalBox)
                {
                    for (int j = startBox + 1; j < finalBox; j++)
                    {
                        blockedCells.Add(new Vector2Int(i, j));
                    }
                }
                
                blockedCells.Add(new Vector2Int(i, startBox));
                blockedCells.Add(new Vector2Int(i, finalBox));
            }

            Vector2Int startCell = Vector2Int.zero;
            Vector2Int targetCell = new Vector2Int(150, 150);
            Navigator2D navigator2D = new Navigator2D(grid, startCell, 300);
            navigator2D.SetMovementValidation((currentCell, nextCell) => !blockedCells.Contains(nextCell));
            
            TestContext.WriteLine($"Find path from {startCell} to {targetCell}");
            TestContext.WriteLine($"Blocking cells {string.Join(", ", blockedCells)}");
            
            var foundCells = navigator2D.CalculateNavigationCells(targetCell);
            
            TestContext.WriteLine($"Found path [ {string.Join(", ", foundCells)} ]");
            CollectionAssert.IsEmpty(foundCells, "Expected no path, but somehow found one!");
        }
    }
}
