using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Day8
{
	internal enum CommandType
	{
		Rect,
		RotateColumn,
		RotateRow
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("START");
			var sw = Stopwatch.StartNew();
			Part1();
			Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");
		}

		private static void Part1()
		{
			var screen = new bool[50, 6];
			foreach (var cmd in ReadInput())
			{
				if (cmd.command == CommandType.Rect)
				{
					for (var y = 0; y < cmd.value2; y++)
					{
						for (var x = 0; x < cmd.value1; x++)
						{
							screen[x, y] = true;
						}
					}
				}
				else if (cmd.command == CommandType.RotateRow)
				{
					var w = screen.GetLength(0);
					var line = new bool[w];
					for (var x = 0; x < w; x++)
					{
						line[(x + cmd.value2) % w] = screen[x, cmd.value1];
					}
					for (var x = 0; x < w; x++)
					{
						screen[x, cmd.value1] = line[x];
					}
				}
				else if (cmd.command == CommandType.RotateColumn)
				{
					var h = screen.GetLength(1);
					var line = new bool[h];
					for (var y = 0; y < h; y++)
					{
						line[(y + cmd.value2) % h] = screen[cmd.value1, y];
					}
					for (var y = 0; y < h; y++)
					{
						screen[cmd.value1, y] = line[y];
					}
				}	
			}
			Print(screen);
		}

		private static void Print(bool[,] screen)
		{
			var count = 0;
			var w = screen.GetLength(0);
			var h = screen.GetLength(1);
			for (var y = 0; y < h; y++)
			{
				for (var x = 0; x < w; x++)
				{
					System.Console.Write(screen[x, y] ? "#" : '.');
					if (screen[x, y])
					{
						count++;
					}
				}
				System.Console.WriteLine();
			}
			System.Console.WriteLine($"{count} pixels are on");
			System.Console.WriteLine();
		}

		private static IEnumerable<(CommandType command, int value1, int value2)> ReadInput()
		{
			foreach (var line in File.ReadAllLines("input.txt"))
			{
				var tokens = line.Split(" ");
				if (tokens[0] == "rect")
				{
					tokens = tokens[1].Split("x");
					yield return (CommandType.Rect, Int32.Parse(tokens[0]), Int32.Parse(tokens[1]));
				}
				else if (tokens[0] == "rotate")
				{
					var cmd = tokens[1] == "row" ? CommandType.RotateRow : CommandType.RotateColumn;
					var by = Int32.Parse(tokens[4]);
					tokens = tokens[2].Split("=");
					yield return (cmd, Int32.Parse(tokens[1]), by);
				}
				else
				{
					throw new InvalidProgramException();
				}
			}
		}
	}
}


