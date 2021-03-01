using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 17

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(false);
Part1(true);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(bool findLongestPath)
{
	var passcode = ReadInput();
	var path = string.Empty;
	var extremePathLen = findLongestPath ? 0 : Int32.MaxValue;
	Step((0, 0), passcode, path, ref extremePathLen, findLongestPath);
}

void Step((int x, int y) pos, string passcode, string path, ref int extremePathLen, bool findLongestPath)
{
	if (pos.x == 3 && pos.y == 3)
	{
		if (findLongestPath)
		{
			if (path.Length > extremePathLen)
			{
				extremePathLen = path.Length;
				System.Console.WriteLine($"Longest path to vault: {extremePathLen}");
			}
		}
		else
		{
			if (path.Length < extremePathLen)
			{
				extremePathLen = path.Length;
				System.Console.WriteLine($"Shortest path to vault: {path}");
			}
		}
		return;
	}
	if (pos.x < 0 || pos.y < 0 || pos.x > 3 || pos.y > 3)
	{
		return;
	}

	var hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(passcode + path))).Replace("-","").ToLower();
	if (IsDoorOpen(hash[0]))
	{
		Step((pos.x, pos.y - 1), passcode, path + "U", ref extremePathLen, findLongestPath);
	}
	if (IsDoorOpen(hash[1]))
	{	 
		Step((pos.x, pos.y + 1), passcode, path + "D", ref extremePathLen, findLongestPath);
	}
	if (IsDoorOpen(hash[2]))
	{	
		Step((pos.x - 1, pos.y), passcode, path + "L", ref extremePathLen, findLongestPath);
	}
	if (IsDoorOpen(hash[3]))
	{	
		Step((pos.x + 1, pos.y), passcode, path + "R", ref extremePathLen, findLongestPath);
	}
}

static bool IsDoorOpen(char c) => c >= 'b' && c <= 'f';

string ReadInput()
{
	return File.ReadAllText("input.txt");
}