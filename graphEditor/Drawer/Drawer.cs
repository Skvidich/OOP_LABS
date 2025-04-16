using graphiclaEditor.Shapes;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace graphiclaEditor
{
    public class Drawer
    {
        private Canvas drawingArea;

        private ConstructorInfo? currConstructor;
        public ConstructorInfo? CurrConstructor { set => currConstructor = value; }


        private BaseClass currBase;
        public BaseClass CurrBase { set => currBase = value; }




        // Drawing parametrs
        private int countVert;
        public int CountVert { set => countVert = value; }

        private Double strokeThickness;
        public Double StrokeThickness { set => strokeThickness = value; }

        private Brush strokeColorBrush = Brushes.Black;
        public Brush StrokeColorBrush { get => strokeColorBrush; set => strokeColorBrush = value; }
        private Brush fillColorBrush = Brushes.Black;
        public Brush FillColorBrush { get => fillColorBrush; set => fillColorBrush = value; }

        private Shape previewElem;
        private Cords startPoint;
        private List<Cords> cordList;


        private List<Shape> shapeList;
        public List<Shape> ShapeList { get => shapeList; set { shapeList = value; this.Redraw(); } }
        private Stack<Shape> redoStack;

        public  bool StartDraw(Cords start)
        {
            if (currConstructor == null) { return false; }

            startPoint = start;
            cordList = new List<Cords>();
            cordList.Add(start);
            previewElem = CurrentDraw(startPoint,startPoint);

            return true;
        }

        public  void EndDraw(Cords endPoint)
        {

            drawingArea.Children.RemoveAt(drawingArea.Children.Count - 1);
            previewElem = CurrentDraw(startPoint, endPoint);
            shapeList.Add(previewElem);
            if (redoStack.Count() != 0)
            {
                redoStack.Clear();
            }

        }

        public void ProcessDraw(Cords currPoint)
        {
            
            drawingArea.Children.RemoveAt(drawingArea.Children.Count - 1);
            cordList.Add(currPoint);
            previewElem = CurrentDraw(startPoint, currPoint);
            cordList.Remove(currPoint);

        }

       public void PolyProcessDraw(Cords currPoint)
        {
            if ( currBase != BaseClass.bcPoly) return;

            cordList.Add(currPoint);
            ProcessDraw(currPoint);

       }
        private object[] GetConstructorArgs(Cords c1, Cords c2)
        {
            var constructors = new Dictionary<BaseClass, Func<object[]>>
    {
        { BaseClass.bcRect,  () => new object[] { c1, c2 } },
        { BaseClass.bcCircle, () => new object[] { c1, c2, countVert } },
        { BaseClass.bcPoly,   () => new object[] { cordList } }
    };

            if (!constructors.TryGetValue(currBase, out var getArgs))
            {
                throw new Exception("Unknown class");
            }

            return getArgs();
        }

        private Shape CurrentDraw(Cords c1, Cords c2)
        {
            if (currConstructor == null)
            {
                throw new InvalidOperationException("Constructor is not set.");
            }

            var pen = new Pen() { Thickness = strokeThickness, Brush = strokeColorBrush };

            var args = GetConstructorArgs(c1, c2);

            var shape = (Shape)currConstructor.Invoke(args);

            shape.Paint(drawingArea, fillColorBrush, pen);

            return shape;
        }
        public void Clear()
        {
            drawingArea.Children.Clear();
        }


        public Drawer(Canvas Area)
        {
            drawingArea = Area;
            shapeList = new List<Shape>();
            redoStack = new Stack<Shape>();
        }

        public bool Undo()
        {
            if (shapeList.Count() == 0) return false;

            var temp =  shapeList.Last();
            shapeList.Remove(temp);
            redoStack.Push(temp);
            Redraw();
            return true;
        }

        public bool Redo()
        {
            if (redoStack.Count() == 0) return false;
            var temp = redoStack.Pop();
            shapeList.Add(temp);
            Redraw();
            return true;
        }

        public void Redraw()
        {
            drawingArea.Children.Clear();
            foreach(var shp in shapeList)
            {
                shp.Paint(drawingArea);
            }
        }



    }
}
