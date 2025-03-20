using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace graphiclaEditor
{
    public static class EventFuncs
    {
        public static void SetMouseUpEvent(System.Windows.Shapes.Shape shape, Canvas canvas)
        {
           shape.MouseLeftButtonUp += (sender, e) =>
            {
                // Передача события родительскому элементу
                var parent = canvas;
                MouseButtonEventArgs newEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
                {
                    RoutedEvent = System.Windows.Input.Mouse.MouseUpEvent
                };
                parent.RaiseEvent(newEvent);
            };
        }

        public static void SetMouseDownEvent(System.Windows.Shapes.Shape shape, Canvas canvas)
        {
            shape.MouseRightButtonDown += (sender, e) =>
            {
                var parent = canvas;
                MouseButtonEventArgs newEvent = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Right)
                {
                    RoutedEvent = System.Windows.Input.Mouse.MouseDownEvent
                };
                parent.RaiseEvent(newEvent);
            };
        }

        public static void SetPaintSettings(System.Windows.Shapes.Shape shape, Brush newFill, Pen newStroke)
        {
            shape.Stroke = newStroke.Brush;
            shape.StrokeThickness = newStroke.Thickness;
            shape.Fill = newFill;
        }
    }
}
