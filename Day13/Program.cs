using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 13

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var favoriteNumber = ReadInput();
	var w = 55;
	var h = 55;
	var maze = new bool[h, w];

	for (var y = 0; y < h; y++)
	{
		for (var x = 0; x < w; x++)
		{
		  var n = x * x + 3 * x + 2 * x * y + y + y * y;	
			n += favoriteNumber;
			maze[y, x] = CountBinaryOnes(n) % 2 == 0;
		}
		System.Console.WriteLine();
	}

	Print(maze);

	(int x, int y) start = (1, 1);
	(int x, int y) dest = (31, 39);

	var minSteps = int.MaxValue;
	//Wander(start, dest, maze, new List<(int x, int y)>(), 0, ref minSteps);

	// Part 2
	var locations = new SortedSet<(int x, int y)>();
	Wander2(start, maze, new List<(int x, int y)>(), 0, locations);
	System.Console.WriteLine($"After 50 steps {locations.Count} different locations could be seen.");
}

void Wander((int x, int y) pos, (int x, int y) dest, bool[,] maze, List<(int x, int y)> history, int steps, ref int minSteps)
{
	if (history.Contains(pos))
	{
		return;
	}
	history.Add(pos);
	if (pos == dest)
	{
		if (steps < minSteps)
		{
			minSteps = steps;
			System.Console.WriteLine($"Destination {dest} reached after {steps} steps.");
		}
		return;
	}
	if (pos.x >= 0 && pos.x < maze.GetLength(1) && pos.y >= 0 && pos.y < maze.GetLength(0) && maze[pos.y, pos.x])
	{
		Wander((pos.x - 1, pos.y + 0), dest, maze, history.ToList(), steps + 1, ref minSteps);
		Wander((pos.x + 1, pos.y + 0), dest, maze, history.ToList(), steps + 1, ref minSteps);
		Wander((pos.x + 0, pos.y - 1), dest, maze, history.ToList(), steps + 1, ref minSteps);
		Wander((pos.x + 0, pos.y + 1), dest, maze, history.ToList(), steps + 1, ref minSteps);
	}
}

void Wander2((int x, int y) pos, bool[,] maze, List<(int x, int y)> history, int steps, SortedSet<(int x, int y)> locations)
{
	if (pos.x < 0 || pos.x >= maze.GetLength(1) || pos.y < 0 || pos.y >= maze.GetLength(0) || !maze[pos.y, pos.x])
	{
		return;
	}
	
	if (history.Contains(pos))
	{
		return;
	}
	history.Add(pos);

	locations.Add(pos);

	if (steps == 50)
	{
		return;
	}

	Wander2((pos.x - 1, pos.y + 0), maze, history.ToList(), steps + 1, locations);
	Wander2((pos.x + 1, pos.y + 0), maze, history.ToList(), steps + 1, locations);
	Wander2((pos.x + 0, pos.y - 1), maze, history.ToList(), steps + 1, locations);
	Wander2((pos.x + 0, pos.y + 1), maze, history.ToList(), steps + 1, locations);
}

int CountBinaryOnes(int n)
{
	var ones = 0;
	while (n > 0)
	{
		if ((n & 1) == 1)
		{
			ones++;
		}
		n >>= 1;
	}
	return ones;
}

static void Print(bool[,] maze)
{
	var h = Math.Min(7, maze.GetLength(0));
	var w = Math.Min(10, maze.GetLength(1));
	for (var y = 0; y < h; y++)
	{
		for (var x = 0; x < w; x++)
		{
			System.Console.Write(maze[y, x] ? '.' : '#');
		}
		System.Console.WriteLine();
	}
}

static int ReadInput()
{
	return int.Parse(File.ReadAllText("input.txt"));
}
