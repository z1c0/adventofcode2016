using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 18

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(40);
Part1(400000);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(int rows)
{
	var tiles = ReadInput(rows);
	for (var y = 1; y < tiles.GetLength(0); y++)
	{
		for (var x = 1; x < tiles.GetLength(1) - 1; x++)
		{
			var l = tiles[y - 1, x - 1];
			var c = tiles[y - 1, x];
			var r = tiles[y - 1, x + 1];
			if ((l && c && !r) || (!l && c && r) || (l && !c && !r) || (!l && !c && r))
			{
				tiles[y, x] = true;
			}
		}
	}

	var count = 0;
	for (var y = 0; y < tiles.GetLength(0); y++)
	{
		for (var x = 1; x < tiles.GetLength(1) - 1; x++)
		{
			if (!tiles[y, x])
			{
				count++;
			}
		}
	}
	System.Console.WriteLine($"{count} tiles are safe.");
}

bool[,] ReadInput(int rows)
{
	var line = File.ReadAllText("input.txt");
	var tiles = new bool[rows, line.Length + 2]; // add 2 "safe tiles" left and right
	for (var i = 0; i < line.Length; i++)
	{
		tiles[0, i + 1] = line[i] == '^';
	}
	return tiles;
}