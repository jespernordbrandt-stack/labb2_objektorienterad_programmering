


namespace labb2
{
    public abstract class Enemy : LevelElement
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public Dice AttackDice { get; set; }
        public Dice DefenceDice { get; set; }

        protected Enemy(int x, int y, char symbol, ConsoleColor color, string name, int hp, Dice attackDice, Dice defenceDice)
            : base(x, y, symbol, color)
        {
            Name = name;
            HP = hp;
            AttackDice = attackDice;
            DefenceDice = defenceDice;
        }

        // Måste ha samma parametrar här
        public abstract void Update(Player player, LevelData levelData);
    }
}