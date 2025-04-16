using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace graphiclaEditor.Shapes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShapeStyle
    {
        [JsonProperty]
        public string FillColor { get; set; }
        [JsonProperty]
        public string StrokeColor { get; set; }
        [JsonProperty]
        public double StrokeThickness { get; set; }

        [JsonIgnore]
        public Brush Fill => StringToBrush(FillColor);

        [JsonIgnore]
        public Pen StrokePen => new Pen(StringToBrush(StrokeColor), StrokeThickness);

        
        public ShapeStyle(Brush fill, Pen stroke)
        {
            if (fill is SolidColorBrush fillBrush)
                FillColor = fillBrush.Color.ToString();

            if (stroke?.Brush is SolidColorBrush strokeBrush)
                StrokeColor = strokeBrush.Color.ToString();

            StrokeThickness = stroke.Thickness ;
        }

        [JsonConstructor]
        public ShapeStyle(string fill, string stroke,double thickness)
        {

            FillColor = fill;
            StrokeColor = stroke;
            StrokeThickness = thickness;
        }


        private static Brush StringToBrush(string colorStr)
        {
            try
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorStr));
            }
            catch
            {
                return new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
