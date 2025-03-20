using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using graphiclaEditor.Shapes;


namespace graphiclaEditor
{
    

    public abstract class RectBase : Shape 
    {
        protected Cords LeftTop;
        protected Cords RightBot;

    }

    public abstract class CircleBase : Shape
    {
        protected Cords Center;
        protected double Radius;
        protected int VertCount;
    }
}
