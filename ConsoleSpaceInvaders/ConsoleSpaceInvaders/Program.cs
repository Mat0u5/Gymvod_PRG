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
        private static List<StringBuilder> lastScreen = new List<StringBuilder>();
        public static List<StringBuilder> currentScreen = new List<StringBuilder>();
        public static bool gameOn = false;
        public static bool redrawing = false;
        public static bool redrawingTwo = false;
        public static int score = 0;
        public static long gameLoopCount = -1;
        public static bool enemiesMovedDownLastUpdate = false;
        public static int enemyDirection = 1;
        public static int currentProjectileDelay = 0;


        static void Main(string[] args)
        {            
            Console.SetWindowSize(width+2, height);
            startGame();

            Thread.Sleep(10000);
            Console.ReadKey();
        }
        static void startGame()
        {
            startScreen();

            Char key = Console.ReadKey().KeyChar;

            Thread thread = new Thread(new ThreadStart(listenForKeysLoop));
            thread.Start();

            mainGameLoop();
        }
        static void gameOver()
        {

            reWriteLivesAndScore();
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
            reDrawPlayer();
            lastScreen = new List<StringBuilder>();
            currentScreen = new List<StringBuilder>();
            Thread.Sleep(500);

            gameOverScreen();
            player = null;
            Char key = Console.ReadKey().KeyChar;
            startGame();
        }
        static void mainGameLoop()
        {
            gameOn = true;
            prepareGameBorder();
            clearEntireScreen();
            spawnEntities();
            while (gameOn)
            {
                gameLoopCount++;
                if (currentProjectileDelay > 0) currentProjectileDelay--;
                reWriteLivesAndScore();
                bool redraw = false;
                if (gameLoopCount % 1 == 0 && projectiles.Count > 0)//projectiles move every 50ms
                {
                    bool redrawLocal = projectilesLoop();
                    if (!redraw) redraw = redrawLocal;
                }
                if (gameLoopCount % 10 == 0)//enemies move every 500 ms
                {
                    bool redrawLocal = enemiesLoop();
                    if (!redraw) redraw = redrawLocal;
                }
                if (gameLoopCount % 1 == 0)
                {
                    bool redrawLocal = UFOloop();
                    if (!redraw) redraw = redrawLocal;
                }
                if (redraw)
                {
                    reDrawScreen();
                }
                Thread.Sleep(50);
            }
        }
        static void listenForKeysLoop()//player movement, shooting etc.
        {
            Thread.Sleep(200);
            player.writeToScreen(currentScreen);
            reDrawPlayer();
            while (gameOn)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey().Key;
                    int moveByX = 0;
                    int moveByY = 0;
                    if ((key.Equals(ConsoleKey.LeftArrow) || key.Equals(ConsoleKey.A)) && player.posX > 0) moveByX = -1;
                    if ((key.Equals(ConsoleKey.RightArrow) || key.Equals(ConsoleKey.D)) && player.posX + 15 < width) moveByX = 1;
                    if (moveByX != 0 || moveByY != 0)
                    {
                        player.removeFromScreen(currentScreen);
                        player.moveBy(moveByX, moveByY);
                        player.writeToScreen(currentScreen);
                        reDrawPlayer();
                    }
                    //Projectiles
                    if ((key.Equals(ConsoleKey.UpArrow) || key.Equals(ConsoleKey.Spacebar) || key.Equals(ConsoleKey.W)) && currentProjectileDelay == 0)
                    {
                        //new Beep().BeepBeep(40, 1000, 50);
                        //new Beep().BeepBeep(40, 2000, 50);
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
                    if (key.Equals(ConsoleKey.L) && currentProjectileDelay == 0)//for developer testing - I left it functional, because why not.
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
        
        //Entity stuff
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
                LivingEntity colidedEnemy = findEntityWithHitboxAt(moveToX, moveToY);
                if (colidedEnemy == null) continue;
                if (enemies.Contains(colidedEnemy) && !projectile.models[0].Equals("↑")) continue;

                newProjectiles.Remove(projectile);
                colidedEnemy.health--;
                if (colidedEnemy == player)
                {
                    new Beep().BeepBeep(40, 150, 500);
                    Thread.Sleep(10);
                    new Beep().BeepBeep(40, 100, 500);
                }
                if (colidedEnemy.health <= 0)
                {
                    score += colidedEnemy.score;
                    if (colidedEnemy == player)
                    {
                        Thread.Sleep(10);
                        new Beep().BeepBeep(40, 75, 500);
                        gameOver();
                    }
                    else if (colidedEnemy == ufo)
                    {

                        ufo.removeFromScreen(currentScreen);
                        ufo = null;
                    }
                    else
                    {
                        enemies.Remove(colidedEnemy);
                        colidedEnemy.removeFromScreen(currentScreen);
                    }
                }
            }
            projectiles = newProjectiles;
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

        //Screen Stuff
        static void gameOverScreen()
        {
            String gameOverScreen = "           _____                         ____                          \n          / ____|                       / __ \\                         \n         | |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __          \n         | | |_ |/ _` | '_ ` _ \\ / _ \\ | |  | \\ \\ / / _ \\ '__|         \n         | |__| | (_| | | | | | |  __/ | |__| |\\ V /  __/ |            \n          \\_____|\\__,_|_| |_| |_|\\___|  \\____/  \\_/ \\___|_|            \n                                                                       \n                                                                       \n┌─┐┬─┐┌─┐┌─┐┌─┐  ┌─┐┌┐┌┬ ┬  ┬┌─┌─┐┬ ┬  ┌┬┐┌─┐  ┌─┐┌─┐┌┐┌┌┬┐┬ ┌┐┌ ┬ ┬┌─┐\n├─┘├┬┘├┤ └─┐└─┐  ├─┤│││└┬┘  ├┴┐├┤ └┬┘   │ │ │  │  │ ││││ │ │ │││ │ │├┤ \n┴  ┴└─└─┘└─┘└─┘  ┴ ┴┘└┘ ┴   ┴ ┴└─┘ ┴    ┴ └─┘  └─┘└─┘┘└┘ ┴ ┴ ┘└┘ └─┘└─┘";
            List<String> gameOverScreenList = new List<String>();
            gameOverScreenList.AddRange(gameOverScreen.Split(new string[] { "\n" }, StringSplitOptions.None));
            transitionIntoScreen(gameOverScreenList);
        }
        static void startScreen()
        {
            allWhiteScreen();
            String startScreen = "________/\\\\\\\\\\\\\\\\\\_______/\\\\\\\\\\_______/\\\\\\\\\\_____/\\\\\\_____/\\\\\\\\\\\\\\\\\\\\\\_________/\\\\\\\\\\_______/\\\\\\______________/\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_        \n _____/\\\\\\////////______/\\\\\\///\\\\\\____\\/\\\\\\\\\\\\___\\/\\\\\\___/\\\\\\/////////\\\\\\_____/\\\\\\///\\\\\\____\\/\\\\\\_____________\\/\\\\\\///////////__       \n  ___/\\\\\\/_____________/\\\\\\/__\\///\\\\\\__\\/\\\\\\/\\\\\\__\\/\\\\\\__\\//\\\\\\______\\///____/\\\\\\/__\\///\\\\\\__\\/\\\\\\_____________\\/\\\\\\_____________      \n   __/\\\\\\______________/\\\\\\______\\//\\\\\\_\\/\\\\\\//\\\\\\_\\/\\\\\\___\\////\\\\\\__________/\\\\\\______\\//\\\\\\_\\/\\\\\\_____________\\/\\\\\\\\\\\\\\\\\\\\\\_____     \n    _\\/\\\\\\_____________\\/\\\\\\_______\\/\\\\\\_\\/\\\\\\\\//\\\\\\\\/\\\\\\______\\////\\\\\\______\\/\\\\\\_______\\/\\\\\\_\\/\\\\\\_____________\\/\\\\\\///////______    \n     _\\//\\\\\\____________\\//\\\\\\______/\\\\\\__\\/\\\\\\_\\//\\\\\\/\\\\\\_________\\////\\\\\\___\\//\\\\\\______/\\\\\\__\\/\\\\\\_____________\\/\\\\\\_____________   \n      __\\///\\\\\\___________\\///\\\\\\__/\\\\\\____\\/\\\\\\__\\//\\\\\\\\\\\\__/\\\\\\______\\//\\\\\\___\\///\\\\\\__/\\\\\\____\\/\\\\\\_____________\\/\\\\\\_____________  \n       ____\\////\\\\\\\\\\\\\\\\\\____\\///\\\\\\\\\\/_____\\/\\\\\\___\\//\\\\\\\\\\_\\///\\\\\\\\\\\\\\\\\\\\\\/______\\///\\\\\\\\\\/_____\\/\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_\\/\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_ \n        _______\\/////////_______\\/////_______\\///_____\\/////____\\///////////__________\\/////_______\\///////////////__\\///////////////__\n                                                                                                                                       \n                                                                                                                                       \n          _______  _______  _______  _______  _______      _________ _                 _______  ______   _______  _______  _______     \n         (  ____ \\(  ____ )(  ___  )(  ____ \\(  ____ \\     \\__   __/( (    /||\\     /|(  ___  )(  __  \\ (  ____ \\(  ____ )(  ____ \\    \n         | (    \\/| (    )|| (   ) || (    \\/| (    \\/        ) (   |  \\  ( || )   ( || (   ) || (  \\  )| (    \\/| (    )|| (    \\/    \n         | (_____ | (____)|| (___) || |      | (__            | |   |   \\ | || |   | || (___) || |   ) || (__    | (____)|| (_____     \n         (_____  )|  _____)|  ___  || |      |  __)           | |   | (\\ \\) |( (   ) )|  ___  || |   | ||  __)   |     __)(_____  )    \n               ) || (      | (   ) || |      | (              | |   | | \\   | \\ \\_/ / | (   ) || |   ) || (      | (\\ (         ) |    \n         /\\____) || )      | )   ( || (____/\\| (____/\\     ___) (___| )  \\  |  \\   /  | )   ( || (__/  )| (____/\\| ) \\ \\__/\\____) |    \n         \\_______)|/       |/     \\|(_______/(_______/     \\_______/|/    )_)   \\_/   |/     \\|(______/ (_______/|/   \\__/\\_______)    \n                                                                                                                                       \n                                                                                                                                       \n                                                                                                                                       \n                                                                                                                                       \n                                                                                                                                       \n┌─┐┬─┐┌─┐┌─┐┌─┐  ┌─┐┌┐┌┬ ┬  ┬┌─┌─┐┬ ┬  ┌┬┐┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌┬┐                                                                         \n├─┘├┬┘├┤ └─┐└─┐  ├─┤│││└┬┘  ├┴┐├┤ └┬┘   │ │ │  └─┐ │ ├─┤├┬┘ │                                                                          \n┴  ┴└─└─┘└─┘└─┘  ┴ ┴┘└┘ ┴   ┴ ┴└─┘ ┴    ┴ └─┘  └─┘ ┴ ┴ ┴┴└─ ┴                                                                          ";
            List<String> startScreenList = new List<String>();
            startScreenList.AddRange(startScreen.Split(new string[] { "\n" }, StringSplitOptions.None));
            transitionIntoScreen(startScreenList);
        }
        static void transitionIntoScreen(List<String> screen)
        {
            int middleX = width / 2;
            int middleY = height / 2;
            char[,] fullScreen = new char[width - 2, height - 2];
            for (int y = 0; y < height - 2; y++)
            {
                for (int x = 0; x < width - 2; x++)
                {
                    fullScreen[x, y] = ' ';
                }
            }
            int startTextX = middleX - screen[0].Length / 2;
            int startTextY = middleY - screen.Count / 2;
            int moveY = 0;
            foreach (String line in screen)
            {
                int moveX = 0;
                foreach (Char c in line)
                {
                    fullScreen[startTextX + moveX, startTextY + moveY] = c;
                    moveX++;
                }
                moveY++;
            }
            bool written = false;
            bool writtenForced = false;
            int maxDist = 0;
            void stopTransition()
            {
                if (!written)
                {
                    Char key = Console.ReadKey().KeyChar;
                    writtenForced = true;
                }
            }
            Thread thread = new Thread(new ThreadStart(stopTransition));
            thread.Start();
            while (!written && !writtenForced)
            {
                /*int fromX = middleX - maxDist;
                int toX = middleX + maxDist;
                int fromY = middleY - maxDist;
                int toY = middleY + maxDist;
                if (fromX < 0) fromX = 0;
                if (fromY < 0) fromY = 0;
                if (toX > width-2) toX = width - 2;
                if (toY > height-2) toY = height-2;*/
                int fromY = 0;
                int fromX = 0;
                int toY = fullScreen.GetLength(1);
                int toX = fullScreen.GetLength(0);

                for (int y = fromY; y < toY; y++)
                {
                    for (int x = fromX; x < toX; x++)
                    {
                        double dist = Math.Sqrt(Math.Pow(middleX - x, 2) + Math.Pow(middleY - y, 2) * 4);
                        if (Math.Abs(dist - maxDist) <= 0.5)
                        {
                            if (x == toX && y == toY)
                            {
                                written = true;
                            }
                            Console.SetCursorPosition(x + 1, y + 2);
                            Console.Write(fullScreen[x, y]);
                        }
                    }
                }
                maxDist++;
                Thread.Sleep(1);
            }
            if (!written && writtenForced)
            {
                List<String> fullScreenStr = new List<String>();
                for (int y = 0; y < fullScreen.GetLength(1); y++)
                {
                    String line = "";
                    for (int x = 0; x < fullScreen.GetLength(0); x++)
                    {
                        line += fullScreen[x, y];
                    }
                    Console.SetCursorPosition(1, y + 2);
                    Console.Write(line);
                }
            }
        }
        static void prepareGameBorder()
        {
            Console.Clear();
            Console.WriteLine("aaaaaa");
            StringBuilder top = new StringBuilder(new string('▄', width));
            StringBuilder middle = new StringBuilder("█" + new string(' ', width - 2) + "█");
            StringBuilder bottom = new StringBuilder(new string('▀', width));

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(top);
            for (int x = 1; x < height - 1; x++) Console.WriteLine(middle);
            Console.WriteLine(bottom);
            Console.ForegroundColor = ConsoleColor.White;

        }
        static void allWhiteScreen()
        {
            Console.Clear();
            Console.WriteLine("aaaaaa");
            StringBuilder middle = new StringBuilder(new string('█', width));
            for (int x = 0; x < height; x++) Console.WriteLine(middle);

        }
        static void clearEntireScreen()
        {
            currentScreen.Clear();
            for (int i = 0; i < height-2; i++) currentScreen.Add(new StringBuilder(new string(' ', width - 2)));
            //currentScreen[0] = new StringBuilder("X" + new string(' ', width -4)+"X");
            //currentScreen[height-3] = new StringBuilder("X" + new string(' ', width -4) + "X");
        }
        static void reWriteLivesAndScore()
        {
            if (!redrawing)
            {
                redrawingTwo = true;
                Console.SetCursorPosition(1, height);
                Console.Write($" Lives: {player.health} ");
                Console.SetCursorPosition(width - 13, height);
                Console.Write($" Score: {score}    ");
                redrawingTwo = false;
            }
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

                Console.SetCursorPosition(1, lineNum+2);
                Console.Write(line);
            }
            lastScreen.Clear();
            //lastScreen.AddRange(currentScreen);
            currentScreen.ForEach(delegate (StringBuilder str) {
                lastScreen.Add(new StringBuilder(str.ToString()));
            });
            redrawing = false;
        }
        static void reDrawPlayer()
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
                if (!redrawing && !redrawingTwo)
                {
                    Console.SetCursorPosition(posX+1, y+2);
                    Console.Write(line.Substring(posX, playerWidth));
                }
            }
            //Console.ResetColor();
        }

    }

}
