using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using graphiclaEditor.Shapes;
using Newtonsoft.Json;

namespace graphiclaEditor
{
    
    class Ellipse : RectBase
    {



        public override void Paint(Canvas canvas)
        {
            var elips = new System.Windows.Shapes.Ellipse();

            elips.Width = Math.Abs(LeftTop.x - RightBot.x);
            elips.Height = Math.Abs(LeftTop.y - RightBot.y);

            EventFuncs.SetPaintSettings(elips, this.style.Fill, this.style.StrokePen);

            EventFuncs.SetMouseUpEvent(elips, canvas);

            canvas.Children.Add(elips);
            Canvas.SetLeft(elips, LeftTop.x);
            Canvas.SetTop(elips, LeftTop.y);
        }

        public override void Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {
            this.style = new ShapeStyle(newFill, newStroke);

            this.Paint(canvas);
            

        }

        public Ellipse(Cords c1, Cords c2)
        {

            this.LeftTop.x = Math.Min(c1.x, c2.x);
            this.LeftTop.y = Math.Min(c1.y, c2.y);
            this.RightBot.x = Math.Max(c1.x, c2.x);
            this.RightBot.y = Math.Max(c1.y, c2.y);
        }

    }
}
