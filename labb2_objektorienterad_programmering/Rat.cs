//Rat förflyttar sig 1 steg i slumpmässig vald riktning (upp, ner, höger eller vänster) varje omgång.
//Rat: HP = 10, Attack = 1d6+3, Defence = 1d6+1

namespace labb2
{
    public class Rat : Enemy
    {
        private static Random random = new Random();

        public Rat(int x, int y)
            : base(x, y, 'r', ConsoleColor.DarkRed, "Rat", 10, new Dice(1, 6, 3), new Dice(1, 6, 1))
        {
        }

        // Lägg till (Player player, LevelData levelData) här så slutar det lysa rött
        public override void Update(Player player, LevelData levelData)
        {
            int direction = random.Next(0, 4);

            switch (direction)
            {
                case 0: X++; break; // Höger
                case 1: X--; break; // Vänster
                case 2: Y++; break; // Nedåt
                case 3: Y--; break; // Uppåt
            }
        }
    }
}