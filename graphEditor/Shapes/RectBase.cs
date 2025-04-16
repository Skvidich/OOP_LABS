using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphiclaEditor.Shapes
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class RectBase : Shape
    {
        [JsonProperty]
        protected Cords LeftTop;
        [JsonProperty]
        protected Cords RightBot;

    }
}
