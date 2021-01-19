using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Day9
{
	internal interface Chunk
	{
		int Position { get; }
		int Length { get; }

		long CalculateLength(List<Chunk> compressed, ref int pos);
	}

	internal class Letter : Chunk
	{
		private char _c;

		public int Position { get; }

		public int Length => 1;

		public Letter(char c, int pos)
		{
			_c = c;
			Position = pos;
		}

		public override string ToString()
		{
			return $"{_c}";
		}

		public long CalculateLength(List<Chunk> compressed, ref int pos)
		{
			pos++;
			return 1;
		}
	}

	internal class Marker : Chunk
	{		
		public Marker(int nrOfChars, int repeatCount, int pos)
		{
			NrOfChars = nrOfChars;
			RepeatCount = repeatCount;
			Position = pos;
		}

		public int NrOfChars { get; }
		public int RepeatCount { get; }

		public int Length => ToString().Length;

		public int Position { get; }

		public long CalculateLength(List<Chunk> compressed, ref int pos)
		{
			long length = 0;

			pos += ToString().Length;

			while (true)
			{
				var c = Program.GetChunkForPosition(compressed, pos);
				length += c.CalculateLength(compressed, ref pos);
				if (pos >= Position + Length + NrOfChars)
				{
					break;
				}
			}

			//System.Console.WriteLine($"{length} * {RepeatCount} -> {length * RepeatCount}");
			length *= RepeatCount;

			return length;
		}

		public override string ToString()
		{
			return $"[{NrOfChars}*{RepeatCount}]";
		}
	}
}