using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace graphiclaEditor.Shapes
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
            tempY *= tempY;

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
}
