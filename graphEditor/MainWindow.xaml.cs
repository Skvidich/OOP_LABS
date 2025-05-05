using graphicalEditor;
using graphiclaEditor.Shapes;
using Microsoft.Win32;

using System.Reflection;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;


namespace graphiclaEditor;



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

    private ShapeStorage serialiser;
    private PluginManager manager;
    public MainWindow()
    {
        
        InitializeComponent();

        drawer = new Drawer(DrawingArea);
        serialiser = new ShapeStorage();
        


        this.RectConstructors = InitShapesClasses(typeof(RectBase), new Type[] { typeof(Cords), typeof(Cords) });
        this.PolyConstructors = InitShapesClasses(typeof(PolyBase), new Type[] { typeof(List<Cords> )});
        this.CircleConstructors = InitShapesClasses(typeof(CircleBase), new Type[] { typeof(Cords), typeof(Cords), typeof(int) });

        manager = new PluginManager(
        ref RectConstructors,
        ref CircleConstructors,
        ref PolyConstructors,
        RefreshButtons
        );

        this.ButtonsToConstructors = new Dictionary<string, ConstructorInfo>();
        this.AddButtons(this.spRectButtons, this.RectConstructors, BaseClass.bcRect);
        this.AddButtons(this.spCircleButtons, this.CircleConstructors, BaseClass.bcCircle);
        this.AddButtons(this.spPolyButtons, this.PolyConstructors, BaseClass.bcPoly);

    }

    // Initialization
    private void ChooseFillColor(object sender, RoutedEventArgs e)
    {
        var dlg = new ColorPickerWindow(((SolidColorBrush)drawer.FillColorBrush).Color);
        if (dlg.ShowDialog() == true)
        {
            drawer.FillColorBrush = new SolidColorBrush(dlg.SelectedColor);
        }
    }

    private void ChooStrokeColor(object sender, RoutedEventArgs e)
    {
        var dlg = new ColorPickerWindow(((SolidColorBrush)drawer.StrokeColorBrush).Color);
        if (dlg.ShowDialog() == true)
        {
            drawer.StrokeColorBrush = new SolidColorBrush(dlg.SelectedColor);
        }
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
            if (constr != null)
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
                btn.Name = "btn" + constructor.DeclaringType.FullName.Replace(".", "_");
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
            if (this.ButtonsToConstructors.TryGetValue(btn.Name, out var constructor))
            {
                drawer.CurrConstructor = constructor;
                drawer.CurrBase = baseClass;
            }
        };
    }

    // Drawing handlers
    private void StartDrawClick(object sender, MouseButtonEventArgs e)
    {
        int countVert;
        
        if (!int.TryParse(tbVertCount.Text,out countVert) || countVert < 3 || countVert > 10)
        {
            
            MessageBox.Show("Number of vertexes must be more than 3 and less than 10");
            return;
        }
        drawer.CountVert = countVert;
        double thickness;
        if (!double.TryParse(tbStrokeThickness.Text, out thickness) || thickness < 1 || thickness > 10)
        {
            
            MessageBox.Show("Stroke thickness must be more than 1 and less than 10");
            return;
        }
        drawer.StrokeThickness = thickness;
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
            var temp = serialiser.Deserialise(openFileDialog.FileName);
            if (temp != null){
                drawer.ShapeList = temp;
            }
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
            serialiser.Serialise(drawer.ShapeList, saveFileDialog.FileName);
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
        if (!drawer.Undo())
        {
            MessageBox.Show("Nothing to undo");
        }
        ;
    }

    private void RedoClick(object sender, RoutedEventArgs e)
    {
        if (!drawer.Redo())
        {
            MessageBox.Show("Nothing to redo");
        }
        ;
    }

    private void RefreshButtons()
    {
        spRectButtons.Children.Clear();
        spCircleButtons.Children.Clear();
        spPolyButtons.Children.Clear();
        ButtonsToConstructors.Clear();


        AddButtons(spRectButtons, RectConstructors, BaseClass.bcRect);
        AddButtons(spCircleButtons, CircleConstructors, BaseClass.bcCircle);
        AddButtons(spPolyButtons, PolyConstructors, BaseClass.bcPoly);
    }

}





