using System;
using System.Collections.Generic;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraPathFinder
{
	public static IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
	{
		// Dictionary to store the minimum cost to reach each cell
		var cellCosts = new Dictionary<Point, int>();
		// Dictionary to store the previous cell in the optimal path
		var previous = new Dictionary<Point, Point>();
		// Priority queue to process cells in order of increasing cost
		var queue = new PriorityQueue<(Point point, int cost)>((a, b) => a.cost.CompareTo(b.cost));
		// Set to track targets that have been found
		var foundTargets = new HashSet<Point>();
		// Convert targets to a hashset for efficient lookup
		var targetSet = new HashSet<Point>(targets);
		
		// Initialize the starting point
		cellCosts[start] = 0;
		queue.Enqueue((start, 0));
		
		while (queue.Count > 0 && foundTargets.Count < targetSet.Count)
		{
			var (current, currentCost) = queue.Dequeue();
			
			// Skip if we've already found a better path to this cell
			if (currentCost > cellCosts[current])
				continue;
			
			// Check if this is a target cell
			if (targetSet.Contains(current) && !foundTargets.Contains(current))
			{
				foundTargets.Add(current);
				
				// Reconstruct the path
				var path = ReconstructPath(previous, start, current);
				yield return new PathWithCost(path, currentCost);
				
				// Continue processing to find paths to other targets
			}
			
			// Explore neighbors
			foreach (var direction in new[] { 
				new Point(0, 1), new Point(1, 0), 
				new Point(0, -1), new Point(-1, 0) 
			})
			{
				var neighbor = new Point(current.X + direction.X, current.Y + direction.Y);
				
				// Skip if the neighbor is outside the maze or is a wall
				if (!state.InBounds(neighbor) || state.IsWall(neighbor))
					continue;
				
				// Calculate the cost to reach the neighbor
				var neighborCost = currentCost + state.CellCost[neighbor.X, neighbor.Y];
				
				// If we found a better path to the neighbor, update it
				if (!cellCosts.ContainsKey(neighbor) || neighborCost < cellCosts[neighbor])
				{
					cellCosts[neighbor] = neighborCost;
					previous[neighbor] = current;
					queue.Enqueue((neighbor, neighborCost));
				}
			}
		}
	}
	
	// Helper method to reconstruct the path from start to end
	private static List<Point> ReconstructPath(Dictionary<Point, Point> previous, Point start, Point end)
	{
		var path = new List<Point>();
		var current = end;
		
		while (!current.Equals(start))
		{
			path.Add(current);
			current = previous[current];
		}
		
		path.Add(start);
		path.Reverse();
		
		return path;
	}
}