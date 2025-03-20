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

        private System.Windows.Shapes.Shape CurrentDraw(Cords c1, Cords c2)
        {


            var pen = new Pen() { Thickness = StrokeThickness, Brush = StrokeColorBrush };
            Shape shape;
            switch (currBase)
            {
                case BaseClass.bcRect:
                    shape = (Shape)this.currConstructor.Invoke([c1, c2]);
                    break;
                case BaseClass.bcCircle:
                    shape = (Shape)this.currConstructor.Invoke([c1, c2, CountVert]);
                    break;
                case BaseClass.bcPoly:
                    shape = (Shape)this.currConstructor.Invoke([CordList]);
                    break;
                default:
                    throw new Exception("unknown class");
            }

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
