//Vision range:

//För att få en effekt av “utforskande” i spelet begränsar vi spelarens synfält till att bara visa objekt inom en radie av 5 tecken (men ni kan också prova med andra radier); Väggarna försvinner dock aldrig när man väl sett dem, men fienderna syns inte så fort de kommer utanför radien.

//Avståndet mellan två punkter i 2D kan enkelt beräknas med hjälp av pythagoras sats.

//Hålla koll på tillståndet: Hålla reda på instanser av LevelData, Player och spelets status (t.ex. om spelet är igång eller om spelaren är död).

//Game Loop: Köra loopen som väntar på tangentbordstryck (Console.ReadKey()), flyttar spelaren, anropar .Update() på alla fiender i LevelData, och kollar kollisioner/attacker.


namespace labb2
{
    public class GameLoop
    {
        private LevelData levelData;
        private bool isRunning;

        public GameLoop(string filePath)
        {
            // Här läses Level1.txt in och skapar alla väggar, fiender och spelaren
            levelData = new LevelData(filePath);
            isRunning = true;
        }

        public void Run()
        {
            // Enkel kontroll så att spelaren faktiskt skapats från kartan
            if (levelData.Player == null)
            {
                Console.WriteLine("Kunde inte hitta spelaren ('@') i kartfilen!");
                Console.ReadKey();
                return;
            }

            while (isRunning)
            {
                Console.Clear();

                // 1. Rita ut alla element på banan (väggar, råttor, ormar etc.)
                foreach (var element in levelData.Elements)
                {
                    element.Draw();
                }

                // 2. Rita ut spelaren
                levelData.Player.Draw();

                // 3. Lyssna på spelarens tangentbordstryckningar
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    // Tryck på ESC för att avsluta spelet
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                        break;
                    }

                    // Räkna ut nästa position via Player-klassens HandleInput
                    int newX, newY;
                    levelData.Player.HandleInput(keyInfo, out newX, out newY);

                    // Enkel koll: Om vi vill bygga ut kollar vi om ny position är en vägg här.
                    // För att bara testa rörelsen just nu flyttar vi spelaren direkt:
                    levelData.Player.X = newX;
                    levelData.Player.Y = newY;
                }

                // Liten paus för att inte överbelasta processorn
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}