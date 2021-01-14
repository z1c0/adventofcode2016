using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day1
{
	enum Direction
	{
		N,
		E,
		S,
		W
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("START");
			var sw = Stopwatch.StartNew();
			Part1();
			Part2();
			Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");
		}
		private static void Part1()
		{
			var direction = Direction.N;
			var x = 0;
			var y = 0;
			var instructions = ReadInput();
			foreach (var i in instructions)
			{
				Turn(i.Item1, ref direction);
				Move(direction, i.Item2, ref x, ref y);
				var p = new ValueTuple<int, int>(x, y);
			}
			System.Console.WriteLine($"Position: {x}/{y} -> {Math.Abs(x) + Math.Abs(y)} blocks away.");
		}

		private static void Part2()
		{
			var direction = Direction.N;
			var x = 0;
			var y = 0;
			var instructions = ReadInput();
			var history = new List<ValueTuple<int, int>>();
			foreach (var i in instructions)
			{
				Turn(i.Item1, ref direction);
				for (var by = 0; by < i.Item2; by++)
				{
					Move(direction, 1, ref x, ref y);
					var p = new ValueTuple<int, int>(x, y);
					if (history.Contains(p))
					{
						System.Console.WriteLine($"Position: {x}/{y} -> {Math.Abs(x) + Math.Abs(y)} blocks away.");
						return;
					}
					history.Add(p);
				}
			}
		}

		private static void Move(Direction direction, int by, ref int x, ref int y)
		{
			switch (direction)
			{
				case Direction.N:
					y-= by;
					break;
				case Direction.E:
					x += by;
					break;
				case Direction.S:
					y+= by;
					break;
				case Direction.W:
					x -= by;
					break;
			}
		}

		private static void Turn(char c, ref Direction direction)
		{
			if (c == 'L')
			{
				var d = ((int)direction + 4 - 1) % 4;
				direction = (Direction)d;
			}
			else if (c == 'R')
			{
				var d = ((int)direction + 1) % 4;
				direction = (Direction)d;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		private static List<Tuple<char, int>> ReadInput()
		{
			var text = File.ReadAllText("input.txt");
			var tokens = text.Split(", ");
			var instructions = new List<Tuple<char, int>>();
			foreach (var t in tokens)
			{
				instructions.Add(new Tuple<char, int>(t[0], Int32.Parse(t.Substring(1))));
			}
			return instructions;
		}
	}
}


