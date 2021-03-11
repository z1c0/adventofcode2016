using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 22

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var count = 0;
	var nodes = ReadInput().ToList();
	for (var i = 0; i < nodes.Count; i++)
	{
		var node1 = nodes[i];
		for (var j = 0; j < nodes.Count; j++)
		{
			var node2 = nodes[j];
			if (i != j && node1.Used > 0 && node1.Used <= node2.Avail)
			{
				count++;
			}
		}
	}
	System.Console.WriteLine($"There are {count} viable pairs.");
}

void Part2()
{
	var nodes = ReadInput().ToList();
	var w = nodes.Max(n => n.X) + 1;
	var h = nodes.Max(n => n.Y) + 1;
	var grid = new char[h, w];

	foreach (var n in nodes)
	{
		grid[n.Y, n.X] = '.';
	}
	var goal = nodes.Single(n => n.X == w - 1 && n.Y == 0);
	grid[goal.Y, goal.X] = 'G';
	var empty = nodes.Single(n => n.Used == 0);
	grid[empty.Y, empty.X] = '_';
	var large = nodes.Where(n => n.Used > empty.Size);
	foreach (var l in large)
	{
		grid[l.Y, l.X] = '#';
	}

	var minSteps = int.MaxValue;
	Step(grid, (empty.X, empty.Y), (goal.X, goal.Y), new Dictionary<string, int>(), 0, ref minSteps);
}

void Step(char[,] grid, (int X, int Y) empty, (int X, int Y) goal, Dictionary<string, int> cache, int steps, ref int minSteps)
{
	var state = $"{empty}{goal}";
	if (cache.ContainsKey(state))
	{
		if (steps < cache[state])
		{
			cache[state] = steps;
		}
		else
		{
			return;
		}
	}
	else
	{
		cache.Add(state, steps);
	}

	if (goal.X == 0 && goal.Y == 0)
	{
		if (steps < minSteps)
		{
			minSteps = steps;
			System.Console.WriteLine($"Goal reached after {steps} steps");
		}
		return;
	}
	if (goal == empty)
	{
		return;
	}

	var h = grid.GetLength(0);
	var w = grid.GetLength(1);
	if (steps > minSteps)
	{
		return;
	}
	if (empty.X < 0 || empty.Y < 0 || empty.X >= w || empty.Y >= h)
	{
		return;
	}
	if (grid[empty.Y, empty.X] == '#')
	{
		return;
	}
	// can we move the goal?
	var dist = Math.Abs(empty.X - goal.X) + Math.Abs(empty.Y - goal.Y);
	if (dist == 1)
	{
		Step(grid, goal, empty, cache, steps + 1, ref minSteps);
	}
	// move the empty
	Step(grid, (empty.X - 1, empty.Y   ),  goal, cache, steps + 1, ref minSteps);
	Step(grid, (empty.X + 1, empty.Y   ),  goal, cache, steps + 1, ref minSteps);
	Step(grid, (empty.X    , empty.Y - 1), goal, cache, steps + 1, ref minSteps);
	Step(grid, (empty.X    , empty.Y + 1), goal, cache, steps + 1, ref minSteps);
}

IEnumerable<Node> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt").Skip(2))
	{
		var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var tokens2  = tokens[0].Split('-');
		var x = int.Parse(tokens2[1][1..]);
		var y = int.Parse(tokens2[2][1..]);
		var size = int.Parse(tokens[1][0..^1]);
		var used = int.Parse(tokens[2][0..^1]);
		var avail = int.Parse(tokens[3][0..^1]);
		yield return new Node(x, y, size, used, avail);
	}
}

record Node(int X, int Y, int Size, int Used, int Avail);
