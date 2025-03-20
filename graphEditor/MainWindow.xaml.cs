using graphiclaEditor.Shapes;
using Microsoft.Win32;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace graphiclaEditor;



public partial class MainWindow : Window
{
        
    // Classes info
    private Dictionary<string, ConstructorInfo> ButtonsToConstructors;
    private List<ConstructorInfo>? RectConstructors;
    private List<ConstructorInfo>? CircleConstructors;
    private List<ConstructorInfo>? PolyConstructors;

    enum BaseClass  {bcRect,bcCircle,bcPoly};
    // Drawing settings
    private int CountVert;
    private Double StrokeThickness;
    private Brush StrokeColorBrush = Brushes.Black;
    private Brush FillColorBrush = Brushes.Black;

    // Drawing parameters
    private System.Windows.Shapes.Shape previewElem;
    private ConstructorInfo? сurrConstructor;
    private BaseClass currBase = BaseClass.bcRect;
    private bool isDrawing = false;
    private Point startPoint;
    private List<Cords> CordList;

    public MainWindow()
    {
        
        

        InitializeComponent();

        this.InitColorComboBox(this.cbFillColor, brush => this.FillColorBrush = brush);
        this.InitColorComboBox(this.cbStrokeColor, brush => this.StrokeColorBrush = brush);

        this.RectConstructors = InitShapesClasses(typeof(RectBase), new Type[] { typeof(Cords), typeof(Cords) });
        this.PolyConstructors = InitShapesClasses(typeof(PolyBase), new Type[] { typeof(List<Cords> )});
        this.CircleConstructors = InitShapesClasses(typeof(CircleBase), new Type[] { typeof(Cords), typeof(Cords),typeof(int) });

        this.сurrConstructor = null;

        this.ButtonsToConstructors = new Dictionary<string, ConstructorInfo>();
        this.AddButtons(this.spRectButtons, this.RectConstructors, BaseClass.bcRect);
        this.AddButtons(this.spCircleButtons, this.CircleConstructors, BaseClass.bcCircle);
        this.AddButtons(this.spPolyButtons, this.PolyConstructors, BaseClass.bcPoly);

    }

    // Initialization
    private void InitColorComboBox(ComboBox cb, Action<SolidColorBrush> updateBrushAction)
    {
        cb.ItemsSource = new List<SolidColorBrush>
        {
            Brushes.Black,
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.Purple
        };

        cb.SelectionChanged += (sender, e) =>
        {
            if (cb.SelectedItem is SolidColorBrush selectedBrush)
            {

                updateBrushAction(selectedBrush);
            }
        };
    }

    private List<ConstructorInfo>? InitShapesClasses(Type baseClass, Type[] constrArgs)
    {
        var assembly = Assembly.GetAssembly(baseClass);
        if (assembly == null)
        {
            return null;
        }

        var subTypes = assembly.GetTypes().Where(t => t != null && t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseClass));

        List<ConstructorInfo> constructors = new List<ConstructorInfo>();
        foreach (Type type in subTypes)
        {
            var constr = type.GetConstructor(constrArgs);
            constructors.Add(constr);
        }
        return constructors;
    }



    private void AddButtons(StackPanel stackPanel, List<ConstructorInfo>? constructors,BaseClass baseClass)
    {
        if (constructors == null) { return; }

        int ButtonSize = 20;

        foreach (ConstructorInfo constructor in constructors)
        {
                Button btn = new Button();
                btn.Height = ButtonSize;
                btn.Width = ButtonSize*5;
                btn.HorizontalAlignment = HorizontalAlignment.Center;
                btn.Name = "btn" + constructor.DeclaringType.Name;
                btn.Content = constructor.DeclaringType.Name;

                this.ButtonsToConstructors.Add(btn.Name, constructor);
                SetButtonClick(btn, baseClass);

                stackPanel.Children.Add(btn);
            
        }  

    }

    private void SetButtonClick(Button btn,BaseClass baseClass)
    {
        btn.Click += (sender, e) =>
        {
            this.сurrConstructor = this.ButtonsToConstructors[btn.Name];
            this.currBase = baseClass;
        };
    }

    // Drawing
    private void StartDraw(object sender, MouseButtonEventArgs e)
    {
        if (this.сurrConstructor== null) { return; }
        if (!int.TryParse(tbVertCount.Text,out CountVert) || CountVert < 3 || CountVert > 10)
        {
            MessageBox.Show("Number of vertexes must be more than 3 and less than 10");
            return;
        }

        if (!double.TryParse(tbStrokeThickness.Text, out StrokeThickness) || StrokeThickness < 1 || StrokeThickness > 10)
        {
            MessageBox.Show("Stroke thickness must be more than 1 and less than 10");
            return;
        }

        isDrawing = true;
        startPoint = e.GetPosition(DrawingArea);

        Cords c1 = new Cords(startPoint);

        CordList = new List<Cords>();
        CordList.Add(c1);
        previewElem = CurrentDraw(c1, c1);

    }

    private void EndDraw(object sender, MouseButtonEventArgs e)
    {
        if (isDrawing == false) { return; };
        isDrawing = false;

        Cords c1 = new Cords(startPoint);
        Cords c2 = new Cords(e.GetPosition(DrawingArea));

        DrawingArea.Children.Remove(previewElem);
        previewElem = CurrentDraw(c1, c2);

       

    }

    private void ProcessDraw(object sender, MouseEventArgs e)
    {
        if (!isDrawing  ) return;
       
        Point currentPoint = e.GetPosition(DrawingArea);
        Cords c1 = new Cords(startPoint);
        Cords c2 = new Cords(e.GetPosition(DrawingArea));
  
        DrawingArea.Children.Remove(previewElem);
        CordList.Add(c2);
        previewElem = previewElem = CurrentDraw(c1, c2);
        CordList.Remove(c2);

    }

    private void PolyProcessDraw(object sender, MouseButtonEventArgs e)
    {
        if (!isDrawing || currBase != BaseClass.bcPoly) return;

        Cords c = new Cords(e.GetPosition(DrawingArea));

        CordList.Add(c);
        ProcessDraw(sender, e);

    }



    private System.Windows.Shapes.Shape CurrentDraw(Cords c1, Cords c2)
    {
        
        
        var pen = new Pen() { Thickness = StrokeThickness, Brush = StrokeColorBrush };
        Shape shape ; 
        switch (currBase)
        {
            case BaseClass.bcRect:
                shape = (Shape)this.сurrConstructor.Invoke([c1,c2]);
                break;
            case BaseClass.bcCircle:
                shape = (Shape)this.сurrConstructor.Invoke([c1, c2,CountVert]);
                break;
            case BaseClass.bcPoly:
                shape = (Shape)this.сurrConstructor.Invoke([CordList]);
                break;
            default:
                shape = (Shape)this.сurrConstructor.Invoke([c1, c2]);
                break;
        }
        
        return shape.Paint(DrawingArea, FillColorBrush, pen);
    }

    private void ClearDrawArea(object sender, RoutedEventArgs e)
    {
        DrawingArea.Children.Clear();

    }

    private void OpenDrawing(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Choose drawing"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            MessageBox.Show("Opened");
        }
    }

    private void SaveDrawing(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save drawing"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            MessageBox.Show("Saved");
        }
    }


    private void AddPlugin(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Choose plugin"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            MessageBox.Show("Plugin added");
        }
    }

    private void Undo(object sender,RoutedEventArgs e)
    {
        MessageBox.Show("Undo");
    }

    private void Redo(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Undo");
    }
}





