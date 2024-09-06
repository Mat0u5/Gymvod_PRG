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
        public static readonly int enemySpacingX = 2;
        public static readonly int enemySpacingY = 1;
        public static readonly int enemyColumsNum = 11;//the number of 'columns' of enemies
        public static readonly int projectileDelay = 10;//for the actual delay, multiply by 50ms
        public static readonly int width = enemyColumsNum*(11+ enemySpacingX) +8;
        public static readonly int height = 63;

        public static List<Entity> projectiles = new List<Entity>();
        public static List<LivingEntity> enemies = new List<LivingEntity>();
        public static LivingEntity player;
        public static LivingEntity ufo;
        public static List<StringBuilder> lastScreen = new List<StringBuilder>();
        public static List<StringBuilder> currentScreen = new List<StringBuilder>();
        public static bool gameOn = false;
        public static bool redrawing = false;
        public static bool redrawingTwo = false;
        public static int score = 0;
        public static long gameLoopCount = -1;
        public static bool enemiesMovedDownLastUpdate = false;
        public static int enemyDirection = 1;
        public static int currentProjectileDelay = 0;

        public static Screen screen = new Screen();

        static void Main(string[] args)
        {            
            Console.SetWindowSize(width+2, height);
            startGame();

            Thread.Sleep(10000);
            Console.ReadKey();
        }
        static void startGame()
        {
            screen.startScreen();

            Char key = Console.ReadKey().KeyChar;

            Thread thread = new Thread(new ThreadStart(listenForKeysLoop));
            thread.Start();

            mainGameLoop();
        }
        static void gameOver()
        {

            screen.reWriteLivesAndScore();
            gameOn = false;
            List<LivingEntity> entities = new List<LivingEntity>();
            entities.AddRange(enemies);
            if (ufo != null) entities.Add(ufo);
            entities.Add(player);
            foreach (LivingEntity entity in entities)
            {
                entity.removeFromScreen(currentScreen);
            }
            ufo = null;
            score = 0;
            projectiles = new List<Entity>();
            enemies = new List<LivingEntity>();
            gameLoopCount = -1;
            enemiesMovedDownLastUpdate = false;
            enemyDirection = 1;
            currentProjectileDelay = 0;


            Thread.Sleep(100);
            redrawing = false;
            redrawingTwo = false;
            player.setModel(" ▄  ▀   ▄   ▄\n   ▀  ▄  ▀█  \n▄██ █  ▄ █▀█▄\n█████ ███████");
            player.writeToScreen(currentScreen);
            screen.reDrawPlayer();
            lastScreen = new List<StringBuilder>();
            currentScreen = new List<StringBuilder>();
            Thread.Sleep(500);

            screen.gameOverScreen();
            player = null;
            Char key = Console.ReadKey().KeyChar;
            startGame();
        }
        static void mainGameLoop()
        {
            gameOn = true;
            screen.prepareGameBorder();
            screen.clearEntireScreen();
            spawnEntities();
            while (gameOn)
            {
                gameLoopCount++;
                if (currentProjectileDelay > 0) currentProjectileDelay--;
                screen.reWriteLivesAndScore();
                bool redraw = false;
                if (gameLoopCount % 1 == 0 && projectiles.Count > 0)//projectiles move every 50ms
                {
                    bool redrawProjectiles = projectilesLoop();
                    if (!redraw && redrawProjectiles) redraw = true;
                }
                if (gameLoopCount % 10 == 0)//enemies move every 500 ms
                {
                    bool redrawEnemies = enemiesLoop();
                    if (!redraw && redrawEnemies) redraw = true;
                }
                if (gameLoopCount % 1 == 0)//ufo moves every 50ms
                {
                    bool redrawUFO = UFOloop();
                    if (!redraw && redrawUFO) redraw = true;
                }
                if (redraw)
                {
                    screen.reDrawScreen();
                }
                Thread.Sleep(50);
            }
        }
        static void listenForKeysLoop()//player movement, shooting etc.
        {
            Thread.Sleep(200);
            player.writeToScreen(currentScreen);
            screen.reDrawPlayer();
            while (gameOn)
            {
                Thread.Sleep(20);
                if (!Console.KeyAvailable) continue;
                ConsoleKey key = Console.ReadKey().Key;
                int moveByX = 0;
                int moveByY = 0;
                if ((key.Equals(ConsoleKey.LeftArrow) || key.Equals(ConsoleKey.A)) && player.posX > 0) moveByX = -1;//move left
                if ((key.Equals(ConsoleKey.RightArrow) || key.Equals(ConsoleKey.D)) && player.posX + 15 < width) moveByX = 1;//move right

                if (moveByX != 0 || moveByY != 0)
                {
                    player.removeFromScreen(currentScreen);
                    player.moveBy(moveByX, moveByY);
                    player.writeToScreen(currentScreen);
                    screen.reDrawPlayer();
                }


                //Projectiles
                if ((key.Equals(ConsoleKey.UpArrow) || key.Equals(ConsoleKey.Spacebar) || key.Equals(ConsoleKey.W)) && currentProjectileDelay == 0)//shoot
                {
                    new Beep().BeepBeep(40, 4000, 25);
                    Thread.Sleep(10);
                    new Beep().BeepBeep(40, 3500, 25);
                    currentProjectileDelay = 10;
                    if (projectiles.Find(p => p.posX == (player.posX + 6) && p.posY == (player.posY - 1)) == null)
                    {
                        Entity projectile = player.createProjectile(currentScreen, "↑", new int[] { player.posX + 6, player.posY - 1 });
                        projectiles.Add(projectile);
                    }

                }
                if (key.Equals(ConsoleKey.L) && currentProjectileDelay == 0)//Shoots 10 bullets - for developer testing - I left it functional, because why not.
                {
                    new Beep().BeepBeep(40, 4000, 25);
                    Thread.Sleep(10);
                    new Beep().BeepBeep(40, 3500, 25);
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
        }
        
        //Entity manipulation stuff (movement, collisions,...) - I didn't want to create a separate class for this, because I wanted it to be in the Program class for ease of access.
        //                                                       And it also interacts with every other class, it'd be a mess if I put it into its own class.
        static bool UFOloop()//returns true if Main should redraw the screen
        {
            Random rnd = new Random();
            bool redraw = false;
            if (ufo == null && rnd.Next(1000) == 0) //spawn ufo
            {
                int ufoDirection = (rnd.Next(2) == 0) ? -1 : 1;
                spawnUfo(ufoDirection);
            }

            if (ufo != null)//UFO LOGIC
            {
                ufo.removeFromScreen(currentScreen);
                if (ufo.posX < -16 || ufo.posX > width + 16)//kill ufo
                {
                    ufo.removeFromScreen(currentScreen);
                    ufo = null;
                }
                else
                {
                    ufo.moveBy(ufo.extraInfo, 0);
                    ufo.writeToScreen(currentScreen);
                    redraw = true;
                }
            }
            return redraw;
        }
        static bool projectilesLoop()//returns true if Main should redraw the screen
        {
            foreach (Entity projectile in projectiles.ToArray()) projectile.removeFromScreen(currentScreen);
            moveProjectiles();
            return true;
        }
        static bool enemiesLoop()//returns true if Main should redraw the screen
        {
            Random rnd = new Random();

            if (enemies.Count == 0)//respawn enemies
            {
                spawnEntities();
            }
            foreach (LivingEntity entity in enemies)
            {
                entity.removeFromScreen(currentScreen);
                if (rnd.Next(50)==0)//enemy shoot projectile
                {
                    Entity projectile = entity.createProjectile(currentScreen, "↓", new int[] { entity.posX + 5, entity.posY + 4 });
                    projectiles.Add(projectile);
                }
            }
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
            return true;
        }
        static void spawnUfo(int ufoDirection)
        {
            ufo = new LivingEntity(1, 100);
            ufo.extraInfo = ufoDirection;
            ufo.addModel("    ▄▄████▄▄    \n  ▄██████████▄  \n▄██▄██▄██▄██▄██▄\n  ▀█▀  ▀▀  ▀█▀  ");
            if (ufoDirection == 1) ufo.setPos(-15, 1);
            if (ufoDirection == -1) ufo.setPos(width+15, 1);
        }
        static void spawnEntities()
        {
            if (player == null)
            {
                player = new LivingEntity(3);
                player.setPos(width / 2 - 7, height - 4 - 2);
                player.addModel("      ▄\n     ███\n▄███████████▄\n█████████████\n█████████████");
            }

            for (int x = 0; x < 11; x++)
            {
                int posX = 4 + x * (11+ enemySpacingX);
                LivingEntity enemy1 = new LivingEntity(1, 40);
                enemy1.addModel("   ▄███▄\n ▄███████▄\n███▄███▄███\n  ▄▀▄▄▄▀▄\n ▀ ▀   ▀ ▀");
                enemy1.addModel("   ▄███▄\n ▄███████▄\n███▄███▄███\n ▄▀ ▀▀▀ ▀▄\n  ▀     ▀");
                enemy1.setPos(posX, 7);

                LivingEntity enemy2 = new LivingEntity(1, 20);
                enemy2.addModel("  ▀▄   ▄▀\n ▄█▀███▀█▄\n█▀███████▀█\n█ █▀▀▀▀▀█ █\n   ▀▀ ▀▀");
                enemy2.addModel("▄ ▀▄   ▄▀ ▄\n█▄███████▄█\n███▄███▄███\n▀█████████▀\n ▄▀     ▀▄");
                enemy2.setPos(posX, enemy1.posY + enemy1.getSizeY() + enemySpacingY);

                LivingEntity enemy3 = new LivingEntity(1, 20);
                enemy3.addModel("▄ ▀▄   ▄▀ ▄\n█▄███████▄█\n███▄███▄███\n▀█████████▀\n ▄▀     ▀▄");
                enemy3.addModel("  ▀▄   ▄▀\n ▄█▀███▀█▄\n█▀███████▀█\n█ █▀▀▀▀▀█ █\n   ▀▀ ▀▀");
                enemy3.setPos(posX, enemy2.posY + enemy2.getSizeY() + enemySpacingY);

                LivingEntity enemy4 = new LivingEntity(1, 10);
                enemy4.addModel(" ▄▄█████▄▄\n███████████\n██▄▄███▄▄██\n ▄▀▄▀▀▀▄▀▄\n▀         ▀");
                enemy4.addModel(" ▄▄█████▄▄ \n███████████\n██▄▄███▄▄██\n ▄▀▀▄▄▄▀▀▄ \n  ▀     ▀");
                enemy4.setPos(posX, enemy3.posY + enemy3.getSizeY() + enemySpacingY);

                LivingEntity enemy5 = new LivingEntity(1, 10);
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
                if (!gameOn) break;
                enemy.moveBy(moveX, moveY);
                enemy.writeToScreen(currentScreen);

            }
        }
        static void moveProjectiles()
        {
            List<Entity> newProjectiles = new List<Entity>();
            newProjectiles.AddRange(projectiles);

            foreach (Entity projectile in projectiles.ToArray())
            {
                if (!gameOn) break;
                int moveDir = 1;
                if (projectile.models[0].Equals("↑"))
                {
                    moveDir = -1;
                }
                int moveToX = projectile.posX;
                int moveToY = projectile.posY+moveDir;
                if (moveToX < 0 || moveToY < 0 || moveToX > width -2|| moveToY > height-3)
                {
                    newProjectiles.Remove(projectile);
                    continue;
                }
                bool collision = (!currentScreen[moveToY][moveToX].Equals(' ') && !currentScreen[moveToY][moveToX].Equals('↑'));
                projectile.moveBy(0, moveDir);
                projectile.writeToScreen(currentScreen);
                if (!collision) continue; //else collision with entity
                LivingEntity colidedEntity = findEntityWithHitboxAt(moveToX, moveToY);
                if (colidedEntity == null) continue;
                if (enemies.Contains(colidedEntity) && !projectile.models[0].Equals("↑")) continue;//to prevent enemies hitting their own
                handleCollision(projectile, colidedEntity, newProjectiles);
            }
            projectiles = newProjectiles;
        }
        static void handleCollision(Entity projectile, LivingEntity colidedEntity, List<Entity> newProjectiles)
        {
            newProjectiles.Remove(projectile);
            colidedEntity.health--;
            if (colidedEntity == player)
            {
                new Beep().BeepBeep(40, 150, 500);
                Thread.Sleep(10);
                new Beep().BeepBeep(40, 100, 500);
            }
            if (colidedEntity.health <= 0)
            {
                score += colidedEntity.score;
                if (colidedEntity == player)
                {
                    Thread.Sleep(10);
                    new Beep().BeepBeep(40, 75, 500);
                    gameOver();
                }
                else if (colidedEntity == ufo)
                {

                    ufo.removeFromScreen(currentScreen);
                    ufo = null;
                }
                else
                {
                    enemies.Remove(colidedEntity);
                    colidedEntity.removeFromScreen(currentScreen);
                }
            }
        }
        static LivingEntity findEntityWithHitboxAt(int posX, int posY)
        {
            List<LivingEntity> entities = new List<LivingEntity>();
            entities.AddRange(enemies);
            if (ufo != null) entities.Add(ufo);
            entities.Add(player);
            foreach (LivingEntity entity in entities)
            {
                int enemyPosX = entity.posX;
                int enemyPosY = entity.posY;
                int relativeX = posX - enemyPosX;
                int relativeY = posY - enemyPosY;
                if (relativeX < 0 || relativeY < 0) continue;
                string currentModel = entity.getModel(false);
                List<String> currentModelList = new List<String>();
                if (currentModel.Contains("\n")) currentModelList.AddRange(currentModel.Split(new string[] { "\n" }, StringSplitOptions.None));
                else currentModelList.Add(currentModel);
                if (relativeY >= currentModelList.Count) continue;
                if (relativeX >= currentModelList[relativeY].ToCharArray().Length) continue;
                if (currentModelList[relativeY].ToCharArray()[relativeX] != ' ')
                {
                    return entity;
                }
            }
            return null;
        }

    }

}
