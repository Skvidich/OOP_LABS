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
    
    class Polyline : PolyBase
    {
        
        public override void Paint(Canvas canvas)
        {
            var polyline = new System.Windows.Shapes.Polyline();


            var fill = new SolidColorBrush(new Color
            {
                A = 0,
            });
            EventFuncs.SetPaintSettings(polyline, fill, this.style.StrokePen);

            EventFuncs.SetMouseDownEvent(polyline, canvas);

            EventFuncs.SetMouseUpEvent(polyline, canvas);

            polyline.Points = new PointCollection(points);
            canvas.Children.Add(polyline);
        }

        public override void Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {


            this.style = new ShapeStyle(newFill, newStroke);
            this.Paint(canvas);

        }

        public Polyline(List<Cords> cPoints)
        {

            points = cPoints.Select(c => new Point(c.x, c.y)).ToList();

        }
    }
}
