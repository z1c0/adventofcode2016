using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Day7
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
			var count1 = ReadInput().Count(SupportsTls);
			System.Console.WriteLine($"{count1} addresses support TLS.");
			var count2 = ReadInput().Count(SupportsSSl);
			System.Console.WriteLine($"{count2} addresses support SSL.");
		}

		private static bool SupportsSSl(string ip)
		{
			var bracketsOn = false;
			for (var i = 0; i < ip.Length - 2; i++)
			{
				var c = ip[i];
				if (c == '[')
				{
					bracketsOn = true;
				}
				else if (c == ']')
				{
					bracketsOn = false;
				}
				else
				{
					if (!bracketsOn)
					{
						var c1 = ip[i + 1];
						var c2 = ip[i + 2];
						if (c != c1 && c == c2)
						{
							// ABA found
							var bracketsOn2 = false;
							for (var j = 0; j < ip.Length - 2; j++)
							{
								var cc = ip[j];
								if (cc == '[')
								{
									bracketsOn2 = true;
								}
								else if (cc == ']')
								{
									bracketsOn2 = false;
								}
								else
								{
									if (bracketsOn2)
									{
										var cc1 = ip[j + 1];
										var cc2 = ip[j + 2];
										if (cc == c1 && cc1 == c && cc1 != cc2 && cc == cc2)
										{
											// BAB found
											//System.Console.WriteLine($"{c}{c1}{c2} - {cc}{cc1}{cc2}");
											return true;
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		private static bool SupportsTls(string ip)
		{
			var abbaFound = false;
			var abbaFoundInBrackets = false;
			var bracketsOn = false;
			for (var i = 0; i < ip.Length - 3; i++)
			{
				var c = ip[i];
				if (c == '[')
				{
					bracketsOn = true;
				}
				else if (c == ']')
				{
					bracketsOn = false;
				}
				else
				{
					var c1 = ip[i + 1];
					var c2 = ip[i + 2];
					var c3 = ip[i + 3];
					if (c != c1 && c == c3 && c1 == c2)
					{
						abbaFound = true;
						if (bracketsOn)
						{
							abbaFoundInBrackets = true;
							break;
						}
					}
				}
			}
			return abbaFound && !abbaFoundInBrackets;
		}

		private static IEnumerable<string> ReadInput()
		{
			return File.ReadAllLines("input.txt");
		}
	}
}


