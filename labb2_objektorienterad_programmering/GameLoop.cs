//Vision range:

//För att få en effekt av “utforskande” i spelet begränsar vi spelarens synfält till att bara visa objekt inom en radie av 5 tecken (men ni kan också prova med andra radier); Väggarna försvinner dock aldrig när man väl sett dem, men fienderna syns inte så fort de kommer utanför radien.

//Avståndet mellan två punkter i 2D kan enkelt beräknas med hjälp av pythagoras sats.

//Hålla koll på tillståndet: Hålla reda på instanser av LevelData, Player och spelets status (t.ex. om spelet är igång eller om spelaren är död).

//Game Loop: Köra loopen som väntar på tangentbordstryck (Console.ReadKey()), flyttar spelaren, anropar .Update() på alla fiender i LevelData, och kollar kollisioner/attacker.

// 3. Uppdatera alla fienders rörelser

namespace labb2
{
    public class GameLoop
    {
        private LevelData levelData;
        private bool isRunning;

        public GameLoop(string filePath)
        {
            levelData = new LevelData(filePath);
            isRunning = true;
        }

        public void Run()
        {
            if (levelData.Player == null)
            {
                Console.WriteLine("Kunde inte hitta spelaren ('@') i kartfilen!");
                Console.ReadKey();
                return;
            }

            Console.CursorVisible = false;

            while (isRunning)
            {
                Console.Clear();

                // 1. Rita ut element med synfältslogik (radie 5)
                foreach (var element in levelData.Elements)
                {
                    double distance = Math.Sqrt(Math.Pow(levelData.Player.X - element.X, 2) + Math.Pow(levelData.Player.Y - element.Y, 2));

                    if (distance <= 5.0)
                    {
                        element.IsDiscovered = true;

                        if (element is Enemy || element is Wall)
                        {
                            element.Draw();
                        }
                    }
                    else
                    {
                        if (element is Wall && element.IsDiscovered)
                        {
                            element.Draw();
                        }
                    }
                }

                levelData.Player.Draw();

                // Spelarstatus högst upp
                Console.SetCursorPosition(0, 0);
                Console.Write($"Spelare HP: {levelData.Player.HP}   ");

                // 2. Lyssna på tangentbordet för spelarens drag
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                        break;
                    }

                    int newX, newY;
                    levelData.Player.HandleInput(keyInfo, out newX, out newY);

                    LevelElement obstacle = levelData.Elements.FirstOrDefault(e => e.X == newX && e.Y == newY);

                    if (obstacle == null)
                    {
                        levelData.Player.X = newX;
                        levelData.Player.Y = newY;
                    }
                    else if (obstacle is Wall)
                    {
                        // Vägg - gör inget
                    }
                    else if (obstacle is Enemy enemy)
                    {
                        // --- STRIDSLOGG: Spelare attackerar fiende ---
                        int playerAttack = levelData.Player.AttackDice.Throw();
                        int enemyDefence = enemy.DefenceDice.Throw();
                        int playerDamage = playerAttack - enemyDefence;

                        Console.SetCursorPosition(0, 22);
                        Console.WriteLine($"Du attackerar {enemy.Name}! Attack: {playerAttack} ({levelData.Player.AttackDice}) vs Försvar: {enemyDefence} ({enemy.DefenceDice})");
                        Console.WriteLine($"Skada gjord: {Math.Max(0, playerDamage)}");

                        if (playerDamage > 0)
                        {
                            enemy.HP -= playerDamage;
                        }

                        if (enemy.HP > 0)
                        {
                            int enemyAttack = enemy.AttackDice.Throw();
                            int playerDefence = levelData.Player.DefenceDice.Throw();
                            int enemyDamage = enemyAttack - playerDefence;

                            Console.WriteLine($"{enemy.Name} gör motattack! Attack: {enemyAttack} ({enemy.AttackDice}) vs Försvar: {playerDefence} ({levelData.Player.DefenceDice})");
                            Console.WriteLine($"Skada på dig: {Math.Max(0, enemyDamage)}");

                            if (enemyDamage > 0)
                            {
                                levelData.Player.HP -= enemyDamage;
                                if (levelData.Player.HP <= 0)
                                {
                                    isRunning = false;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{enemy.Name} besegrades!");
                            levelData.Elements.Remove(enemy);
                            levelData.Player.X = newX;
                            levelData.Player.Y = newY;
                        }
                        System.Threading.Thread.Sleep(1200); // Paus så man hinner läsa stridsloggen
                    }

                    // 3. Uppdatera alla fiender
                    foreach (var element in levelData.Elements.ToList())
                    {
                        if (element is Enemy enemyElement)
                        {
                            int oldX = enemyElement.X;
                            int oldY = enemyElement.Y;

                            enemyElement.Update(levelData.Player, levelData);

                            LevelElement hitWall = levelData.Elements.FirstOrDefault(e => e is Wall && e.X == enemyElement.X && e.Y == enemyElement.Y);

                            if (hitWall != null)
                            {
                                enemyElement.X = oldX;
                                enemyElement.Y = oldY;
                            }
                            else
                            {
                                if (enemyElement.X == levelData.Player.X && enemyElement.Y == levelData.Player.Y)
                                {
                                    // --- STRIDSLOGG: Fiende går in i spelaren ---
                                    int enemyAttack = enemyElement.AttackDice.Throw();
                                    int playerDefence = levelData.Player.DefenceDice.Throw();
                                    int enemyDamage = enemyAttack - playerDefence;

                                    Console.SetCursorPosition(0, 22);
                                    Console.WriteLine($"{enemyElement.Name} attackerar dig! Attack: {enemyAttack} vs Försvar: {playerDefence} -> Skada: {Math.Max(0, enemyDamage)}");

                                    if (enemyDamage > 0)
                                    {
                                        levelData.Player.HP -= enemyDamage;
                                        if (levelData.Player.HP <= 0)
                                        {
                                            isRunning = false;
                                            break;
                                        }
                                    }

                                    int playerAttack = levelData.Player.AttackDice.Throw();
                                    int enemyDefence = enemyElement.DefenceDice.Throw();
                                    int playerDamage = playerAttack - enemyDefence;

                                    Console.WriteLine($"Du gör motattack mot {enemyElement.Name}! Attack: {playerAttack} vs Försvar: {enemyDefence} -> Skada: {Math.Max(0, playerDamage)}");

                                    if (playerDamage > 0)
                                    {
                                        enemyElement.HP -= playerDamage;
                                    }

                                    if (enemyElement.HP <= 0)
                                    {
                                        Console.WriteLine($"{enemyElement.Name} besegrades!");
                                        levelData.Elements.Remove(enemyElement);
                                    }
                                    else
                                    {
                                        enemyElement.X = oldX;
                                        enemyElement.Y = oldY;
                                    }
                                    System.Threading.Thread.Sleep(1200);
                                }
                            }
                        }
                    }
                }

                System.Threading.Thread.Sleep(50);
            }

            Console.Clear();
            Console.WriteLine("Spelet är slut!");
            Console.ReadKey();
        }
    }
}