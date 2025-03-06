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
    class Polygon : RectBase
    {

        protected List<Point> points;
        public override System.Windows.Shapes.Shape Paint(Canvas canvas)
        {
            return Paint(canvas, this.Fill, this.Stroke);
        }

        public override System.Windows.Shapes.Shape Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {
            var polygon = new System.Windows.Shapes.Polygon();


            polygon.Stroke = newStroke.Brush;
            polygon.StrokeThickness = newStroke.Thickness;
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

            polygon.MouseRightButtonDown += (sender, e) =>
            {

                var parent = canvas;
                MouseButtonEventArgs newEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Right)
                {
                    RoutedEvent = System.Windows.Input.Mouse.MouseDownEvent
                };
                parent.RaiseEvent(newEvent);
            };

            polygon.Points = new PointCollection(points);
            canvas.Children.Add(polygon);

            return polygon;

        }

        public Polygon(List<Cords> cPoints)
        {


            points = cPoints.Select(c => new Point(c.x, c.y)).ToList();

            LeftTop.x = cPoints.Min(c => c.x);
            RightBot.x = cPoints.Max(c => c.x);
            LeftTop.y = cPoints.Min(c => c.y);
            RightBot.y = cPoints.Max(c => c.y);

        }
    }
}
