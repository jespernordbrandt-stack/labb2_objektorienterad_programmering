//När en spelare attackerar (går in i) en fiende, eller tvärtom så behöver vi simulera tärningskast för att få en poäng som avgör hur mycket skada attacken gör. Den som attackerar kastar då sina tärningar först och får en attackpoäng.
// Sedan kastar den som försvarar sina tärningar och får en försvarspoäng. Ta sedan attackpoängen minus försvarspoängen, och om differensen är större än 0, dra motsvarande antal från förvararens HP (hälsopoäng). Efter en eller flera attacker kommer HP ner till 0, varpå fienden dör (eller spelaren får game over).
using System;

public class Class1
{
	public Class1()
	{
	}
}
