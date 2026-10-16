//Vision range:

//För att få en effekt av “utforskande” i spelet begränsar vi spelarens synfält till att bara visa objekt inom en radie av 5 tecken (men ni kan också prova med andra radier); Väggarna försvinner dock aldrig när man väl sett dem, men fienderna syns inte så fort de kommer utanför radien.

//Avståndet mellan två punkter i 2D kan enkelt beräknas med hjälp av pythagoras sats.

//Hålla koll på tillståndet: Hålla reda på instanser av LevelData, Player och spelets status (t.ex. om spelet är igång eller om spelaren är död).

//Game Loop: Köra loopen som väntar på tangentbordstryck (Console.ReadKey()), flyttar spelaren, anropar .Update() på alla fiender i LevelData, och kollar kollisioner/attacker.

// 3. Uppdatera alla fienders rörelser
using System;
using System.Linq;

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

            // Dölj den blinkande markören för att slippa flimmer
            Console.CursorVisible = false;

            while (isRunning)
            {
                Console.Clear();

                // 1. Rita ut element med synfälts- och minneslogik (radie 5)
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
                            element.Draw(); // Väggar som har synts tidigare stannar kvar
                        }
                    }
                }

                // Rita alltid ut spelaren
                levelData.Player.Draw();

                // Skriv ut spelarstatus högst upp
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

                    // Kolla om spelaren krockar med något på den nya rutan
                    LevelElement obstacle = levelData.Elements.FirstOrDefault(e => e.X == newX && e.Y == newY);

                    if (obstacle == null)
                    {
                        // Tom ruta - flytta spelaren
                        levelData.Player.X = newX;
                        levelData.Player.Y = newY;
                    }
                    else if (obstacle is Wall)
                    {
                        // Vägg - stoppas, gör inget
                    }
                    else if (obstacle is Enemy enemy)
                    {
                        // Spelaren går på fiende och attackerar först
                        int playerAttack = levelData.Player.AttackDice.Throw();
                        int enemyDefence = enemy.DefenceDice.Throw();
                        int playerDamage = playerAttack - enemyDefence;

                        if (playerDamage > 0)
                        {
                            enemy.HP -= playerDamage;
                        }

                        if (enemy.HP > 0)
                        {
                            // Fienden lever och gör motattack
                            int enemyAttack = enemy.AttackDice.Throw();
                            int playerDefence = levelData.Player.DefenceDice.Throw();
                            int enemyDamage = enemyAttack - playerDefence;

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
                            // Fienden dör - ta bort den och flytta fram spelaren
                            levelData.Elements.Remove(enemy);
                            levelData.Player.X = newX;
                            levelData.Player.Y = newY;
                        }
                    }

                    // 3. Uppdatera alla fiender varje omgång
                    foreach (var element in levelData.Elements.ToList())
                    {
                        if (element is Enemy enemyElement)
                        {
                            int oldX = enemyElement.X;
                            int oldY = enemyElement.Y;

                            // Fiendens rörelse (t.ex. råttans slumpade steg)
                            enemyElement.Update();

                            // Kolla om fienden krockar med en vägg
                            LevelElement hitWall = levelData.Elements.FirstOrDefault(e => e is Wall && e.X == enemyElement.X && e.Y == enemyElement.Y);

                            if (hitWall != null)
                            {
                                // Går inte att gå in i vägg, flytta tillbaka till gamla positionen
                                enemyElement.X = oldX;
                                enemyElement.Y = oldY;
                            }
                            else
                            {
                                // Kolla om fienden gick in i spelaren
                                if (enemyElement.X == levelData.Player.X && enemyElement.Y == levelData.Player.Y)
                                {
                                    // Fienden attackerar först!
                                    int enemyAttack = enemyElement.AttackDice.Throw();
                                    int playerDefence = levelData.Player.DefenceDice.Throw();
                                    int enemyDamage = enemyAttack - playerDefence;

                                    if (enemyDamage > 0)
                                    {
                                        levelData.Player.HP -= enemyDamage;
                                        if (levelData.Player.HP <= 0)
                                        {
                                            isRunning = false;
                                            break;
                                        }
                                    }

                                    // Spelarens motattack om spelaren överlever
                                    int playerAttack = levelData.Player.AttackDice.Throw();
                                    int enemyDefence = enemyElement.DefenceDice.Throw();
                                    int playerDamage = playerAttack - enemyDefence;

                                    if (playerDamage > 0)
                                    {
                                        enemyElement.HP -= playerDamage;
                                    }

                                    if (enemyElement.HP <= 0)
                                    {
                                        levelData.Elements.Remove(enemyElement);
                                    }
                                    else
                                    {
                                        // Om fienden överlever stannar den kvar på gamla positionen
                                        enemyElement.X = oldX;
                                        enemyElement.Y = oldY;
                                    }
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
}sd