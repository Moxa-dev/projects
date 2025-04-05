using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            // If no chests, find direct path from start to exit
            if (map.Chests.Length == 0)
            {
                var directPath = BfsTask.FindPaths(map, map.InitialPosition, new[] { map.Exit })
                    .FirstOrDefault();

                return directPath?.Value?.ToArray() ?? Array.Empty<MoveDirection>();
            }

            // Find paths from start to chests
            var pathsToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);

            // Find paths from each chest to the exit
            var pathsFromChestsToExit = new Dictionary<Chest, IEnumerable<MoveDirection>>();
            foreach (var chest in map.Chests)
            {
                var path = BfsTask.FindPaths(map, chest.Location, new[] { map.Exit }) // Assuming Chest has a Location property
                    .FirstOrDefault();
                if (path != null)
                {
                    pathsFromChestsToExit[chest] = path.Value;
                }
            }

            // Find all valid paths through chests
            var pathsThroughChests = map.Chests
                .Select(chest => new
                {
                    Chest = chest,
                    PathToChest = pathsToChests.TryGetValue(chest, out var pathToChest) ? pathToChest : null,
                    PathFromChestToExit = pathsFromChestsToExit.TryGetValue(chest, out var pathFromChestToExit) ? pathFromChestToExit : null
                })
                .Where(p => p.PathToChest != null && p.PathFromChestToExit != null)
                .Select(p => new
                {
                    p.Chest,
                    TotalLength = p.PathToChest.Count() + p.PathFromChestToExit.Count(),
                    Path = p.PathToChest.Concat(p.PathFromChestToExit)
                })
                .ToList();

            if (!pathsThroughChests.Any())
                return Array.Empty<MoveDirection>();

            // Find the shortest path through a chest
            var minLength = pathsThroughChests.Min(p => p.TotalLength);

            // If multiple paths with the same length, choose the one with the "heaviest" chest
            var bestPath = pathsThroughChests
                .Where(p => p.TotalLength == minLength)
                .OrderByDescending(p => p.Chest.Weight)
                .First();

            return bestPath.Path.ToArray();
        }

        private static MoveDirection GetReverseDirection(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up: return MoveDirection.Down;
                case MoveDirection.Down: return MoveDirection.Up;
                case MoveDirection.Left: return MoveDirection.Right;
                case MoveDirection.Right: return MoveDirection.Left;
                default: throw new ArgumentException("Unknown direction");
            }
        }
    }
}