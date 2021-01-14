using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Day6
{

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
			var lines = ReadInput().ToList();
			var lineLength = lines.First().Length;
			var message1 = new char[lineLength];
			var message2 = new char[lineLength];
			for (var pos = 0; pos < lineLength; pos++)
			{
				var dict = new Dictionary<char, int>();
				foreach (var line in ReadInput())
				{
					var c = line[pos];
					if (!dict.ContainsKey(c))
					{
						dict.Add(c, 1);
					}
					else
					{
						dict[c]++;
					}
				}
				var sorted = dict.OrderBy(e => e.Value);
				message1[pos] = sorted.Last().Key;
				message2[pos] = sorted.First().Key;
			}
			System.Console.WriteLine($"Message 1: {new string(message1)}");
			System.Console.WriteLine($"Message 2: {new string(message2)}");
		}

		private static IEnumerable<string> ReadInput()
		{
			return File.ReadAllLines("input.txt");
		}
	}
}


