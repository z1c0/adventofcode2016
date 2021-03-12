using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 25

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1()
{
	var instructions = ReadInput().ToList();
	var cpu = new Cpu();
	for (var a = 0; a < 100000; a++)
	{
		cpu.Reset(a);
		cpu.Run(instructions);
		var expect = 0;
		var found = true;
		foreach (var i in cpu.Buffer)
		{
			if (i != expect)
			{
				found = false;
				break;
			}
			expect = (expect == 0) ? 1 : 0;
		}
		if (found)
		{
			System.Console.WriteLine($"Input {a} yields the signal.");
			break;
		}
	}
}

static IEnumerable<Instruction> ReadInput()
{
	foreach (var line in File.ReadLines("input.txt"))
	{
		var tokens = line.Split(' ');
		yield return tokens[0] switch {
			"cpy" => new Instruction(OpCode.Cpy, new NumberOrRegister(tokens[1]), new NumberOrRegister(tokens[2])),
			"inc" => new Instruction(OpCode.Inc, new NumberOrRegister(tokens[1]), new NumberOrRegister()),
			"dec" => new Instruction(OpCode.Dec, new NumberOrRegister(tokens[1]), new NumberOrRegister()),
			"jnz" => new Instruction(OpCode.Jnz, new NumberOrRegister(tokens[1]), new NumberOrRegister(tokens[2])),
			"out" => new Instruction(OpCode.Out, new NumberOrRegister(tokens[1]), new NumberOrRegister()),
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

	internal List<int> Buffer { get; } = new List<int>();

	internal void Reset(int a = 0, int b = 0, int c = 0, int d = 0)
	{
		Registers['a'] = a;
		Registers['b'] = b;
		Registers['c'] = c;
		Registers['d'] = d;
		Buffer.Clear();
	}

	internal void Run(List<Instruction> instructions)
	{
		var sb = new StringBuilder();
		var pc = 0;
		var count = 0;
		while (pc < instructions.Count && count++ < 100000)
		{
			var i = instructions[pc];
			//System.Console.WriteLine($"PC: {pc}, code: {i.Code}, a: {Registers['a']}, b: {Registers['b']}, c: {Registers['c']}, d: {Registers['d']}");
			switch (i.Code)
			{
				case OpCode.Cpy:
					Registers[i.NR2.Register] = GetValue(i.NR1);
					pc++;
					break;
				case OpCode.Inc:
					Registers[i.NR1.Register]++;
					pc++;
					break;
				case OpCode.Dec:
					Registers[i.NR1.Register]--;
					pc++;
					break;
				case OpCode.Jnz:
					if (GetValue(i.NR1) != 0)
					{
						pc += GetValue(i.NR2);
					}
					else
					{
						pc++;
					}
					break;
				case OpCode.Out:
					Buffer.Add(GetValue(i.NR1));
					pc++;
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

internal class Instruction
{
	public Instruction(OpCode code, NumberOrRegister nr1, NumberOrRegister nr2)
	{
		Code = code;
		NR1 = nr1;
		NR2 = nr2;
	}

	public OpCode Code { get; internal set; }
	public NumberOrRegister NR2 { get; internal set; }
	public NumberOrRegister NR1 { get; internal set; }
}

internal class NumberOrRegister
{
	private char _register;
	private int _number;

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

	public override string ToString()
	{
		return IsRegister ? Register.ToString() : Number.ToString();
	}

	public char Register
	{ 
		get
		{
			if (!IsRegister)
			{
				throw new InvalidOperationException();
			}
			return _register;
		}
		internal set => _register = value;
	}

	public bool IsRegister { get; internal set; }
	public int Number
	{
		get
		{
			if (IsRegister)
			{
				throw new InvalidOperationException();
			}
			return _number;
		}
		internal set => _number = value;
	}
}

enum OpCode
{
	Cpy,
	Inc,
	Dec,
	Jnz,
	Out
}