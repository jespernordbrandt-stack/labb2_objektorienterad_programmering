//Player: HP = 100, Attack = 2d6+2, Defence = 2d6+0
//Logiken för hur man kollar avstånd.


namespace labb2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Skapa spelet och skicka med filnamnet på din karta
            GameLoop game = new GameLoop("Level1.txt");
            game.Run();
        }
    }
}