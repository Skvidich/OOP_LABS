using graphiclaEditor.Shapes;
using graphiclaEditor;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace graphiclaEditor.Shapes
{
    public class Trapecy : CircleBase
    {
        

        public Trapecy(Cords center, Cords edge, int vertCount)
        {
            Center = center;
            Radius = Cords.Distance(center, edge);
            VertCount = vertCount;
            
        }


        public override void Paint(Canvas canvas, Brush newFill, Pen newStroke)
        {

            this.style = new ShapeStyle(newFill, newStroke);
            this.Paint(canvas);

        }

        public override void Paint(Canvas canvas)
        {
            var polygon = new System.Windows.Shapes.Polygon();

            EventFuncs.SetPaintSettings(polygon, this.style.Fill, this.style.StrokePen);
            EventFuncs.SetMouseDownEvent(polygon, canvas);
            EventFuncs.SetMouseUpEvent(polygon, canvas);

            polygon.Points = CalculateTrapecyPoints();
            canvas.Children.Add(polygon);
        }

        private PointCollection CalculateTrapecyPoints()
        {
            
            double bottomWidth = Radius * 2;
            double topWidth = Radius; 
            double height = Radius;

            double cx = Center.x;
            double cy = Center.y;

            return new PointCollection
            {
                new Point(cx - bottomWidth / 2, cy + height / 2),   
                new Point(cx + bottomWidth / 2, cy + height / 2),  
                new Point(cx + topWidth / 2, cy - height / 2),      
                new Point(cx - topWidth / 2, cy - height / 2)       
            };
        }

    }
}
