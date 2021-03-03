using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 19

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var numberOfElves = ReadInput();
	var elves = new LinkedList<Elf>();
	for (var i = 1; i <= numberOfElves; i++)
	{
		elves.AddLast(new Elf(i));
	}

	var current = elves.First;
	do
	{
		var next = current.Next != null ? current.Next : elves.First;
		current.Value.Presents += next.Value.Presents;
		elves.Remove(next);
		current = current.Next != null ? current.Next : elves.First;
	}
	while (elves.Count > 1);

	System.Console.WriteLine($"Elf {elves.First.Value.Id} gets all ({elves.First.Value.Presents}) the presents.");
}

void Part2()
{
	var numberOfElves = ReadInput();
	var elves = new BucketList<Elf>(20_000);
	for (var i = 1; i <= numberOfElves; i++)
	{
		elves.Add(new Elf(i));
	}

	var current = 0;
	do
	{
		var next = (current + elves.Count / 2) % elves.Count;
		var elf = elves[current];
		var victim = elves[next];
		elf.Presents += victim.Presents;
		elves.RemoveAt(next);
		if (next < current)
		{
			current--;
		}
		current = (current + 1) % elves.Count;
	}
	while (elves.Count > 1);

	System.Console.WriteLine($"Elf {elves[0].Id} gets all ({elves[0].Presents}) the presents.");
}

int ReadInput()
{
	return int.Parse(File.ReadAllText("input.txt"));
}

class Elf
{
	public Elf(int id)
	{
		Id = id;
		Presents = 1;
	}
	public int Id { get; }

	public override string ToString()
	{
		return $"#{Id}";
	}

	public int Presents { get; set;  }
}