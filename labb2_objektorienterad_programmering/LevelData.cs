//Skapa en klass “LevelData” som har en private field “elements” av typ List<LevelElement>. Denna ska även exponeras i form av en public readonly property “Elements”.
//Vidare har LevelData en metod, Load(string filename), som läser in data från filen man anger vid anrop. Load läser igenom textfilen tecken för tecken, och för varje tecken den hittar (som är någon av #, r, eller s) så skapar den en ny instans av den klass som motsvarar tecknet och lägger till en referens till instansen på “elements”-listan. Tänk på att när instansen skapas så måste den även få en (X/Y) position; d.v.s Load behöver alltså hålla reda på vilken rad och kolumn i filen som tecknet hittades på. Den behöver även spara undan startpositionen för spelaren när den stöter på @.



namespace labb2
{
    public class LevelData
    {
        // En lista som håller reda på alla element på banan (väggar, fiender etc.)
        public List<LevelElement> Elements { get; set; } = new List<LevelElement>();

        // Håller reda på spelarens startposition
        public Player Player { get; set; }

        public LevelData(string filePath)
        {
            LoadLevel(filePath);
        }

        private void LoadLevel(string filePath)
        {
            // Kontrollera att filen finns
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Kunde inte hitta kartfilen: {filePath}");
            }

            // Läs alla rader från textfilen
            string[] lines = File.ReadAllLines(filePath);

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    char c = line[x];

                    // Skapa objekt beroende på vilket tecken som står i filen
                    switch (c)
                    {
                        case '#':
                            Elements.Add(new Wall(x, y));
                            break;
                        case 'r':
                            Elements.Add(new Rat(x, y));
                            break;
                        case 's':
                            Elements.Add(new Snake(x, y));
                            break;
                        case '@':
                            // Skapar spelaren på denna position
                            Player = new Player(x, y, "Hjälten");
                            break;
                        case ' ':
                            // Mellanslag är tom yta, gör inget
                            break;
                    }
                }

            }

        }
        // Kontrollera om en viss position är tillgänglig (inte vägg)
        public bool IsWalkable(int x, int y)
        {
            foreach (var element in Elements)
            {
                if (element.X == x && element.Y == y)
                {
                    // Om det är en vägg så kan man inte gå där
                    if (element is Wall)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

}