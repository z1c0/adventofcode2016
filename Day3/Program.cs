using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day3
{

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("START");
			var sw = Stopwatch.StartNew();
			Part1(false);
			Part1(true);
			Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");
		}

		private static void Part1(bool readVertically)
		{
			var triangles = ReadInput().ToList();
			if (readVertically)
			{
				var tmp = new List<(int side1, int side2, int side3)>();
				for (var i = 0; i < triangles.Count - 2; i += 3)
				{
					tmp.Add((triangles[i + 0].side1, triangles[i + 1].side1, triangles[i + 2].side1));
					tmp.Add((triangles[i + 0].side2, triangles[i + 1].side2, triangles[i + 2].side2));
					tmp.Add((triangles[i + 0].side3, triangles[i + 1].side3, triangles[i + 2].side3));
				}
				triangles = tmp;
			}
			var count = triangles.Count(t => 
				t.side1 + t.side2 > t.side3 &&
				t.side1 + t.side3 > t.side2 &&
				t.side2 + t.side3 > t.side1
			);
			System.Console.WriteLine($"{count} items are possible triangles");
		}

		private static IEnumerable<(int side1, int side2, int side3)> ReadInput()
		{
			foreach (var line in File.ReadAllLines("input.txt"))
			{
				var tokens = line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				yield return (Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), Int32.Parse(tokens[2]));
			}
		}
	}
}


