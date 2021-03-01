using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 16

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(272);
Part1(35651584);
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(int diskLength)
{
	var sb = new StringBuilder(diskLength);
	var data = ReadInput();
	while (data.Length < diskLength)
	{
		sb.Append(data);
		sb.Append('0');
		foreach (var c in data.Reverse())
		{
			sb.Append(c == '1' ? '0' : '1');
		}
		if (sb.Length > diskLength)
		{
			sb.Length = diskLength;
		}
		data = sb.ToString();
		sb.Clear();
	}

	// checksum
	var checksum = CheckSum(data);
	System.Console.WriteLine($"disk length {diskLength}: checksum: {checksum}");
}

string CheckSum(string data)
{
	var sb = new StringBuilder();
	do
	{
		for (var i = 0; i < data.Length; i += 2)
		{
			sb.Append(data[i] == data[i + 1] ? '1' : '0');
		}
		data = sb.ToString();
		sb.Clear();
	}
	while (data.Length % 2 == 0);

	return data;
}

static string ReadInput()
{
	return File.ReadAllText("input.txt");
}