//Klassen “Wall” ärver direkt av “LevelElement”, och klasserna “Rat” och “Snake” ärver direkt av “Enemy” (indirekt av “LevelElement”).




namespace labb2
{
	public class Wall : LevelElement
	{
		public Wall(int x, int y) : base(x, y, '#', ConsoleColor.Gray)
		{

		}
	}
}