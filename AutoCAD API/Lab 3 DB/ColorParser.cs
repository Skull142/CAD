//ONDARZA ORTEGA JOAQUIN
//DISEÑO ASISTIDO POR COMPUTADORA
//SEMESTRE 2016-1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//usings de AutoCAD
//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Runtime;
//using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;


namespace AutoCADAPI.Lab3
{
    class ColorParser
    {
        public double width;
        public double height;
        public List<Color> colors;
        public ColorParser(string txtPath)
        {
            String[] lines = System.IO.File.ReadAllLines(txtPath);
            string[] rect = lines[0].Split(',');
            width = int.Parse(rect[0]);
            height = int.Parse(rect[1]);
            this.colors = new List<Color>();
            for(int i = 1 ; i < lines.Length; i++)
            {
                colors.Add( Line2Color(lines[i]) );
            }
        }
        public static Color Line2Color(string line)
        {
            string[] aux = line.Split(',');
            Color c = new Color();
            c= Color.FromRgb( byte.Parse(aux[0]), byte.Parse(aux[1]), byte.Parse(aux[2]) );
            return c;
        }
    }
}
