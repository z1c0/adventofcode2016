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
	BFS(state);
}

Nullable<bool> Check(State state)
{
	// Win?
	if (state.Elevator == 3 && !state.Floors[0].Any() && !state.Floors[1].Any() && !state.Floors[2].Any())
	{
		return true;
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
				return false;
			}
		}
	}
	return null;
}

void BFS(State startState)
{
	var cache = new HashSet<string>();
  var queue = new Queue<State>();
	queue.Enqueue(startState);
	while (queue.Count > 0)
	{
		var state = queue.Dequeue();

		var encoded = state.Encode();
		if (!cache.Contains(encoded))
		{
			cache.Add(encoded);

			var check = Check(state);
			if (check.HasValue)
			{
				if (check.Value)
				{
					System.Console.WriteLine($"Finished after {state.Steps} steps!");
				}
			}
			else
			{
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
							queue.Enqueue(stateCopy);
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
							queue.Enqueue(stateCopy);
						}
					}

					// go up?
					if (state.Elevator < 3)
					{
						var stateCopy = state.Clone();
						stateCopy.Floors[stateCopy.Elevator].Remove(item);
						stateCopy.Elevator++;
						stateCopy.Floors[stateCopy.Elevator].Add(item);
						queue.Enqueue(stateCopy);
					}
					// go down?
					if (state.Elevator > 0)
					{
						var stateCopy = state.Clone();
						stateCopy.Floors[stateCopy.Elevator].Remove(item);
						stateCopy.Elevator--;
						stateCopy.Floors[stateCopy.Elevator].Add(item);
						queue.Enqueue(stateCopy);
					}
				}
			}
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
	
	internal int Steps { get; private set; }

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
			Steps = Steps + 1,
		};
		clone.Floors[0] = Floors[0].ToList();
		clone.Floors[1] = Floors[1].ToList();
		clone.Floors[2] = Floors[2].ToList();
		clone.Floors[3] = Floors[3].ToList();
		return clone;
	}

	internal string Encode()
	{
		void EncodeFloor(int level, StringBuilder sb)
		{
			sb.Append($"/{level}:");
			foreach (var s in Floors[level])
			{
				if (s[1] == 'M')
				{
					for (var i = 0; i < Floors.Length; i++)
					{
						var floor = Floors[i];
						if (floor.Contains($"{s[0]}G"))
						{
							sb.Append(level - i);
						}
					}
				}
			}
		}
		var sb = new StringBuilder();
		sb.Append(Elevator);
		EncodeFloor(0, sb);
		EncodeFloor(1, sb);
		EncodeFloor(2, sb);
		EncodeFloor(3, sb);
		return sb.ToString();
	}
}