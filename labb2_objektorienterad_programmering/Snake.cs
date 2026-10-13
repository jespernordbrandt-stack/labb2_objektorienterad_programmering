//Snake står still om spelaren är mer än 2 rutor bort, annars förflyttar den sig bort från spelaren.
//Snake: HP = 25, Attack = 3d4+2, Defence = 1d8+5



namespace labb2
{
    public class Snake : Enemy
    {
        private static Random random = new Random();

        public Snake(int x, int y)
            : base(x, y, 's', ConsoleColor.Green, "Snake", 20, new Dice(3, 4, 2), new Dice(1, 8, 5))
        {
        }

        public override void Update()
        {
        }
    }
}
