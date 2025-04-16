using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using graphiclaEditor.Shapes;
using Newtonsoft.Json;


namespace graphiclaEditor
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class CircleBase : Shape
    {
        [JsonProperty]
        protected Cords Center;

        [JsonProperty]
        protected double Radius;

        [JsonProperty]
        protected int VertCount;
    }
}
