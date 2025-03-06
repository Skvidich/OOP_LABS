using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace graphiclaEditor
{
   
    class RightPolygon : CircleBase
    {
        protected int VertCount;
        protected Cords StartPoint;
        public override System.Windows.Shapes.Shape Paint(Canvas canvas)
        {
            return Paint(canvas, this.Fill, this.Stroke);
        }

        public override System.Windows.Shapes.Shape Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {
            var polygon = new System.Windows.Shapes.Polygon();

            polygon.Stroke = newStroke.Brush;
            polygon.StrokeThickness = 2;
            polygon.Fill = newFill;

            polygon.MouseLeftButtonUp += (sender, e) =>
            {
                
                var parent = canvas;
                MouseButtonEventArgs newEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
                {
                    RoutedEvent = System.Windows.Input.Mouse.MouseUpEvent
                };
                parent.RaiseEvent(newEvent);
            };

            double dx = StartPoint.x - Center.x;
            double dy = StartPoint.y - Center.y;
            double startAngle = Math.Atan2(dy, dx);

            for (int i = 0; i < VertCount; i++)
            {
                double angle =startAngle+ 2 * Math.PI * i / VertCount - Math.PI / 2; 
                double x = Center.x + Radius * Math.Cos(angle);
                double y = Center.y + Radius * Math.Sin(angle);
                polygon.Points.Add(new Point(x, y));
            }





            canvas.Children.Add(polygon);
            return polygon;

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
