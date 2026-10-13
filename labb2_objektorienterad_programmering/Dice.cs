//När en spelare attackerar (går in i) en fiende, eller tvärtom så behöver vi simulera tärningskast för att få en poäng som avgör hur mycket skada attacken gör. Den som attackerar kastar då sina tärningar först och får en attackpoäng.
// Sedan kastar den som försvarar sina tärningar och får en försvarspoäng. Ta sedan attackpoängen minus försvarspoängen, och om differensen är större än 0, dra motsvarande antal från förvararens HP (hälsopoäng). Efter en eller flera attacker kommer HP ner till 0, varpå fienden dör (eller spelaren får game over).


namespace labb2
{
	public class Dice
	{
		public int NumberOfDice { get; set; }
		public int SidesPerDice { get; set; }
		public int Modifier { get; set; }

		private static Random random = new Random();

		public Dice (int numberOfDice, int sidesPerDice, int modifier)
		{
			NumberOfDice = numberOfDice;
			SidesPerDice = sidesPerDice;
			Modifier = modifier;
		}
			
		public int Throw()
		{
			int total = 0;
			for (int i = 0; i < NumberOfDice; i++)
			{
				total += random.Next(1, SidesPerDice + 1);
			}
			return total + Modifier;
		}

		public override string ToString()
		{
			return $"{NumberOfDice}d{SidesPerDice}+{Modifier}";
		}
	}
}