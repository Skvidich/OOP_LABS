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

namespace graphiclaEditor
{
    class Ellipse : RectBase
    {



        public override System.Windows.Shapes.Shape Paint(Canvas canvas)
        {
            return Paint(canvas, this.Fill, this.Stroke);
        }

        public override System.Windows.Shapes.Shape Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {
            var elips = new System.Windows.Shapes.Ellipse();

            elips.Width = Math.Abs(LeftTop.x - RightBot.x);
            elips.Height = Math.Abs(LeftTop.y - RightBot.y);

            EventFuncs.SetPaintSettings(elips, newFill, newStroke);

            EventFuncs.SetMouseUpEvent(elips, canvas);

            canvas.Children.Add(elips);
            Canvas.SetLeft(elips, LeftTop.x);
            Canvas.SetTop(elips, LeftTop.y);
            return elips;

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
