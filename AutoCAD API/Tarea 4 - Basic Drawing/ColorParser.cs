using Autodesk.AutoCAD.Colors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Tarea4
{
    public class ColorParser
    {
        
        /// <summary>
        /// Aquí se guardan los colores que define el archivo  
        /// </summary> 
        public List<Color> Colors;
        /// <summary> 
        /// Aquí se guardan el ancho de la imagen 
        /// </summary> 
        public Double Width;
        /// <summary> 
        /// Aquí se guardan el alto de la imagen 
        /// </summary> 
        public Double Height;


        public ColorParser(String pth)
        {
            String[] lines = File.ReadAllLines(pth);
            String[] data = lines[0].Split(',');
            Height = Double.Parse(data[0]);
            Width = Double.Parse(data[1]);
            Colors = new List<Color>();
            for (int i = 1; i < lines.Length; i++)
            {
                data = lines[i].Split(',');
                if (data.Length == 3)
                    Colors.Add(Color.FromRgb(byte.Parse(data[0]), byte.Parse(data[1]), byte.Parse(data[2])));
            }
        }

    }
}
