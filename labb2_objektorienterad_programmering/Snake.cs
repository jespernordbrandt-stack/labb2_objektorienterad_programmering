//Snake står still om spelaren är mer än 2 rutor bort, annars förflyttar den sig bort från spelaren.
//Snake: HP = 25, Attack = 3d4+2, Defence = 1d8+5



namespace labb2
{
    public class Snake : Enemy
    {
        public Snake(int x, int y) : base(x, y, 's', ConsoleColor.Green, "Snake", 25, new Dice(3, 4, 2), new Dice(1, 8, 5))
        {
        }

        public override void Update(Player player, LevelData levelData)
        {
            int deltaX = player.X - X;
            int deltaY = player.Y - Y;

            // Beräkna avståndet med Pythagoras sats
            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            // Står still om spelaren är mer än 2 rutor bort, annars flyttar den sig bort
            if (distance <= 2.0)
            {
                int moveX = X;
                int moveY = Y;

                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                {
                    moveX += (deltaX > 0) ? -1 : 1;
                }
                else
                {
                    moveY += (deltaY > 0) ? -1 : 1;
                }

                if (levelData.IsWalkable(moveX, moveY))
                {
                    X = moveX;
                    Y = moveY;
                }
            }
        }
    }
}