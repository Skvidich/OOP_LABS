using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using graphiclaEditor.Shapes;

namespace graphiclaEditor
{
    public class Line : RectBase
    {
        public override void Paint(Canvas canvas)
        {
            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();

            line.X1 = this.LeftTop.x;
            line.Y1 = this.LeftTop.y;

            line.X2 = this.RightBot.x;
            line.Y2 = this.RightBot.y;

            EventFuncs.SetPaintSettings(line, this.style.Fill, this.style.StrokePen);

            EventFuncs.SetMouseUpEvent(line, canvas);

            canvas.Children.Add(line);
    

        }

        public override void Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {
            this.style = new ShapeStyle(newFill, newStroke);
            this.Paint(canvas);

        }

        public Line(Cords c1, Cords c2)
        {
            this.LeftTop = c1;
            this.RightBot = c2;
        }

    }
}
