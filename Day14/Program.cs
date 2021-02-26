using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 14

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(false);
Part1(true);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(bool stretchHash)
{
	var salt = ReadInput();
	var foundHashes = 0;
	var index = 0;
	var cache = new Dictionary<int, string>();
	while (foundHashes < 64)
	{
		var hash = GetHash(salt, index, stretchHash, cache);
		for (var i = 0; i < hash.Length - 2; i++)
		{
			var c = hash[i];
			// find triples
			if (c == hash[i + 1] && c == hash[i + 2])
			{
				// find quintuple
				for (var index2 = index + 1; index2 <= index + 1000; index2++)
				{
					var hashFound = false;
					var hash2 = GetHash(salt, index2, stretchHash, cache);
					for (var j = 0; j < hash2.Length - 4; j++)
					{
						var c2 = hash2[j];
						if (c == c2 && c2 == hash2[j + 1] && c2 == hash2[j + 2] && c2 == hash2[j + 3] && c2 == hash2[j + 4])
						{
							hashFound = true;
							break;
						}
					}
					if (hashFound)
					{
							System.Console.Write(".");
							foundHashes++;
							break;
					}
				}
				break;
			}
		}
		index++;
	}

	System.Console.WriteLine($" index {index - 1} produces the 64th key.");
}

string GetHash(string salt, int index, bool stretchHash, Dictionary<int, string> cache)
{
	if (!cache.ContainsKey(index))
	{
		var hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + index))).Replace("-", "").ToLower();
		if (stretchHash)
		{
			for (var i = 0; i < 2016; i++)
			{
				hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(hash))).Replace("-", "").ToLower();
			}
		}
		cache[index] = hash;
	}
	return cache[index];
}

static string ReadInput()
{
	return File.ReadAllText("input.txt");
}
