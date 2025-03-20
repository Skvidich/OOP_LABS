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


public enum BaseClass { bcRect, bcCircle, bcPoly };
public partial class MainWindow : Window
{
        
    // Classes info
    private Dictionary<string, ConstructorInfo> ButtonsToConstructors;
    private List<ConstructorInfo>? RectConstructors;
    private List<ConstructorInfo>? CircleConstructors;
    private List<ConstructorInfo>? PolyConstructors;

    // Drawing parameters
    private Drawer drawer;
    private bool isDrawing = false;

    private Serialiser serialiser;
    private PluginManager manager;
    public MainWindow()
    {
        
        InitializeComponent();

        drawer = new Drawer(DrawingArea);
        serialiser = new Serialiser();

        this.InitColorComboBox(this.cbFillColor, brush => drawer.FillColorBrush = brush);
        this.InitColorComboBox(this.cbStrokeColor, brush => drawer.StrokeColorBrush = brush);

        this.RectConstructors = InitShapesClasses(typeof(RectBase), new Type[] { typeof(Cords), typeof(Cords) });
        this.PolyConstructors = InitShapesClasses(typeof(PolyBase), new Type[] { typeof(List<Cords> )});
        this.CircleConstructors = InitShapesClasses(typeof(CircleBase), new Type[] { typeof(Cords), typeof(Cords), typeof(int) });

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
            Brushes.Purple,
            Brushes.White,
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
            drawer.currConstructor = this.ButtonsToConstructors[btn.Name];
            drawer.currBase = baseClass;
        };
    }

    // Drawing handlers
    private void StartDrawClick(object sender, MouseButtonEventArgs e)
    {
        
        if (!int.TryParse(tbVertCount.Text,out drawer.CountVert) || drawer.CountVert < 3 || drawer.CountVert > 10)
        {
            MessageBox.Show("Number of vertexes must be more than 3 and less than 10");
            return;
        }

        if (!double.TryParse(tbStrokeThickness.Text, out drawer.StrokeThickness) || drawer.StrokeThickness < 1 || drawer.StrokeThickness > 10)
        {
            MessageBox.Show("Stroke thickness must be more than 1 and less than 10");
            return;
        }

        Cords c1 = new Cords(e.GetPosition(DrawingArea));
        isDrawing = drawer.StartDraw(c1);

    }

    private void EndDrawClick(object sender, MouseButtonEventArgs e)
    {
        if (isDrawing == false) { return; };
        isDrawing = false;

        Cords endPoint = new Cords(e.GetPosition(DrawingArea));
        drawer.EndDraw(endPoint);
    }

    private void DrawMove(object sender, MouseEventArgs e)
    {
        if (!isDrawing  ) return;
       
        Cords currentPoint = new Cords(e.GetPosition(DrawingArea));
        drawer.ProcessDraw(currentPoint);

    }

    private void PolyDrawClick(object sender, MouseButtonEventArgs e)
    {
        if (!isDrawing ) return;

        Cords currPoint = new Cords(e.GetPosition(DrawingArea));
        drawer.PolyProcessDraw(currPoint);

    }

    private void ClearClick(object sender, RoutedEventArgs e)
    {
        drawer.Clear();

    }

    // Menu click handlers
    private void OpenClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Choose drawing"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            serialiser.Deserialise(openFileDialog.FileName);
        }
        
    }

    private void SaveClick(object sender, RoutedEventArgs e)
    {

        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save drawing"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            serialiser.Serialise(null, saveFileDialog.FileName);
        }
        
    }

     private void AddClick(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Choose plugin"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            manager.AddPlugin(openFileDialog.FileName);
        }
    }

     private void UndoClick(object sender, RoutedEventArgs e)
    {
        drawer.Undo();
    }

    private void RedoClick(object sender, RoutedEventArgs e)
    {
        drawer.Redo();
    }
}





