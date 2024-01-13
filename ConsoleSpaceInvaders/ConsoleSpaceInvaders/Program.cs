using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSpaceInvaders
{
    internal class Program
    {
        public static int enemySpacingX = 2;
        public static int enemySpacingY = 1;
        public static int enemyColumsNum = 11;//the number of 'columns' of enemies

        public static int width = enemyColumsNum*(11+ enemySpacingX) +8;
        public static int height = 63;
        public static List<Entity> projectiles = new List<Entity>();
        public static List<LivingEntity> enemies = new List<LivingEntity>();
        public static LivingEntity player;
        private static List<StringBuilder> lastScreen = new List<StringBuilder>();
        public static List<StringBuilder> currentScreen = new List<StringBuilder>();
        public static bool gameOn = false;
        public static int projectileDelay = 0;
        public static bool redrawing = false;
        public static int score = 0;

        
        static void Main(string[] args)
        {            
            Console.SetWindowSize(width+2, height);

            gameOn = true;
            bool enemiesMovedDownLastUpdate = false;
            int enemyDirection = 1;
            prepareGameScreen();
            clearEntireScreen();
            spawnEntities();
            long count = -1;

            Thread thread = new Thread(new ThreadStart(listenForKeys));
            thread.Start();

            while (true)
            {
                count++;

                bool redraw = false;
                if (count % 1 == 0 && projectiles.Count > 0)
                {
                    foreach (Entity projectile in projectiles) projectile.removeFromScreen(currentScreen);
                    moveProjectiles();
                    redraw = true;
                }
                if (count%10==0)
                {
                    foreach (LivingEntity entity in enemies) entity.removeFromScreen(currentScreen);
                    int edgeLeft = 1;
                    int edgeRight = currentScreen[0].Length - 1;
                    int leftmostEnemyHitbox = enemies[0].posX;
                    int rightmostEnemyHitbox = enemies[enemies.Count - 1].posX + 11;
                    if ((leftmostEnemyHitbox <= edgeLeft || rightmostEnemyHitbox >= edgeRight) && !enemiesMovedDownLastUpdate)
                    {
                        moveEnemies(0, 1);
                        enemiesMovedDownLastUpdate = true;
                        enemyDirection *= -1;
                    }
                    else
                    {
                        moveEnemies(enemyDirection, 0);
                        enemiesMovedDownLastUpdate = false;
                    }
                    redraw = true;
                }
                if (redraw)
                {
                    reDrawScreen();
                }
                Thread.Sleep(50);
            }

        }
        static void listenForKeys()
        {
            Thread.Sleep(20);
            player.writeToScreen(currentScreen);
            reDrawPlayer(player);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    int moveByX = 0;
                    int moveByY = 0;
                    if (key.Equals(ConsoleKey.LeftArrow) && player.posX > 0) moveByX = -1;
                    if (key.Equals(ConsoleKey.RightArrow) && player.posX + 15 < width) moveByX = 1;
                    if (moveByX != 0 || moveByY != 0)
                    {
                        player.removeFromScreen(currentScreen);
                        player.moveBy(moveByX, moveByY);
                        player.writeToScreen(currentScreen);
                        reDrawPlayer(player);
                    }
                    //Projectiles
                    if (key.Equals(ConsoleKey.UpArrow))
                    {
                        if (projectiles.Find(p => p.posX == (player.posX + 6) && p.posY == (player.posY - 1)) == null)
                        {
                            Entity projectile = player.createProjectile(currentScreen, "↑", new int[] { player.posX + 6, player.posY - 1 });
                            projectiles.Add(projectile);
                        }

                    }
                    if (key.Equals(ConsoleKey.Spacebar))
                    {
                        for (int i = -5; i < 6; i++)
                        {
                            if (projectiles.Find(p => p.posX == (player.posX + 6 + i) && p.posY == (player.posY - 1)) == null)
                            {
                                Entity projectile = player.createProjectile(currentScreen, "↑", new int[] { player.posX + 6 + i, player.posY - 1 });
                                projectiles.Add(projectile);
                            }
                        }

                    }
                }
                Thread.Sleep(5);
            }
        }
        static void spawnUfo()
        {
            Entity ufo = new Entity("ufo");
            ufo.addModel("    ▄▄████▄▄    \n  ▄██████████▄  \n▄██▄██▄██▄██▄██▄\n  ▀█▀  ▀▀  ▀█▀  ");
            ufo.posX = 5;
            ufo.posY = 3;
        }
        static void spawnEntities()
        {
            player = new LivingEntity(3);
            player.setPos(width/2 - 7, height - 4-2);
            player.addModel("      ▄\n     ███\n▄███████████▄\n█████████████\n█████████████");

            for (int x = 0; x < 11; x++)
            {
                int posX = 4 + x * (11+ enemySpacingX);
                LivingEntity enemy1 = new LivingEntity(1);
                enemy1.addModel("   ▄███▄\n ▄███████▄\n███▄███▄███\n  ▄▀▄▄▄▀▄\n ▀ ▀   ▀ ▀");
                enemy1.addModel("   ▄███▄\n ▄███████▄\n███▄███▄███\n ▄▀ ▀▀▀ ▀▄\n  ▀     ▀");
                enemy1.setPos(posX, 7);

                LivingEntity enemy2 = new LivingEntity(1);
                enemy2.addModel("  ▀▄   ▄▀\n ▄█▀███▀█▄\n█▀███████▀█\n█ █▀▀▀▀▀█ █\n   ▀▀ ▀▀");
                enemy2.addModel("▄ ▀▄   ▄▀ ▄\n█▄███████▄█\n███▄███▄███\n▀█████████▀\n ▄▀     ▀▄");
                enemy2.setPos(posX, enemy1.posY + enemy1.getSizeY() + enemySpacingY);

                LivingEntity enemy3 = new LivingEntity(1);
                enemy3.addModel("▄ ▀▄   ▄▀ ▄\n█▄███████▄█\n███▄███▄███\n▀█████████▀\n ▄▀     ▀▄");
                enemy3.addModel("  ▀▄   ▄▀\n ▄█▀███▀█▄\n█▀███████▀█\n█ █▀▀▀▀▀█ █\n   ▀▀ ▀▀");
                enemy3.setPos(posX, enemy2.posY + enemy2.getSizeY() + enemySpacingY);

                LivingEntity enemy4 = new LivingEntity(1);
                enemy4.addModel(" ▄▄█████▄▄\n███████████\n██▄▄███▄▄██\n ▄▀▄▀▀▀▄▀▄\n▀         ▀");
                enemy4.addModel(" ▄▄█████▄▄ \n███████████\n██▄▄███▄▄██\n ▄▀▀▄▄▄▀▀▄ \n  ▀     ▀");
                enemy4.setPos(posX, enemy3.posY + enemy3.getSizeY() + enemySpacingY);

                LivingEntity enemy5 = new LivingEntity(1);
                enemy5.addModel(" ▄▄█████▄▄\n███████████\n██▄▄███▄▄██\n ▄▀▄▀▀▀▄▀▄\n▀         ▀");
                enemy5.addModel(" ▄▄█████▄▄ \n███████████\n██▄▄███▄▄██\n ▄▀▀▄▄▄▀▀▄ \n  ▀     ▀");
                enemy5.setPos(posX, enemy4.posY + enemy4.getSizeY() + enemySpacingY);

                enemies.Add(enemy1);
                enemies.Add(enemy2);
                enemies.Add(enemy3);
                enemies.Add(enemy4);
                enemies.Add(enemy5);
            }
        }
        static void moveEnemies(int moveX, int moveY)
        { 
            foreach (LivingEntity enemy in enemies)
            {
                enemy.moveBy(moveX, moveY);
                enemy.writeToScreen(currentScreen);

            }
        }
        static void moveProjectiles()
        {
            List<Entity> newProjectiles = new List<Entity>();
            newProjectiles.AddRange(projectiles);

            foreach (Entity projectile in projectiles)
            {
                int moveDir = 1;
                if (projectile.models[0].Equals("↑"))
                {
                    moveDir = -1;
                }
                int moveToX = projectile.posX;
                int moveToY = projectile.posY+moveDir;
                if (moveToX < 0 || moveToY < 0)
                {
                    newProjectiles.Remove(projectile);
                    continue;
                }
                bool collision = (!currentScreen[moveToY][moveToX].Equals(' ') && !currentScreen[moveToY][moveToX].Equals('↑'));
                projectile.moveBy(0, moveDir);
                projectile.writeToScreen(currentScreen);

                if (!collision) continue; //else collision with entity
                newProjectiles.Remove(projectile);
                LivingEntity colidedEnemy = findEnemyHitboxAt(moveToX, moveToY);
                if (colidedEnemy == null)
                {
                    continue;
                }
                colidedEnemy.health--;
                if (colidedEnemy.health <= 0)
                {
                    enemies.Remove(colidedEnemy);
                    colidedEnemy.removeFromScreen(currentScreen);
                }
            }
            projectiles = newProjectiles;
        }
        static LivingEntity findEnemyHitboxAt(int posX, int posY)
        {
            foreach (LivingEntity enemy in enemies)
            {
                int enemyPosX = enemy.posX;//100
                int enemyPosY = enemy.posY;//200
                int relativeX = posX - enemyPosX;
                int relativeY = posY - enemyPosY;
                if (relativeX < 0 || relativeY < 0) continue;
                string currentModel = enemy.getModel(false);
                List<String> currentModelList = new List<String>();
                if (currentModel.Contains("\n")) currentModelList.AddRange(currentModel.Split(new string[] { "\n" }, StringSplitOptions.None));
                else currentModelList.Add(currentModel);
                if (relativeY > currentModelList.Count) continue;
                if (relativeX > currentModelList[relativeY].ToCharArray().Length) continue;
                if (currentModelList[relativeY].ToCharArray()[relativeX] != ' ')
                {
                    return enemy;
                }
            }
            return null;
        }
        static void prepareGameScreen()
        {
            Console.Clear();
            StringBuilder top = new StringBuilder(new string('▄', width));
            StringBuilder middle = new StringBuilder("█" + new string(' ', width - 2) + "█");
            StringBuilder bottom = new StringBuilder(new string('▀', width));

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(top);
            for (int x = 1; x < height - 1; x++) Console.WriteLine(middle);
            Console.WriteLine(bottom);
            Console.ForegroundColor = ConsoleColor.White;

        }
        static void clearEntireScreen()
        {
            currentScreen.Clear();
            for (int i = 0; i < height-2; i++) currentScreen.Add(new StringBuilder(new string(' ', width - 2)));
            currentScreen[0] = new StringBuilder("X" + new string(' ', width -4)+"X");
            currentScreen[height-3] = new StringBuilder("X" + new string(' ', width -4) + "X");
        }
        static void reDrawScreen()
        {
            redrawing = true;

            int lineNum = -1;
            foreach (StringBuilder line in currentScreen)
            {
                lineNum++;
                if (lastScreen.Count == currentScreen.Count)
                {
                    if (line == lastScreen[lineNum]) continue;
                }

                Console.SetCursorPosition(1, lineNum+1);
                Console.Write(line);
            }
            lastScreen.Clear();
            //lastScreen.AddRange(currentScreen);
            currentScreen.ForEach(delegate (StringBuilder str) {
                lastScreen.Add(new StringBuilder(str.ToString()));
            });
            redrawing = false;
        }
        static void reDrawPlayer(LivingEntity player)
        {
            int posX = player.posX-2;
            int posY = player.posY;
            int playerWidth = player.getSizeX()+4;
            int playerHeight = player.getSizeY();

            if (posX < 0)
            {
                playerWidth += posX;
                posX = 0;
            }
            if (posX+playerWidth > width-2) 
            {
                playerWidth = width - posX-2;
            }

            //Console.ForegroundColor = ConsoleColor.DarkGreen;
            for (int y = posY; y < (posY+playerHeight-1);y++)
            {
                string line = currentScreen[y].ToString();
                if (!redrawing)
                {
                    Console.SetCursorPosition(posX+1, y+1);
                    Console.Write(line.Substring(posX, playerWidth));
                }
            }
            //Console.ResetColor();
        }

    }
}
