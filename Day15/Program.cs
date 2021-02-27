using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 15

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(false);
Part1(true);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(bool addDisk)
{
	var discs = ReadInput().ToList();
	if (addDisk)
	{
		discs.Add(new Disc(7, 11, 0));
	}
	var time = 0;
	while (true)
	{
		var clone = discs.Select(d => d.Clone()).ToList();
		if (Simulate(time, clone))
		{
			System.Console.WriteLine($"At time {time} the capsule went through");
			break;
		}
		MoveDiscs(discs);
		time++;
	}
}

void MoveDiscs(List<Disc> discs)
{
	foreach (var d in discs)
	{
		d.Move();
	}
}

bool Simulate(int time, List<Disc> discs)
{
	for (var i = 0; i < discs.Count; i++)
	{
		MoveDiscs(discs);
		if (discs[i].Current != 0)
		{
			return false;
		}
	}
	return true;
}

static IEnumerable<Disc> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		var id = int.Parse(tokens[1][1..]);
		var positions = int.Parse(tokens[3]);
		var start = int.Parse(tokens.Last()[0..^1]);
		yield return new Disc(id, positions, start);
	}
}

internal class Disc
{
	private int _id;
	private int _positionCount;

	public int Current { get; set; }

	public Disc(int id, int positions, int start)
	{
		_id = id;
		_positionCount = positions;
		Current = start;
	}

	public override string ToString()
	{
		return $"{_id} - {Current}/{_positionCount}";
	}

	internal void Move()
	{
		Current = (Current + 1) % _positionCount;
	}

	internal Disc Clone()
	{
		return new Disc(_id, _positionCount, Current);
	}
}