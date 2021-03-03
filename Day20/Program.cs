using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 20

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var dict = new SortedDictionary<long, long>();
	foreach (var t in ReadInput())
	{
		dict[t.from] = t.to;
	}

	long lowest = long.MaxValue;
	var count = 0;
	long ip = 0;
	while (ip <= 4294967295)
	{
		var range = dict.Where(t => t.Key <= ip && ip <= t.Value).ToList();
		if (!range.Any())
		{
			lowest = Math.Min(lowest, ip);
			count++;
			ip++;
		}
		else
		{
			ip = range.First().Value + 1;
		}
	}
	
	System.Console.WriteLine($"Lowest IP found: {lowest}.");
	System.Console.WriteLine($"Total IPs found: {count}.");
}

IEnumerable<(long from, long to)> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split('-');
		yield return (long.Parse(tokens[0]), long.Parse(tokens[1]));
	}
}
