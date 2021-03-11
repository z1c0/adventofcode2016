using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// Day 23

Console.WriteLine("START");
var sw = Stopwatch.StartNew();
Part1(7);
// This does not terminate (in reasonable time)
// Part1(12);
// Part2 transformed into C# code does.
Part2();
Console.WriteLine($"END (after {sw.Elapsed.TotalSeconds} seconds)");

void Part1(int a)
{
	var instructions = ReadInput().ToList();
	var cpu = new Cpu(a);
	cpu.Run(instructions);
	System.Console.WriteLine($"Register a: {cpu.Registers['a']}");
}

void Part2()
{
	long a = 12;
	long b = a;
	long c = 0;
	long d = 0;
	b--;
	while (true)
	{
		c = 0;
		d = a;
		a = 0;
		while (d != 0)
		{
			c = b;
			while (c != 0)
			{
				a++;
				c--;
			}
			d--;
		}

		b--;
		c = b;
		d = c;
		while (d != 0)
		{
			d--;
			c++;
		}
		
		if (c == 2)
		{
			break;
		}
	}

	c = 16;
	c = 1;
	c = 83;
	while (c != 0)
	{
		d = 78;
		while (d != 0)
		{
			d--;
			a++;
		}
		c--;
	}
	System.Console.WriteLine($"a: {a}");
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
			"tgl" => new Instruction(OpCode.Tgl, new NumberOrRegister(tokens[1]), new NumberOrRegister()),
			_ => throw new InvalidProgramException()
		};
	}
}

internal class Cpu
{
	public Cpu(int a)
	{
		Reset(a, 0, 0, 0);
	}

	internal Dictionary<char, int> Registers { get; } = new Dictionary<char, int>();

	internal void Reset(int a, int b, int c, int d)
	{
		Registers['a'] = a;
		Registers['b'] = b;
		Registers['c'] = c;
		Registers['d'] = d;
	}

	internal void Run(List<Instruction> instructions)
	{
		var pc = 0;
		while (pc < instructions.Count)
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
				case OpCode.Tgl:
					{
						var ii = instructions.ElementAtOrDefault(pc + GetValue(i.NR1));
						if (ii != null)
						{
							if (ii.Arity == 1)
							{
								ii.Code = (ii.Code == OpCode.Inc) ? OpCode.Dec : OpCode.Inc;
							}
							else if (ii.Arity == 2)
							{
								ii.Code = (ii.Code == OpCode.Jnz) ? OpCode.Cpy : OpCode.Jnz;
							}
							else
							{
								throw new InvalidOperationException();
							}
						}
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
	public int Arity
	{
		get
		{
			return Code switch
			{
				OpCode.Cpy => 2,
				OpCode.Jnz => 2,
				OpCode.Dec => 1,
				OpCode.Inc => 1,
				OpCode.Tgl => 1,
				_ => throw new InvalidProgramException(),
			};
		}
	}
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
	Tgl
}