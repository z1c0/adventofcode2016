using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day4
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
			var rooms = ReadInput().ToList();
			long sum = 0;
			foreach (var r in rooms)
			{
				var checkSum = CalculateCheckSum(r.name);
				if (checkSum == r.checkSum)
				{
					sum += r.sectorId;
					var roomName = Decrypt(r.name, r.sectorId);
					if (roomName.Contains("north"))
					{
						System.Console.WriteLine($"{roomName}: {r.sectorId}");
					}
				}
			}
			System.Console.WriteLine($"Sum of valid sector ids is {sum}");
		}

		private static string Decrypt(string name, int sectorId)
		{
			var sb = new StringBuilder();
			sectorId %= ('z' - 'a' + 1);
			foreach (var c in name)
			{
				if (c == '-')
				{
					sb.Append(' ');
				}
				else
				{
					var cc = (char)(c + sectorId);
					if (cc > 'z')
					{
						cc = (char)('a' + cc - 'z' - 1);
					}
					sb.Append(cc);
				}
			}
			return sb.ToString();
		}

		private static string CalculateCheckSum(string name)
		{
			var dict = new Dictionary<char, int>();
			foreach (var c in name)
			{
				if (c != '-')
				{
					if (!dict.ContainsKey(c))
					{
						dict.Add(c, 1);
					}
					else
					{
						dict[c]++;
					}
				}
			}
			var list = dict.ToList();
			list.Sort((e1, e2) => 
			{
				if (e1.Value == e2.Value)
				{
					return e1.Key.CompareTo(e2.Key);
				}
				return e2.Value.CompareTo(e1.Value);
			});
			return new string(list.Take(5).Select(e => e.Key).ToArray());
		}

		private static IEnumerable<(string name, int sectorId, string checkSum)> ReadInput()
		{
			foreach (var line in File.ReadAllLines("input.txt"))
			{
				var i = line.IndexOf('[');
				var s = line.Substring(0, i);
				var j = s.LastIndexOf('-');
				var name = s.Substring(0, j);
				var sectorId =  Int32.Parse(s.Substring(j + 1));
				var checkSum = line.Substring(i + 1, line.Length - i - 2);
				yield return (name, sectorId, checkSum);
			}
		}
	}
}


