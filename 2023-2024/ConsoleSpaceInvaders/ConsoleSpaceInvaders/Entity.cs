using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSpaceInvaders
{
    internal class Entity
    {
        public string id;
        public int posX;
        public int posY;
        /*public int[] moveX;//[0] = the number of 100ms loops before moving by [1]   ->  the higher the number, the slower the actual speed
        public int[] moveY;*/
        private int lastModelNum = 0;
        public List<string> models = new List<String>();

        public Entity() { }
        public Entity(string id) 
        { 
            this.id = id;
        }
        public int getSizeX()
        {
            if (models.Count == 0) return 0;
            int maxLength = -1;
            foreach (string model in models)
            {
                List<String> modelLines = new List<String>();
                if (model.Contains("\n")) modelLines.AddRange(model.Split(new string[] { "\n" }, StringSplitOptions.None));
                else modelLines.Add(model);
                foreach (string modelLine in modelLines)
                {
                    maxLength = Math.Max(maxLength, modelLine.Length);
                }
            }
            return maxLength;
        }
        public int getSizeY()
        {
            if (models.Count == 0) return 0;
            int maxLength = -1;
            foreach (string s in models)
            {
                if (!s.Contains("\n")) continue;
                maxLength = Math.Max(maxLength, s.Split(new string[] { "\n" }, StringSplitOptions.None).Length);
            }
            return maxLength;
        } 
        public void setPos(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
        }
        public void moveBy(int moveX, int moveY)
        {
            posX += moveX;
            posY += moveY;
        }
        public string getModel(Boolean addToLastModelNum)
        {
            if (addToLastModelNum)
            {
                lastModelNum++;
                if (lastModelNum == models.Count) lastModelNum = 0;
            }
            return models[lastModelNum];
        }
        public void addModel(string model)
        {
            models.Add(model);
        }
        public void setModel(string model)
        {
            models.Clear();
            addModel(model);
        }
        private List<StringBuilder> writeOrDeleteScreen(List<StringBuilder> currentScreen, bool deleteNotWrite)
        {
            int currentPosY = posY;
            List<String> modelLines = new List<String>();
            String model = getModel(!deleteNotWrite);
            if (model.Contains("\n"))
            {
                modelLines.AddRange(model.Split(new string[] { "\n" }, StringSplitOptions.None));
            }
            else
            {
                modelLines.Add(model);
            }
            foreach (String modelLine in modelLines)
            {
                int currentPosX = posX;
                foreach (Char modelChar in modelLine.ToCharArray())
                {
                    if (modelChar == ' ')
                    {
                        currentPosX++;
                        continue;
                    }
                    if (currentPosY < 0 || currentPosY >= currentScreen.Count) break;
                    if (currentPosX < 0 || currentPosX >= currentScreen[currentPosY].Length) break;
                    Char replacedChar = currentScreen[currentPosY][currentPosX];
                    currentScreen[currentPosY][currentPosX] = (deleteNotWrite) ? ' ' : modelChar;
                    currentPosX++;
                }
                currentPosY++;
            }
            return currentScreen;
        }
        public List<StringBuilder> writeToScreen(List<StringBuilder> currentScreen)
        {
            return writeOrDeleteScreen(currentScreen, false);
        }
        public List<StringBuilder> removeFromScreen(List<StringBuilder> currentScreen)
        {
            return writeOrDeleteScreen(currentScreen, true);
        }
    }
    class LivingEntity : Entity
    {
        public int health;
        public int score = 0;
        public int extraInfo;


        public LivingEntity() { }
        public LivingEntity(int health)
        {
            this.health = health;
        }
        public LivingEntity(int health, int score)
        {
            this.health = health;
            this.score = score;
        }
        public Entity createProjectile(List<StringBuilder> currentScreen, string projectileModel, int[] shootFrom)
        {
            Entity projectile = new Entity();
            projectile.addModel(projectileModel);
            projectile.setPos(shootFrom[0], shootFrom[1]);
            //projectile.writeToScreen(currentScreen);
            return projectile;
        }
    }
}
