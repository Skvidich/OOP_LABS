using graphiclaEditor.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace graphiclaEditor
{
    
    public abstract class Shape
    {
        [JsonProperty]
        protected ShapeStyle style;
        abstract public void Paint(Canvas canvas);
        abstract public void Paint(Canvas canvas,Brush newFill, Pen newStroke);

    }
}
