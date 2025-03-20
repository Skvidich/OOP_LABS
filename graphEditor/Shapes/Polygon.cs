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
    class Polygon : PolyBase
    {

        public override System.Windows.Shapes.Shape Paint(Canvas canvas)
        {
            return Paint(canvas, this.Fill, this.Stroke);
        }

        public override System.Windows.Shapes.Shape Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {
            var polygon = new System.Windows.Shapes.Polygon();


            EventFuncs.SetPaintSettings(polygon, newFill, newStroke);

            EventFuncs.SetMouseDownEvent(polygon, canvas);

            EventFuncs.SetMouseUpEvent(polygon, canvas);

            polygon.Points = new PointCollection(points);
            canvas.Children.Add(polygon);

            return polygon;

        }

        public Polygon(List<Cords> cPoints)
        {

            points = cPoints.Select(c => new Point(c.x, c.y)).ToList();

        }
    }
}
