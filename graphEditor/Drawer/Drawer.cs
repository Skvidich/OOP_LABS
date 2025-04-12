using graphiclaEditor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace graphiclaEditor
{
    public class Drawer
    {
        public  Canvas DrawingArea;
        public  ConstructorInfo? currConstructor;
        public  BaseClass currBase;


        public  int CountVert;
        public Double StrokeThickness;
        public Brush StrokeColorBrush = Brushes.Black;
        public Brush FillColorBrush = Brushes.Black;

        public   System.Windows.Shapes.Shape previewElem;
        public  Cords startPoint;
        public  List<Cords> CordList;

        public  bool StartDraw(Cords start)
        {
            if (currConstructor == null) { return false; }

            startPoint = start;
            CordList = new List<Cords>();
            CordList.Add(start);
            previewElem = CurrentDraw(startPoint,startPoint);

            return true;
        }

        public  void EndDraw(Cords endPoint)
        {

            DrawingArea.Children.Remove(previewElem);
            previewElem = CurrentDraw(startPoint, endPoint);

        }

        public void ProcessDraw(Cords currPoint)
        {

            DrawingArea.Children.Remove(previewElem);
            CordList.Add(currPoint);
            previewElem = previewElem = CurrentDraw(startPoint, currPoint);
            CordList.Remove(currPoint);

        }

       public void PolyProcessDraw(Cords currPoint)
        {
            if ( currBase != BaseClass.bcPoly) return;

            CordList.Add(currPoint);
            ProcessDraw(currPoint);

       }
        private object[] GetConstructorArgs(Cords c1, Cords c2)
        {
            var constructors = new Dictionary<BaseClass, Func<object[]>>
    {
        { BaseClass.bcRect,  () => new object[] { c1, c2 } },
        { BaseClass.bcCircle, () => new object[] { c1, c2, CountVert } },
        { BaseClass.bcPoly,   () => new object[] { CordList } }
    };

            if (!constructors.TryGetValue(currBase, out var getArgs))
            {
                throw new Exception("Unknown class");
            }

            return getArgs();
        }

        private System.Windows.Shapes.Shape CurrentDraw(Cords c1, Cords c2)
        {
            if (currConstructor == null)
            {
                throw new InvalidOperationException("Constructor is not set.");
            }

            var pen = new Pen() { Thickness = StrokeThickness, Brush = StrokeColorBrush };

            var args = GetConstructorArgs(c1, c2);

            var shape = (Shape)currConstructor.Invoke(args);

            return shape.Paint(DrawingArea, FillColorBrush, pen);
        }
        public void Clear()
        {
            DrawingArea.Children.Clear();
        }

        public Drawer(Canvas drawingArea)
        {
            DrawingArea = drawingArea;
        }

        public bool Undo()
        {
            MessageBox.Show("Undo");
            return true;
        }

        public bool Redo()
        {
            MessageBox.Show("Redo");
            return true;
        }

    }
}
