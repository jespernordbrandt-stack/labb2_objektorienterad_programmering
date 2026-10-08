//Klassen “Wall” ärver direkt av “LevelElement”, och klasserna “Rat” och “Snake” ärver direkt av “Enemy” (indirekt av “LevelElement”).





public class Wall : LevelElement
{
	public Wall()
	{
		Symbol = '#';
		Color = ConsoleColor.Gray;
	}
}
