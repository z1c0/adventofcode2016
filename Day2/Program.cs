using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day2
{

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

		private static void Part2()
		{
			var keyPad = new[,]
			{
				{ ' ', ' ', '1', ' ', ' ' },
				{ ' ', '2', '3', '4', ' ' },
				{ '5', '6', '7', '8', '9' },
				{ ' ', 'A', 'B', 'C', ' ' },
				{ ' ', ' ', 'D', ' ', ' ' },
			};
			Run(keyPad, 0, 2);
		}

		private static void Part1()
		{
			var keyPad = new[,]
			{
				{ '1', '2', '3' },
				{ '4', '5', '6' },
				{ '7', '8', '9' },
			};
			Run(keyPad, 1, 1);
		}

		private static void Run(char[,] keyPad, int x, int y)
		{
			var lines = ReadInput();
			foreach (var line in lines)
			{
				foreach (var c in line)
				{
					switch (c)
					{
						case 'U':
							TrySet(keyPad, ref x, ref y, x, y - 1);
							break;
						case 'D':
							TrySet(keyPad, ref x, ref y, x, y + 1);
							break;
						case 'L':
							TrySet(keyPad, ref x, ref y, x - 1, y);
							break;
						case 'R':
							TrySet(keyPad, ref x, ref y, x + 1, y);
							break;
					}
				}
				System.Console.WriteLine($"{x + 1}, {y + 1} -> {keyPad[y, x]}");
			}
			System.Console.WriteLine();
		}

		private static void TrySet(char[,] keyPad, ref int x, ref int y, int newX, int newY)
		{
			var w = keyPad.GetLength(0);
			var h = keyPad.GetLength(1);
			if (newX >= 0 && newX < w && newY >= 0 && newY < h && keyPad[newY, newX] != ' ')
			{
				x = newX;
				y = newY;
			}
		}

		private static List<string> ReadInput()
		{
			return File.ReadAllLines("input.txt").ToList();
		}
	}
}


