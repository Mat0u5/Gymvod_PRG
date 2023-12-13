using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            Human human1 = new Human();
            human1.age = 18;
            human1.name = "Tobias";
            human1.height = 250;
            human1.weight = 60;
            human1.skinColor = "blue";
            human1.introduceHuman();
            Human human2 = new Human(18, 150, 70, "Marek", "pink");
            human2.introduceHuman();
            float bmi = human2.getBMI();
            Console.WriteLine($"{human2.name} has a bmi  of {bmi}");

            human2.partner = human1;
            human1.partner = human2;

            Human child = Human.makeChild(human1,human2);
            child.introduceHuman();
            Console.WriteLine($"{child.name}'s skin color: {child.skinColor}");
            */
            Animal newAnimal = new Animal();
            newAnimal.MakeNoise();

            Dog newDog = new Dog();
            newDog.breed = "p e s";
            newDog.name = "dogname";
            newDog.age = 167;
            Console.WriteLine($"The dog's name is {newDog.name}, it's {newDog.age} years old and it's breed is {newDog.breed}");
            newDog.MakeNoise();

            Cat newCat = new Cat();
            newCat.furColour = "white";
            newCat.name = "catbname";
            newCat.age = 99999999;
            Console.WriteLine($"The cat's name is {newCat.name}, it's {newCat.age} years old and it's fur colour is {newCat.furColour}");
            newCat.MakeNoise();

            Console.ReadKey();
        }

        class Animal
        {
            public string name;
            public int age;
            public virtual void MakeNoise()
            {
                Console.WriteLine("*animal noises*");
            }
        }
        class Dog : Animal
        {
            public string breed;
            public override void MakeNoise()
            {
                Console.WriteLine("*bark bark*");
            }
        }
        class Cat : Animal
        {
            public string furColour;
            public override void MakeNoise()
            {
                Console.WriteLine("*meeeeeeeeow*");
            }
        }

        class Human
        {
            public int age;
            public int height;
            public int weight;
            public string name;
            public string skinColor;
            public Human partner;
            public Human(int age, int height, int weight, string name, string skinColor)
            {
                this.age = age;
                this.height = height;
                this.weight = weight;
                this.name = name;
                this.skinColor = skinColor;
            }
            public Human(string name)
            {
                this.name = name;
            }
            public Human()
            {
            }

            public void introduceHuman()
            {
                Console.WriteLine($"Hi, my name is {name}, I'm {age} years old, I weigh {weight} kilograms and I'm {height} cm tall");
            }
            public float getBMI()
            {
                double heightCm = height / 100;
                double bmi = weight / Math.Pow(heightCm, 2);
                return (float) bmi;
            }
            public static Human makeChild(Human human1, Human human2)
            {
                if (human1.partner != human2 || human2.partner != human1)
                {
                    Console.WriteLine("lehký cheating");
                    return new Human("Bastard");
                }
                Human child = new Human();
                child.weight = (human1.weight + human2.weight) / 2;
                child.height = (human1.height + human2.height) / 2;
                child.name = human1.name + human2.name;
                child.age = 0;
                child.partner = null;
                Random rnd = new Random();
                String[] skinColors = {human1.skinColor,human2.skinColor};
                child.skinColor = skinColors[rnd.Next(2)];

                return child;
            }
        }
    }
}
