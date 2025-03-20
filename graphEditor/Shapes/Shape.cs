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
        protected Brush fill;
        protected Pen stroke;

        public Brush Fill{ 
            get
            {
                return fill;
            } set
            {
                fill = value;
            }
        }

        public Pen Stroke
        {
            get
            {
                return stroke;
            }
            set
            {
                stroke = value;
            }
        }
        abstract public System.Windows.Shapes.Shape Paint(Canvas canvas);
        abstract public System.Windows.Shapes.Shape Paint(Canvas canvas,Brush newFill, Pen newStroke);

    }
}
