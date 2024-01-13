using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSpaceInvaders
{
    internal class Screen : Program
    {
        
        public void gameOverScreen()
        {
            String gameOverScreen = "           _____                         ____                          \n          / ____|                       / __ \\                         \n         | |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __          \n         | | |_ |/ _` | '_ ` _ \\ / _ \\ | |  | \\ \\ / / _ \\ '__|         \n         | |__| | (_| | | | | | |  __/ | |__| |\\ V /  __/ |            \n          \\_____|\\__,_|_| |_| |_|\\___|  \\____/  \\_/ \\___|_|            \n                                                                       \n                                                                       \n┌─┐┬─┐┌─┐┌─┐┌─┐  ┌─┐┌┐┌┬ ┬  ┬┌─┌─┐┬ ┬  ┌┬┐┌─┐  ┌─┐┌─┐┌┐┌┌┬┐┬ ┌┐┌ ┬ ┬┌─┐\n├─┘├┬┘├┤ └─┐└─┐  ├─┤│││└┬┘  ├┴┐├┤ └┬┘   │ │ │  │  │ ││││ │ │ │││ │ │├┤ \n┴  ┴└─└─┘└─┘└─┘  ┴ ┴┘└┘ ┴   ┴ ┴└─┘ ┴    ┴ └─┘  └─┘└─┘┘└┘ ┴ ┴ ┘└┘ └─┘└─┘";
            List<String> gameOverScreenList = new List<String>();
            gameOverScreenList.AddRange(gameOverScreen.Split(new string[] { "\n" }, StringSplitOptions.None));
            transitionIntoScreen(gameOverScreenList);
        }
        public void startScreen()
        {
            allWhiteScreen();
            String startScreen = "________/\\\\\\\\\\\\\\\\\\_______/\\\\\\\\\\_______/\\\\\\\\\\_____/\\\\\\_____/\\\\\\\\\\\\\\\\\\\\\\_________/\\\\\\\\\\_______/\\\\\\______________/\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_        \n _____/\\\\\\////////______/\\\\\\///\\\\\\____\\/\\\\\\\\\\\\___\\/\\\\\\___/\\\\\\/////////\\\\\\_____/\\\\\\///\\\\\\____\\/\\\\\\_____________\\/\\\\\\///////////__       \n  ___/\\\\\\/_____________/\\\\\\/__\\///\\\\\\__\\/\\\\\\/\\\\\\__\\/\\\\\\__\\//\\\\\\______\\///____/\\\\\\/__\\///\\\\\\__\\/\\\\\\_____________\\/\\\\\\_____________      \n   __/\\\\\\______________/\\\\\\______\\//\\\\\\_\\/\\\\\\//\\\\\\_\\/\\\\\\___\\////\\\\\\__________/\\\\\\______\\//\\\\\\_\\/\\\\\\_____________\\/\\\\\\\\\\\\\\\\\\\\\\_____     \n    _\\/\\\\\\_____________\\/\\\\\\_______\\/\\\\\\_\\/\\\\\\\\//\\\\\\\\/\\\\\\______\\////\\\\\\______\\/\\\\\\_______\\/\\\\\\_\\/\\\\\\_____________\\/\\\\\\///////______    \n     _\\//\\\\\\____________\\//\\\\\\______/\\\\\\__\\/\\\\\\_\\//\\\\\\/\\\\\\_________\\////\\\\\\___\\//\\\\\\______/\\\\\\__\\/\\\\\\_____________\\/\\\\\\_____________   \n      __\\///\\\\\\___________\\///\\\\\\__/\\\\\\____\\/\\\\\\__\\//\\\\\\\\\\\\__/\\\\\\______\\//\\\\\\___\\///\\\\\\__/\\\\\\____\\/\\\\\\_____________\\/\\\\\\_____________  \n       ____\\////\\\\\\\\\\\\\\\\\\____\\///\\\\\\\\\\/_____\\/\\\\\\___\\//\\\\\\\\\\_\\///\\\\\\\\\\\\\\\\\\\\\\/______\\///\\\\\\\\\\/_____\\/\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_\\/\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_ \n        _______\\/////////_______\\/////_______\\///_____\\/////____\\///////////__________\\/////_______\\///////////////__\\///////////////__\n                                                                                                                                       \n                                                                                                                                       \n          _______  _______  _______  _______  _______      _________ _                 _______  ______   _______  _______  _______     \n         (  ____ \\(  ____ )(  ___  )(  ____ \\(  ____ \\     \\__   __/( (    /||\\     /|(  ___  )(  __  \\ (  ____ \\(  ____ )(  ____ \\    \n         | (    \\/| (    )|| (   ) || (    \\/| (    \\/        ) (   |  \\  ( || )   ( || (   ) || (  \\  )| (    \\/| (    )|| (    \\/    \n         | (_____ | (____)|| (___) || |      | (__            | |   |   \\ | || |   | || (___) || |   ) || (__    | (____)|| (_____     \n         (_____  )|  _____)|  ___  || |      |  __)           | |   | (\\ \\) |( (   ) )|  ___  || |   | ||  __)   |     __)(_____  )    \n               ) || (      | (   ) || |      | (              | |   | | \\   | \\ \\_/ / | (   ) || |   ) || (      | (\\ (         ) |    \n         /\\____) || )      | )   ( || (____/\\| (____/\\     ___) (___| )  \\  |  \\   /  | )   ( || (__/  )| (____/\\| ) \\ \\__/\\____) |    \n         \\_______)|/       |/     \\|(_______/(_______/     \\_______/|/    )_)   \\_/   |/     \\|(______/ (_______/|/   \\__/\\_______)    \n                                                                                                                                       \n                                                                                                                                       \n                                                                                                                                       \n                                                                                                                                       \n                                                                                                                                       \n                                                                                             Notes: Play on fullscreen                 \n                                                                                                    Sounds ON                          \n┌─┐┬─┐┌─┐┌─┐┌─┐  ┌─┐┌┐┌┬ ┬  ┬┌─┌─┐┬ ┬  ┌┬┐┌─┐  ┌─┐┌┬┐┌─┐┬─┐┌┬┐                               Controls: Move left - A, LeftArrow        \n├─┘├┬┘├┤ └─┐└─┐  ├─┤│││└┬┘  ├┴┐├┤ └┬┘   │ │ │  └─┐ │ ├─┤├┬┘ │                                          MoveRight - D, RightArrow       \n┴  ┴└─└─┘└─┘└─┘  ┴ ┴┘└┘ ┴   ┴ ┴└─┘ ┴    ┴ └─┘  └─┘ ┴ ┴ ┴┴└─ ┴                                          Shoot - W, UpArrow              ";
            List<String> startScreenList = new List<String>();
            startScreenList.AddRange(startScreen.Split(new string[] { "\n" }, StringSplitOptions.None));
            transitionIntoScreen(startScreenList);
        }
        public void transitionIntoScreen(List<String> screen)
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
            //the code above places the screen into the middle of char[]
            bool written = false;
            bool writtenForced = false;
            int maxDist = 0;
            void stopTransition()
            {
                while (!written)
                {
                    Thread.Sleep(50);
                    if (!Console.KeyAvailable) continue;
                    Char key = Console.ReadKey().KeyChar;
                    writtenForced = true;
                }
            }
            Thread thread = new Thread(new ThreadStart(stopTransition));
            thread.Start();
            int arraySizeX = fullScreen.GetLength(0);
            int arraySizeY = fullScreen.GetLength(1);
            while (!written && !writtenForced)
            {
                //This loop Basically writes the characters on the border of a circle, that gets bigger and bigger
                for (int y = 0; y < arraySizeY; y++)
                {
                    for (int x = 0; x < arraySizeX; x++)
                    {
                        double dist = Math.Sqrt(Math.Pow(middleX - x, 2) + Math.Pow(middleY - y, 2) * 4);
                        if (Math.Abs(dist - maxDist) <= 0.5)
                        {
                            if (x == 0 && y == 0)
                            {
                                written = true;
                            }
                            Console.SetCursorPosition(x + 1, y + 1);
                            Console.Write(fullScreen[x, y]);
                        }
                    }
                }
                maxDist++;
                Thread.Sleep(10);
            }
            if (!written && writtenForced)//if the animation has been skipped
            {
                List<String> fullScreenStr = new List<String>();
                for (int y = 0; y < fullScreen.GetLength(1); y++)
                {
                    String line = "";
                    for (int x = 0; x < fullScreen.GetLength(0); x++)
                    {
                        line += fullScreen[x, y];
                    }
                    Console.SetCursorPosition(1, y + 1);
                    Console.Write(line);
                }
            }
        }
        public void prepareGameBorder()
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
        public void allWhiteScreen()
        {
            Console.Clear();
            StringBuilder top = new StringBuilder(new string('▄', width));
            StringBuilder middle = new StringBuilder(new string('█', width));
            StringBuilder bottom = new StringBuilder(new string('▀', width));
            Console.WriteLine(top);
            for (int x = 1; x < height - 1; x++) Console.WriteLine(middle);
            Console.WriteLine(bottom);

        }
        public void clearEntireScreen()
        {
            currentScreen.Clear();
            for (int i = 0; i < height - 2; i++) currentScreen.Add(new StringBuilder(new string(' ', width - 2)));
            //currentScreen[0] = new StringBuilder("X" + new string(' ', width -4)+"X");
            //currentScreen[height-3] = new StringBuilder("X" + new string(' ', width -4) + "X");
        }
        public void reWriteLivesAndScore()
        {
            if (!redrawing)
            {
                redrawingTwo = true;
                Console.SetCursorPosition(1, height-1);
                Console.Write($" Lives: {player.health} ");
                Console.SetCursorPosition(width - 13, height-1);
                Console.Write($" Score: {score}    ");
                redrawingTwo = false;
            }
        }
        public void reDrawScreen()
        {
            redrawing = true;

            int lineNum = -1;
            foreach (StringBuilder line in currentScreen)
            {
                lineNum++;
                if (lastScreen.Count == currentScreen.Count)
                {
                    //if the line hasn't changed from the last time it has been written
                    if (line == lastScreen[lineNum]) continue;
                }

                Console.SetCursorPosition(1, lineNum + 1);
                Console.Write(line);
            }
            lastScreen.Clear();
            //lastScreen.AddRange(currentScreen);
            currentScreen.ForEach(delegate (StringBuilder str) {
                lastScreen.Add(new StringBuilder(str.ToString()));
            });
            redrawing = false;
        }
        public void reDrawPlayer()
        {
            int posX = player.posX - 2;
            int posY = player.posY;
            int playerWidth = player.getSizeX() + 4;
            int playerHeight = player.getSizeY();

            //these checks are here to prevent out of bounds exceptions, when the player is on the sides of the map
            if (posX < 0)
            {
                playerWidth += posX;
                posX = 0;
            }
            if (posX + playerWidth > width - 2)
            {
                playerWidth = width - posX - 2;
            }

            for (int y = posY; y < (posY + playerHeight - 1); y++)
            {
                string line = currentScreen[y].ToString();
                if (!redrawing && !redrawingTwo)
                {
                    Console.SetCursorPosition(posX + 1, y + 1);
                    Console.Write(line.Substring(posX, playerWidth));
                }
            }
        }
    }
}
