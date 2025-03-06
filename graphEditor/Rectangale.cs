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

namespace graphiclaEditor
{
    class Rectangale : RectBase
    {

        public override System.Windows.Shapes.Shape Paint(Canvas canvas)
        {
            return Paint(canvas,this.Fill, this.Stroke);
        }

        public override System.Windows.Shapes.Shape Paint(Canvas canvas,Brush newFill, Pen newStroke)
        {
            var rect = new System.Windows.Shapes.Rectangle();

            rect.Width = Math.Abs(LeftTop.x - RightBot.x);
            rect.Height = Math.Abs(LeftTop.y - RightBot.y);

            rect.Stroke = newStroke.Brush;
            rect.StrokeThickness = newStroke.Thickness;
            rect.Fill = newFill;

            rect.MouseLeftButtonUp += (sender, e) =>
            {
                // Передача события родительскому элементу
                var parent = canvas;
                MouseButtonEventArgs newEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
                {
                    RoutedEvent = System.Windows.Input.Mouse.MouseUpEvent
                };
                parent.RaiseEvent(newEvent);
            };

            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, LeftTop.x);
            Canvas.SetTop(rect, LeftTop.y);
            return rect;

        }

        public Rectangale(Cords c1, Cords c2)
        {
            if (c1.x > c2.x)
            {
                this.LeftTop.x = c2.x;
                this.RightBot.x = c1.x;
            }
            else
            {
                this.LeftTop.x = c1.x;
                this.RightBot.x = c2.x;
            }

            if (c1.y > c2.y)
            {
                this.LeftTop.y = c2.y;
                this.RightBot.y = c1.y;
            }
            else
            {
                this.LeftTop.y = c1.y;
                this.RightBot.y = c2.y;
            }


            
        }
    }
}
