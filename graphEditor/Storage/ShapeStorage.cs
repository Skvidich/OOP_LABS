using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace graphiclaEditor
{
    class ShapeStorage
    {

        public void Serialise(List<Shape> shapes,string filePath)
        {
            MessageBox.Show("Serialised");
        }

        public List<Shape>  Deserialise( string filePath)
        {
            MessageBox.Show("Deserialised");
            return null;
        }

        public ShapeStorage()
        {

        }
    }
}
