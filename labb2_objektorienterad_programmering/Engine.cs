//Vision range:

//För att få en effekt av “utforskande” i spelet begränsar vi spelarens synfält till att bara visa objekt inom en radie av 5 tecken (men ni kan också prova med andra radier); Väggarna försvinner dock aldrig när man väl sett dem, men fienderna syns inte så fort de kommer utanför radien.

//Avståndet mellan två punkter i 2D kan enkelt beräknas med hjälp av pythagoras sats.

//Hålla koll på tillståndet: Hålla reda på instanser av LevelData, Player och spelets status (t.ex. om spelet är igång eller om spelaren är död).

//Game Loop: Köra loopen som väntar på tangentbordstryck (Console.ReadKey()), flyttar spelaren, anropar .Update() på alla fiender i LevelData, och kollar kollisioner/attacker.

//Rendering / Synfält: Anropa LevelData-elementen och räkna ut Pythagoras sats för att avgöra vad som ska ritas ut inom radie 5.
using System;

public class Class1
{
	public Class1()
	{
	}
}
