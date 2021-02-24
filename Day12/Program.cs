using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// Day 12

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var instructions = ReadInput().ToList();
	var cpu = new Cpu();
	cpu.Run(instructions);
	System.Console.WriteLine($"Register a: {cpu.Registers['a']}");
	// part 2
	cpu.Reset(0, 0, 1, 0);
	cpu.Run(instructions);
	System.Console.WriteLine($"Register a: {cpu.Registers['a']}");
}

static IEnumerable<(OpCode code, NumberOrRegister nr1, NumberOrRegister nr2)> ReadInput()
{
	foreach (var line in File.ReadLines("input.txt"))
	{
		var tokens = line.Split(' ');
		yield return tokens[0] switch {
			"cpy" => (OpCode.Cpy, new NumberOrRegister(tokens[1]), new NumberOrRegister(tokens[2])),
			"inc" => (OpCode.Inc, new NumberOrRegister(tokens[1]), new NumberOrRegister()),
			"dec" => (OpCode.Dec, new NumberOrRegister(tokens[1]), new NumberOrRegister()),
			"jnz" => (OpCode.Jnz, new NumberOrRegister(tokens[1]), new NumberOrRegister(tokens[2])),
			_ => throw new InvalidProgramException()
		};
	}
}

internal class Cpu
{
	public Cpu()
	{
		Reset(0, 0, 0, 0);
	}

	internal Dictionary<char, int> Registers { get; } = new Dictionary<char, int>();

	internal void Reset(int a, int b, int c, int d)
	{
		Registers['a'] = a;
		Registers['b'] = b;
		Registers['c'] = c;
		Registers['d'] = d;
	}

	internal void Run(List<(OpCode code, NumberOrRegister nr1, NumberOrRegister nr2)> instructions)
	{
		var pc = 0;
		while (pc < instructions.Count)
		{
			var i = instructions[pc];
			switch (i.code)
			{
				case OpCode.Cpy:
					Registers[i.nr2.Register] = GetValue(i.nr1);
					pc++;
					break;
				case OpCode.Inc:
					Registers[i.nr1.Register]++;
					pc++;
					break;
				case OpCode.Dec:
					Registers[i.nr1.Register]--;
					pc++;
					break;
				case OpCode.Jnz:
					if (GetValue(i.nr1) != 0)
					{
						pc += GetValue(i.nr2);
					}
					else
					{
						pc++;
					}
					break;
				default:
					throw new InvalidOperationException();
			}
		}
	}

	private int GetValue(NumberOrRegister nr)
	{
		return nr.IsRegister ? Registers[nr.Register] : nr.Number;
	}
}

internal class NumberOrRegister
{
	public NumberOrRegister()
	{
	}

	public NumberOrRegister(string s)
	{
		if (int.TryParse(s, out var n))
		{
			Number = n;
		}
		else
		{
			Register = s[0];
			IsRegister = true;
		}
	}

	public char Register { get; internal set; }
	public bool IsRegister { get; internal set; }
	public int Number { get; internal set; }
}

enum OpCode
{
	Cpy,
	Inc,
	Dec,
	Jnz
}