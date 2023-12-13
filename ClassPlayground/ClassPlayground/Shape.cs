using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassPlayground
{
    internal class Rectangle
    {
        public double width;
        public double height;
        public Rectangle(double width, double height)
        {
            this.width = width;
            this.height = height;
        }
        public double CalculateArea()
        {
            return width * height;
        }
        public double CalculateAspectRatio()
        {
            return width / height;
        }

        public bool ContainsPoint(double x, double y)
        {
            return (x <= width && y <= height);
        }

    }
    internal class Circle
    {
        public double radius;
        public Circle(double radius)
        {
            this.radius = radius;
        }
        public double CalculateArea()
        {
            return Math.PI*Math.Pow(radius,2);
        }

        public bool ContainsPoint(double x, double y)
        {
            return (Math.Sqrt(x*x+y*y)<=radius);
        }

    }
    internal class Triangle
    {
        public double side;
        public double height;
        public Triangle(double side, double height)
        {
            this.side = side;
            this.height = height;
        }
        public double CalculateArea()
        {
            return (side*height)/2;
        }

    }
}
