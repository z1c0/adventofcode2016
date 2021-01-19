using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Day10
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
			var instructions = ReadInput();
			var bots = new List<Bot>();
			var bins = new List<Bin>();
			var inputs = instructions.Where(i => i.IsInput).ToList();
			foreach (var i in inputs)
			{
				EnsureBot(bots, i.BotId).Input(i.Value);
				i.Processed = true;
			}
			var outputs = instructions.Where(i => !i.IsInput).ToList();
			while (outputs.Any(o => !o.Processed))
			{
				foreach (var o in outputs)
				{
					if (!o.Processed)
					{
						var bot = EnsureBot(bots, o.BotId);
						if (bot.Has2Chips)
						{
							bot.GiveTo(false, o.LowToBin, o.OutputLow, bots, bins);
							bot.GiveTo(true, o.HiToBin, o.OutputHi, bots, bins);
							o.Processed = true;
						}
					}
				}
			}

			var bin0 = bins.Single(b => b.Id == 0);
			var bin1 = bins.Single(b => b.Id == 1);
			var bin2 = bins.Single(b => b.Id == 2);
			System.Console.WriteLine($"{bin0.Value} * {bin1.Value} * {bin2.Value} = {bin0.Value * bin1.Value * bin2.Value}");
		}

		internal static Bin EnsureBin(List<Bin> bins, int binId)
		{
			var bin = bins.SingleOrDefault(b => b.Id == binId);
			if (bin == null)
			{
				bin = new Bin(binId);
				bins.Add(bin);
			}
			return bin;
		}

		internal static Bot EnsureBot(List<Bot> bots, int botId)
		{
			var bot = bots.SingleOrDefault(b => b.Id == botId);
			if (bot == null)
			{
				bot = new Bot(botId);
				bots.Add(bot);
			}
			return bot;
		}

		private static IEnumerable<Instruction> ReadInput()
		{
			foreach (var line in File.ReadAllLines("input.txt"))
			{
				var tokens = line.Split(" ");
				if (tokens[0] == "value")
				{
					yield return new Instruction(
						Int32.Parse(tokens[1]),
						Int32.Parse(tokens[5]));
				}
				else if (tokens[0] == "bot")
				{
					var botId = Int32.Parse(tokens[1]);
					if (tokens[3] != "low")
					{
						throw new InvalidProgramException();
					}
					var lowToBin = tokens[5] == "output";
					var outputLow = Int32.Parse(tokens[6]);
					if (tokens[8] != "high")
					{
						throw new InvalidProgramException();
					}
					var hiToBin = tokens[10] == "output";
					var outputHi = Int32.Parse(tokens[11]);
					yield return new Instruction(
						botId, 
						lowToBin,
						outputLow,
						hiToBin,
						outputHi);
				}
				else
				{
					throw new InvalidProgramException();
				}
			}
		}
	}
}


