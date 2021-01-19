using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Day9
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
			var input = ReadInput();
			var chunks = Parse(input);
			long uncompressedLength = 0;
			var pos = 0;
			while (pos < input.Length)
			{
				var c = GetChunkForPosition(chunks, pos);
				uncompressedLength += c.CalculateLength(chunks, ref pos);
			}
			System.Console.WriteLine($"Length of decompressed data: {uncompressedLength}");
		}

		internal static Chunk GetChunkForPosition(List<Chunk> chunks, int pos)
		{
			var i = 0;
			foreach (var c in chunks)
			{			
				i += c.Length;
				if (i > pos)
				{
					return c;
				}
			}
			return null;
		}

		private static List<Chunk> Parse(string input)
		{
			var chunks = new List<Chunk>();
			for (var i = 0; i < input.Length; i++)
			{
				var c = input[i];
				if (c == '(')
				{
					chunks.Add(ParseMarker(ref i, input));
				}
				else
				{
					chunks.Add(new Letter(c, i));
				}
			}
			return chunks;
		}		

		private static void Part1()
		{
			var input = ReadInput();
			var decompressed = Decompress(input);
			//System.Console.WriteLine(decompressed);
			System.Console.WriteLine($"Length of decompressed data: {decompressed.Length}");
		}

		private static string Decompress(string input)
		{
			var decompressed = new StringBuilder();
			for (var i = 0; i < input.Length; i++)
			{
				var c = input[i];
				if (c == '(')
				{
					var marker = ParseMarker(ref i, input);
					i++;
					decompressed.Append(ApplyMarker(marker, ref i, input));
				}
				else
				{
					decompressed.Append(c);
				}
			}
			return decompressed.ToString();
		}

		private static string ApplyMarker(Marker marker, ref int i, string input)
		{
			var sb = new StringBuilder();
			for (var j = 0; j < marker.NrOfChars; j++)
			{
				sb.Append(input[i++]);
			}
			i--;
			return string.Concat(Enumerable.Repeat(sb.ToString(), marker.RepeatCount));
		}

		private static Marker ParseMarker(ref int i, string input)
		{
			var sb = new StringBuilder();
			var pos = i;
			while (input[++i] != ')')
			{
				sb.Append(input[i]);
			}
			var tokens = sb.ToString().Split("x");
			return new Marker(Int32.Parse(tokens[0]), Int32.Parse(tokens[1]), pos);
		}

		private static string ReadInput()
		{
			return File.ReadAllText("input.txt");
		}
	}
}


