//Player: HP = 100, Attack = 2d6+2, Defence = 2d6+0
//Logiken för hur man kollar avstånd.



namespace labb2
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            // Skapa spelet och skicka med filnamnet på din karta
            GameLoop game = new GameLoop(@"C:\Users\nexus\source\repos\labb2_objektorienterad_programmering\labb2_objektorienterad_programmering\Level1.txt");
            game.Run();
        }
    }
}

