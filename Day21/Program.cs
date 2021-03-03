using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 21

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part2()
{
	void Swap(ref char a, ref char b)
	{
		char tmp = a;
		a = b;
		b = tmp;
	}

	void Permute(char[] elements, int recursionDepth, int maxDepth, List<string> permutations)
	{
		if (recursionDepth == maxDepth)
		{
			permutations.Add(new string(elements));
			return;
		}

		for (int i = recursionDepth; i <= maxDepth; i++)
		{
			Swap(ref elements[recursionDepth], ref elements[i]);
			Permute(elements, recursionDepth + 1, maxDepth, permutations);
			// backtrack
			Swap(ref elements[recursionDepth], ref elements[i]);
		}
	}

	var permutations = new List<string>();
	var array = "abcdefgh".ToCharArray();
	Permute(array, 0, array.Length - 1, permutations);
	var instructions = ReadInput().ToList();
	foreach (var s in permutations)
	{
		if (Scramble(s, instructions) == "fbgdceah")
		{
			System.Console.WriteLine($"{s} is the unscrambled version.");
			return;
		}
	}
	System.Console.WriteLine(permutations.Count);
}

void Part1()
{
	System.Console.WriteLine(Scramble("abcdefgh", ReadInput().ToList()));
}

string Scramble(string s, List<(Operation op, LetterOrNumber lon1, LetterOrNumber lon2)> instructions)
{
	var input = new List<char>(s.ToCharArray());
	foreach (var i in instructions)
	{
		switch (i.op)
		{
			case Operation.SwapPosition:
				{
					var tmp = input[i.lon1.Number];
					input[i.lon1.Number] = input[i.lon2.Number];
					input[i.lon2.Number] = tmp;
				}
				break;

			case Operation.SwapLetter:
				{
					var idx1 = input.IndexOf(i.lon1.Letter);
					var idx2 = input.IndexOf(i.lon2.Letter);
					input[idx1] = i.lon2.Letter;
					input[idx2] = i.lon1.Letter;
				}
				break;

			case Operation.ReversePositions:
				for (var j = 0; j <= (i.lon2.Number - i.lon1.Number) / 2; j++)
				{
					var tmp = input[i.lon1.Number + j];
					input[i.lon1.Number + j] = input[i.lon2.Number - j];
					input[i.lon2.Number - j] = tmp;
				}
				break;

			case Operation.RotateLeft:
				for (var j = 0; j < i.lon1.Number; j++)
				{
					var tmp = input[0];
					input.RemoveAt(0);
					input.Add(tmp);
				}
				break;

			case Operation.RotateRight:
				for (var j = 0; j < i.lon1.Number; j++)
				{
						var tmp = input.Last();
						input.RemoveAt(input.Count - 1);
						input.Insert(0, tmp);
				}
				break;

			case Operation.MovePosition:
				{
					var tmp = input[i.lon1.Number];
					input.RemoveAt(i.lon1.Number);
					input.Insert(i.lon2.Number, tmp);
				}
				break;

			case Operation.RotateBased:
				{
					var index = input.IndexOf(i.lon1.Letter);
					var rotations = 1 + index + (index >= 4 ? 1 : 0);
					for (var j = 0; j < rotations; j++)
					{
						var tmp = input.Last();
						input.RemoveAt(input.Count - 1);
						input.Insert(0, tmp);
					}
				}
				break;

			default:
				throw new InvalidOperationException();
		}
	}
	return new string(input.ToArray());
}

IEnumerable<(Operation op, LetterOrNumber lon1, LetterOrNumber lon2)> ReadInput()
{
	foreach (var line in File.ReadAllLines("input.txt"))
	{
		var tokens = line.Split(' ');
		if (tokens[0] == "swap" && tokens[1] == "position")
		{
			yield return (Operation.SwapPosition, new LetterOrNumber(tokens[2]), new LetterOrNumber(tokens[5]));
		}
		else if (tokens[0] == "swap" && tokens[1] == "letter")
		{
			yield return (Operation.SwapLetter, new LetterOrNumber(tokens[2]), new LetterOrNumber(tokens[5]));
		}
		else if (tokens[0] == "reverse" && tokens[1] == "positions")
		{
			yield return (Operation.ReversePositions, new LetterOrNumber(tokens[2]), new LetterOrNumber(tokens[4]));
		}
		else if (tokens[0] == "rotate" && tokens[1] == "left")
		{
			yield return (Operation.RotateLeft, new LetterOrNumber(tokens[2]), null);
		}
		else if (tokens[0] == "rotate" && tokens[1] == "right")
		{
			yield return (Operation.RotateRight, new LetterOrNumber(tokens[2]), null);
		}
		else if (tokens[0] == "move" && tokens[1] == "position")
		{
			yield return (Operation.MovePosition, new LetterOrNumber(tokens[2]), new LetterOrNumber(tokens[5]));
		}
		else if (tokens[0] == "rotate" && tokens[1] == "based")
		{
			yield return (Operation.RotateBased, new LetterOrNumber(tokens[6]), null);
		}
		else
		{
			throw new InvalidProgramException();
		}
	}
}

internal class LetterOrNumber
{
	public LetterOrNumber(string s)
	{
		if (int.TryParse(s, out var n))
		{
			Number = n;
		}
		else
		{
			IsLetter = true;
			Letter = s[0];
		}
	}

	public override string ToString()
	{
		return IsLetter ? Letter.ToString() : Number.ToString();
	}

	public bool IsLetter { get; }
	public char Letter { get; }
	public int Number { get; }
}

enum Operation
{
	SwapPosition,
	SwapLetter,
	ReversePositions,
	RotateLeft,
	RotateRight,
	MovePosition,
	RotateBased
}
