using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace graphiclaEditor.Shapes
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class PolyBase : Shape
    {
        [JsonProperty]
        protected List<Point> points;
    }
}
