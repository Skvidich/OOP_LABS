using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Newtonsoft.Json;

namespace graphiclaEditor
{
    class ShapeStorage
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.Indented
        };

        public void Serialise(List<Shape> shapes,string filePath)
        {
            string json = JsonConvert.SerializeObject(shapes, Settings);
            File.WriteAllText(filePath, json);
        }

        public List<Shape>?  Deserialise( string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Shape>>(json, Settings);
        }

        public ShapeStorage()
        {

        }
    }
}
