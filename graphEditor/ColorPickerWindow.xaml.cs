using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace graphicalEditor
{
    public partial class ColorPickerWindow : Window
    {
        public Color SelectedColor { get; private set; }

        public ColorPickerWindow(Color initialColor)
        {
            InitializeComponent();
            colorPicker.SelectedColor = initialColor;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (colorPicker.SelectedColor.HasValue)
            {
                SelectedColor = colorPicker.SelectedColor.Value;
                DialogResult = true;
            }
        }
    }
}
