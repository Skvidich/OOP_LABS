using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace graphiclaEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{
    bool isDrawing;
    Point startPoint;
    public enum ShapeType { stLine, stRectangle,stElipse,stRightPolygon,stPolyline,stPolygon }

    ShapeType currType;

    List<Cords> CordList;

    int CountVert;
    Double StrokeThickness;
    private System.Windows.Shapes.Shape previewElem;

    Brush StrokeColorBrush;
    Brush FillColorBrush;
    public MainWindow()
    {
        isDrawing = false;
        CountVert = 4;
        StrokeColorBrush = Brushes.Black;
        FillColorBrush = Brushes.White;
        StrokeThickness = 1;
        currType = ShapeType.stLine;

        

        InitializeComponent();

        cbFillColor.ItemsSource = new List<SolidColorBrush>
        {
            Brushes.White,
            Brushes.Black,
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.Purple
        };


        cbFillColor.SelectionChanged += (sender, e) =>
        {
            if (cbFillColor.SelectedItem is SolidColorBrush selectedBrush)
            {

                FillColorBrush = selectedBrush;
            }
        };

        cbStrokeColor.ItemsSource = new List<SolidColorBrush>
        {
            Brushes.Black,
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.Purple
        };


        cbStrokeColor.SelectionChanged += (sender, e) =>
        {
            if (cbStrokeColor.SelectedItem is SolidColorBrush selectedBrush)
            {

                StrokeColorBrush = selectedBrush;
            }
        };
    }



    

    private void StartDraw(object sender, MouseButtonEventArgs e)
    {
        isDrawing = true;
        startPoint = e.GetPosition(DrawingArea);


        Cords c1 = new Cords(startPoint);

        CordList = new List<Cords>();
        CordList.Add(c1);
        previewElem = CurrentDraw(c1, c1);


    }

    private void EndDraw(object sender, MouseButtonEventArgs e)
    {
        isDrawing = false;

        Cords c1 = new Cords(startPoint);
        Cords c2 = new Cords(e.GetPosition(DrawingArea));

        previewElem = CurrentDraw(c1, c2);
        

    }

    private void ProcessDraw(object sender, MouseEventArgs e)
    {
        if (!isDrawing ) return;

       
        Point currentPoint = e.GetPosition(DrawingArea);
        Cords c1 = new Cords(startPoint);
        Cords c2 = new Cords(e.GetPosition(DrawingArea));
  
        DrawingArea.Children.Remove(previewElem);
        previewElem = previewElem = CurrentDraw(c1, c2);


    }

    private void PolyProcessDraw(object sender, MouseButtonEventArgs e)
    {
        if (!isDrawing) return;


        
        Cords c = new Cords(e.GetPosition(DrawingArea));

        CordList.Add(c);
        ProcessDraw(sender, e);

    }



    private System.Windows.Shapes.Shape CurrentDraw(Cords c1, Cords c2)
    {
        StrokeThickness = Int32.Parse(tbStrokeThickness.Text);
        var pen = new Pen() { Thickness = StrokeThickness, Brush = StrokeColorBrush };
        switch (currType)
        {
            case ShapeType.stLine:
                var line = new Line(c1, c2);
                return line.Paint(DrawingArea, FillColorBrush, pen);
            case ShapeType.stRectangle:
                var rect = new Rectangale(c1, c2);
                return rect.Paint(DrawingArea, FillColorBrush, pen);
            case ShapeType.stElipse:
                var ellps = new Ellipse(c1, c2);
                return ellps.Paint(DrawingArea, FillColorBrush, pen);
            case ShapeType.stRightPolygon:
                var rigPol = new RightPolygon(c1, c2,CountVert);
                return rigPol.Paint(DrawingArea, FillColorBrush, pen);

            case ShapeType.stPolyline:
                
                var plLine = new Polyline(CordList);
                return plLine.Paint(DrawingArea, FillColorBrush, pen);
            case ShapeType.stPolygon:
                
                var polyg = new Polygon(CordList);
                return polyg.Paint(DrawingArea, FillColorBrush, pen);
            default:
                return null;

        }
    }

    private void ClearDrawArea(object sender, RoutedEventArgs e)
    {
        DrawingArea.Children.Clear();

    }

    private void ChooseShape(object sender, RoutedEventArgs e)
    {
      
        if (sender is Button button)
        {
            switch (button.Name)
            {
                case "btnLine":
                    currType = ShapeType.stLine;
                    break;

                case "btnRightPolygon":
                    CountVert = Int32.Parse(tbVertCount.Text);
                    currType = ShapeType.stRightPolygon;
                    break;

                case "btnElipse":
                    currType = ShapeType.stElipse;
                    break;

                case "btnRectangle":
                    currType = ShapeType.stRectangle;
                    break;

                case "btnPolyline":
                    currType = ShapeType.stPolyline;
                    break;

                case "btnPolygon":
                    
                    currType = ShapeType.stPolygon;
                    break;

                default:
                    MessageBox.Show("Неизвестный тип фигуры");
                    return;
            }

            
        }
    }
}





