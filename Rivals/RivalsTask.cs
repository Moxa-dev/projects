using System.Collections.Generic;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var rows = map.Maze.GetLength(0);
            var cols = map.Maze.GetLength(1);
            var result = new OwnedLocation[rows, cols];
            
            // Initialize all cells as unowned
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = new OwnedLocation(-1, int.MaxValue, new Point(i, j));
            
            // Queue for BFS
            var queue = new Queue<(Point point, int owner, int distance)>();
            
            // Add all player starting positions to the queue
            for (int player = 0; player < map.Players.Length; player++)
            {
                var position = map.Players[player];
                queue.Enqueue((position, player, 0));
                result[position.X, position.Y] = new OwnedLocation(player, 0, position);
            }
            
            // Directions: right, left, down, up
            var directions = new[] { new Point(0, 1), new Point(0, -1), new Point(1, 0), new Point(-1, 0) };
            
            // BFS
            while (queue.Count > 0)
            {
                var (point, owner, distance) = queue.Dequeue();
                
                // If this cell has a chest, don't continue from here
                if (map.Maze[point.X, point.Y] == MapCellExit.Chest)
                    continue;
                
                // Try all four directions
                foreach (var dir in directions)
                {
                    var newPoint = new Point(point.X + dir.X, point.Y + dir.Y);
                    
                    // Check if the new position is valid
                    if (newPoint.X < 0 || newPoint.X >= rows || newPoint.Y < 0 || newPoint.Y >= cols)
                        continue;
                    
                    // Skip walls
                    if (map.Maze[newPoint.X, newPoint.Y] == MapCellExit.Wall)
                        continue;
                    
                    // Calculate new distance
                    var newDistance = distance + 1;
                    
                    // Check if we can claim this cell
                    var currentOwner = result[newPoint.X, newPoint.Y];
                    if (currentOwner.Owner == -1 || 
                        newDistance < currentOwner.Distance || 
                        (newDistance == currentOwner.Distance && owner < currentOwner.Owner))
                    {
                        // Claim the cell
                        result[newPoint.X, newPoint.Y] = new OwnedLocation(owner, newDistance, newPoint);
                        queue.Enqueue((newPoint, owner, newDistance));
                    }
                }
            }
            
            // Convert 2D array to IEnumerable
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    yield return result[i, j];
        }
    }
}