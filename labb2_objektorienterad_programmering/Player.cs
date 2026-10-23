

namespace labb2
{
    public class Player : LevelElement
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }

        public Player(int x, int y, string name)
            : base(x, y, '@', ConsoleColor.Yellow)
        {
            Name = name;
            HP = 100; // Standardvärde för spelaren
            AttackDice = new Dice(2, 6, 2);  // Exempel: 2d6+2
            DefenceDice = new Dice(2, 6, 2); // Exempel: 2d6+2
        }

        // Beräknar vart spelaren vill flytta baserat på tangentbordet
        public void HandleInput(ConsoleKeyInfo keyInfo, out int newX, out int newY)
        {
            newX = X;
            newY = Y;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    newY--;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    newY++;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    newX--;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    newX++;
                    break;
            }
        }
    }
}