using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Day5
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
			var doorId = ReadInput();
			var password = new char[8];
			ulong i = 0;
			while (password.Any(c => c == 0))
			{
				var input = doorId + (i++).ToString();
				var hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-","").ToLower();
				if (hash.StartsWith("00000"))
				{
					var pos = hash[5] - '0';
					if (pos < 8 && password[pos] == 0)
					{
						password[pos] = hash[6];
						System.Console.WriteLine($"Password: {new string(password)}");
					}
				}
			}
		}

		private static void Part1()
		{
			var doorId = ReadInput();
			var password = string.Empty;
			var i = 0;
			while (password.Length < 8)
			{
				var input = doorId + (i++).ToString();
				var hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-","").ToLower();
				if (hash.StartsWith("00000"))
				{
					password += hash[5];
				}
			}
			System.Console.WriteLine($"Password: {password}");
			System.Console.WriteLine();
		}

		private static string ReadInput()
		{
			return File.ReadAllText("input.txt");
		}
	}
}


