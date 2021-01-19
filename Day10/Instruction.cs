namespace Day10
{
	internal class Instruction
	{
		public Instruction(int value, int botId)
		{
			IsInput = true;
			Value = value;
			BotId = botId;
		}

		public Instruction(int botId, bool lowToBin, int outputLow, bool hiToBin, int outputHi)
		{
			IsInput = false;
			BotId = botId;
			LowToBin = lowToBin;
			OutputLow = outputLow;
			HiToBin = hiToBin;
			OutputHi = outputHi;
		}

		public bool IsInput { get; }
		public int Value { get; }
		public int BotId { get; }
		public bool LowToBin { get; }
		public int OutputLow { get; }
		public bool HiToBin { get; }
		public int OutputHi { get; }
		public bool Processed { get; internal set; }

		public override string ToString()
		{
			string BinToString(bool isBin) => isBin ? "bin" : "bot";
			return IsInput ?
				$"{Value} -> {BotId}" :
				$"{BotId}: low to {BinToString(LowToBin)} {OutputLow}, hi to {BinToString(HiToBin)} {OutputHi}";
		}
	}
}