using graphiclaEditor.Shapes;
using graphicalEditor;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace graphiclaEditor.Shapes
{
    public class SimpleCircle : CircleBase
    {
        [JsonProperty]
        protected Cords StartPoint;

        public SimpleCircle(Cords center, Cords edge, int vertCount)
        {
            Center = center;
            Radius = Cords.Distance(center, edge); 
            VertCount = vertCount;
            StartPoint = edge;
        }

        

        


        public override void Paint(Canvas canvas)
        {
            var circle = new System.Windows.Shapes.Ellipse();

            EventFuncs.SetPaintSettings(circle, this.style.Fill, this.style.StrokePen);

            EventFuncs.SetMouseUpEvent(circle, canvas);

            circle.Width = Radius*2;
            circle.Height = Radius*2;

            
            
            canvas.Children.Add(circle);
            Canvas.SetLeft(circle, Center.x-Radius);
            Canvas.SetTop(circle, Center.y-Radius);
        }

        public override void Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {

            this.style = new ShapeStyle(newFill, newStroke);
            this.Paint(canvas);

        }



    }
}
