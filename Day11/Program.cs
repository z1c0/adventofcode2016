using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 11

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
  var state = ReadInput();
	Step(state, 0, new Dictionary<string, int>());
}

void Step(State state, int steps, Dictionary<string, int> cache)
{
	if (steps > 80) return;

	var encoded = state.Encode();

	if (cache.ContainsKey(encoded))
	{
		if (cache[encoded] == -1)
		{
			return;
		}
		if (steps < cache[encoded])
		{
			cache[encoded] = steps;
		}
		else
		{
			return;
		}
	}
	else
	{
		cache.Add(encoded, steps);
	}

	// Win?
	if (state.Elevator == 3 && !state.Floors[0].Any() && !state.Floors[1].Any() && !state.Floors[2].Any())
	{
		System.Console.WriteLine($"Finished after {steps} steps!");
		return;
	}
	// Lose?
	for (var i = 0; i < state.Floors.Length; i++)
	{
		var chips = state.Floors[i].Where(s => s[1] == 'M');
		foreach (var chip in chips)
		{
			var goodGen = state.Floors[i].SingleOrDefault(s => s[1] == 'G' && s[0] == chip[0]);
			var badGens = state.Floors[i].Where(s => s[1] == 'G' && s[0] != chip[0]);
			if (goodGen == null && badGens.Any())
			{
				cache[encoded] = -1;
				return;
			}
		}
	}

  // get items at current floor
	var items = state.Floors[state.Elevator].ToList();
	if (!items.Any())
	{
		throw new InvalidOperationException();
	}
	foreach (var item in items)
	{
		// Second item?
		var items2 = state.Floors[state.Elevator].ToList();
		items2.Remove(item);
		foreach (var item2 in items2)
		{
			// go up?
			if (state.Elevator < 3)
			{
				var stateCopy = state.Clone();
				stateCopy.Floors[stateCopy.Elevator].Remove(item);
				stateCopy.Floors[stateCopy.Elevator].Remove(item2);
				stateCopy.Elevator++;
				stateCopy.Floors[stateCopy.Elevator].Add(item);
				stateCopy.Floors[stateCopy.Elevator].Add(item2);
				Step(stateCopy, steps + 1, cache);
			}
			// go down?
			if (state.Elevator > 0)
			{
				var stateCopy = state.Clone();
				stateCopy.Floors[stateCopy.Elevator].Remove(item);
				stateCopy.Floors[stateCopy.Elevator].Remove(item2);
				stateCopy.Elevator--;
				stateCopy.Floors[stateCopy.Elevator].Add(item);
				stateCopy.Floors[stateCopy.Elevator].Add(item2);
				Step(stateCopy, steps + 1, cache);
			}
		}

		// go up?
		if (state.Elevator < 3)
		{
			var stateCopy = state.Clone();
			stateCopy.Floors[stateCopy.Elevator].Remove(item);
			stateCopy.Elevator++;
			stateCopy.Floors[stateCopy.Elevator].Add(item);
			Step(stateCopy, steps + 1, cache);
		}
		// go down?
		if (state.Elevator > 0)
		{
			var stateCopy = state.Clone();
			stateCopy.Floors[stateCopy.Elevator].Remove(item);
			stateCopy.Elevator--;
			stateCopy.Floors[stateCopy.Elevator].Add(item);
			Step(stateCopy, steps + 1, cache);
		}
	}
}

static State ReadInput()
{
	var state = new State();
	foreach (var line in File.ReadLines("input.txt"))
	{
		var tokens = line.Split(':');
		state.Floors[int.Parse(tokens[0])].Add(tokens[1]);
	}
	return state;
}

class State
{
	internal int Elevator { get; set; }

	internal List<string>[] Floors { get;} = new List<string>[4]
	{
		new List<string>(),
		new List<string>(),
		new List<string>(),
		new List<string>(),
	};
	
	internal State Clone()
	{
		var clone = new State
		{
			Elevator = Elevator,
		};
		clone.Floors[0] = Floors[0].ToList();
		clone.Floors[1] = Floors[1].ToList();
		clone.Floors[2] = Floors[2].ToList();
		clone.Floors[3] = Floors[3].ToList();
		return clone;
	}

	internal string Encode()
	{
		var sb = new StringBuilder();
		sb.Append(Elevator);
		sb.Append("/0:");
		sb.Append(string.Join(";", Floors[0].OrderBy(x => x).Select(x => x.ToString()).ToArray()));
		sb.Append("/1:");
		sb.Append(string.Join(";", Floors[1].OrderBy(x => x).Select(x => x.ToString()).ToArray()));
		sb.Append("/2:");
		sb.Append(string.Join(";", Floors[2].OrderBy(x => x).Select(x => x.ToString()).ToArray()));
		sb.Append("/3:");
		sb.Append(string.Join(";", Floors[3].OrderBy(x => x).Select(x => x.ToString()).ToArray()));
		return sb.ToString();
	}
}