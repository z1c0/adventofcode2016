using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 24

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(false);
Part1(true);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(bool returnToZero)
{
	// TSP
	// measure all connections
	// find shortest route
	const int max = 8;
	var maze = ReadInput();
	var connections = new int[max, max];
	for (var i = 0; i < max; i++)
	{
		for (var j = i + 1; j < max; j++)
		{
			connections[i, j] = ShortestPath(maze, i, j);
			connections[j, i] = ShortestPath(maze, i, j);
		}
	}

	var visited = new List<int>();
	var unvisited = Enumerable.Range(0, max).ToList();
	var shortestDistance = int.MaxValue;
	FindShortestRoute(connections, 0, visited, unvisited, 0, ref shortestDistance, returnToZero);
	System.Console.WriteLine($"Fewest number of steps: {shortestDistance}");
}

void FindShortestRoute(int[,] connections, int from, List<int> visited, List<int> unvisited, int distance, ref int shortestDistance, bool returnToZero)
{
	visited.Add(from);
	unvisited.Remove(from);
	foreach (var n in unvisited)
	{
		var copyVisited = visited.ToList();
		var copyUnvisited = unvisited.ToList();
		FindShortestRoute(connections, n, copyVisited, copyUnvisited, distance + connections[from, n], ref shortestDistance, returnToZero);
	}
	if (!unvisited.Any())
	{
		if (returnToZero)
		{
			distance += connections[visited.Last(), 0];
		}
		//System.Console.WriteLine($"{string.Join(" - ", visited)}: {distance}");
		shortestDistance = Math.Min(distance, shortestDistance);
	}
}

(int x, int y) GetPos(char[,] maze, int n)
{
	var h = maze.GetLength(0);
	var w = maze.GetLength(1);
	for (var y = 0; y < h; y++)
	{
		for (var x = 0; x < w; x++)
		{
			if (maze[y, x] == '0' + n)
			{
				return (x, y);
			}
		}
	}
	throw new InvalidOperationException();
}

Nullable<bool> Check(char[,] maze, (int x, int y) current, (int x, int y) to)
{
	var h = maze.GetLength(0);
	var w = maze.GetLength(1);
	if (current.x < 0 || current.y < 0 || current.x >= w || current.y >= h || maze[current.y, current.x] == '#')
	{
		return false;
	}
	if (current == to)
	{
		return true;
	}
	return null;
}

int ShortestPath(char[,] maze, int from, int to)
{
	var fromPos = GetPos(maze, from);
	var toPos = GetPos(maze, to);
	var history = new HashSet<(int x, int y)>();
  var queue = new Queue<(int x, int y, int length)>();
	queue.Enqueue((fromPos.x, fromPos.y, 0));
	while (queue.Count > 0)
	{
		var p = queue.Dequeue();
		if (!history.Contains((p.x, p.y)))
		{
			history.Add((p.x, p.y));
			var check = Check(maze, (p.x, p.y), toPos);
			if (check.HasValue)
			{
				if (check.Value)
				{
					//System.Console.WriteLine($"{from} -> {to} -> {p.length} steps.");
					return p.length;
				}
			}
			else
			{
				queue.Enqueue((p.x - 1, p.y    , p.length + 1));
				queue.Enqueue((p.x + 1, p.y    , p.length + 1));
				queue.Enqueue((p.x    , p.y - 1, p.length + 1));
				queue.Enqueue((p.x    , p.y + 1, p.length + 1));
			}
		}
	}
	throw new InvalidOperationException();
}

static char[,] ReadInput()
{
	var lines = File.ReadLines("input.txt").ToList();
	var maze = new char[lines.Count, lines[0].Length];
	for (var y = 0; y < lines.Count; y++)
	{
		for (var x = 0; x < lines[y].Length; x++)
		{
			maze[y, x] = lines[y][x];
		}
	}
	return maze;
}
