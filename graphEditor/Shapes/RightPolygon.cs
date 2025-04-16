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
using Newtonsoft.Json;

namespace graphiclaEditor
{
   
    class RightPolygon : CircleBase
    {
        [JsonProperty]
        protected Cords StartPoint;
        public override void Paint(Canvas canvas)
        {
            var polygon = new System.Windows.Shapes.Polygon();

            EventFuncs.SetPaintSettings(polygon, this.style.Fill, this.style.StrokePen);

            EventFuncs.SetMouseUpEvent(polygon, canvas);

            double dx = StartPoint.x - Center.x;
            double dy = StartPoint.y - Center.y;
            double startAngle = Math.Atan2(dy, dx);

            for (int i = 0; i < VertCount; i++)
            {
                double angle = startAngle + 2 * Math.PI * i / VertCount - Math.PI / 2;
                double x = Center.x + Radius * Math.Cos(angle);
                double y = Center.y + Radius * Math.Sin(angle);
                polygon.Points.Add(new Point(x, y));
            }
            canvas.Children.Add(polygon);
        }

        public override void Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {

            this.style = new ShapeStyle(newFill, newStroke);
            this.Paint(canvas);

        }

        public RightPolygon(Cords c1, Cords c2,int count)
        {
            Center = c1;
            Radius = Cords.Distance(c1, c2);
            VertCount = count;
            StartPoint = c2;
        }
    }
}
