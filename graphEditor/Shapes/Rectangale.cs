using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Input;
using graphiclaEditor.Shapes;

namespace graphiclaEditor
{
    class Rectangale : RectBase
    {

        public override void Paint(Canvas canvas)
        {
            var rect = new System.Windows.Shapes.Rectangle();

            rect.Width = Math.Abs(LeftTop.x - RightBot.x);
            rect.Height = Math.Abs(LeftTop.y - RightBot.y);

            EventFuncs.SetPaintSettings(rect, this.style.Fill, this.style.StrokePen);

            EventFuncs.SetMouseUpEvent(rect, canvas);

            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, LeftTop.x);
            Canvas.SetTop(rect, LeftTop.y);
        }

        public override void  Paint(Canvas canvas,Brush newFill, Pen newStroke)
        {

            this.style = new ShapeStyle(newFill, newStroke);
            this.Paint(canvas);

        }

        public Rectangale(Cords c1, Cords c2) 
        {
            this.LeftTop.x = Math.Min(c1.x, c2.x);
            this.LeftTop.y = Math.Min(c1.y, c2.y);
            this.RightBot.x = Math.Max(c1.x, c2.x);
            this.RightBot.y = Math.Max(c1.y, c2.y);


        }
    }
}
