using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace graphiclaEditor
{
    public struct Cords
    {
        public double x;
        public double y;
        public static double Distance(Cords p1, Cords p2)
        {
            double tempX = p1.x - p2.x;
            double tempY = p1.y - p2.y;
            tempX *= tempX;
            tempY += tempY;

            return Math.Sqrt(tempY + tempX);
        }

        public void Copy(Point pnt)
        {
            x = pnt.X;
            y = pnt.Y;
        }

        public Cords(Point pnt)
        {
            x = pnt.X;
            y = pnt.Y;
        }
    }

    public abstract class RectBase : Shape 
    {
        protected Cords LeftTop;
        protected Cords RightBot;

    }

    public abstract class CircleBase : Shape
    {
        protected Cords Center;
        protected double Radius;
    }
}
