using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Tarea4
{
    public class Pixel
    {
        /// <summary> 
        /// El color del pixel 
        /// </summary> 
        public Color Color;
        /// <summary> 
        /// El tamaño del pixel 
        /// </summary> 
        public Double Size;
        /// <summary> 
        /// El centro del pixel 
        /// </summary> 
        public Point3d Center;
        /// <summary> 
        /// La geometría del pixel, la entidad que dibujaran 
        /// </summary> 
        public Face QUAD;
        /// <summary> 
        /// El constructor recibe la información del pixel, color, tamaño y  
        /// centro. Con esta información construye una entidad Face con cuatro  
        /// lados, la entidad Face funciona como un QUAD. 
        /// </summary> 
        public Pixel(Color color, Point3d center, Double size = 10)
        {
            this.Color = color;
            this.Center = center;
            this.Size = size;
            Point3d[] geo = new Point3d[]
            {
                new Point3d(center.X - size, center.Y - size, 0),  //0
                new Point3d(center.X + size, center.Y - size, 0),  //1
                new Point3d(center.X + size, center.Y + size, 0),  //2
                new Point3d(center.X - size, center.Y + size, 0),  //3
            };
            this.QUAD = new Face(geo[0], geo[1], geo[2], geo[3], true, true, true, true);
            this.QUAD.Color = color;
        }
    }
}
