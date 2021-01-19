using System;
using System.Collections.Generic;

namespace Day10
{
	internal class Bin
	{
		public Bin(int id)
		{
			Id = id;
			Value = -1;
		}

		public int Id { get; }
		public int Value { get; private set; }

		internal void Add(int value)
		{
			if (Value != -1)
			{
				throw new InvalidOperationException();
			}
			Value = value;
		}
	}
	internal class Bot
	{
		public Bot(int id)
		{
			Id = id;
			Low = -1;
			Hi = -1;
		}

		public int Id { get; }
		public int Low { get; private set; }
		public int Hi { get; private set; }
		public bool Has2Chips { get => Hi != -1 && Low != -1; }

		internal void Input(int value)
		{
			if (Low == -1)
			{
				Low = value;
			}
			else if (Hi == -1)
			{
				Hi = value;
				if (Low > Hi)
				{
					var tmp = Hi;
					Hi = Low;
					Low = tmp;
				}
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		internal void GiveTo(bool useHi, bool useBin, int to, List<Bot> bots, List<Bin> bins)
		{
			Check();
			var value = 0;
			if (useHi)
			{
				value = Hi;
				Hi = -1;
			}
			else
			{
				value = Low;
				Low = -1;
			}
			if (useBin)
			{
				Program.EnsureBin(bins, to).Add(value);
			}
			else
			{
				Program.EnsureBot(bots, to).Input(value);
			}
		}

		public override string ToString()
		{
			return $"Bot {Id} ({Low}, {Hi})";
		}

		private void Check()
		{
			if (Hi == 61 && Low == 17 || Hi == 17 && Low == 61)
			{
				System.Console.WriteLine(this);
			}
		}
	}
}