//ONDARZA ORTEGA JOAQUIN
//DISEÑO ASISTIDO POR COMPUTADORA
//SEMESTRE 2016-1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Ussings de AutoCad
//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

namespace AutoCADAPI.Lab3
{
    class Pixel
    {
        public Point3dCollection points;
        Color c;
        Point3d center;
        public double size;
        public Face face;
        public Pixel(Color c, Point3d center, Double size  = 10)
        {
            this.center = center;
            this.c = c;
            this.size = size;
            points = new Point3dCollection();
            points.Add(new Point3d(this.center.X - this.size / 2, this.center.Y - this.size / 2, this.center.Z));
            points.Add(new Point3d(this.center.X + this.size / 2, this.center.Y - this.size / 2, this.center.Z));
            points.Add(new Point3d(this.center.X + this.size / 2, this.center.Y + this.size / 2, this.center.Z));
            points.Add(new Point3d(this.center.X - this.size / 2, this.center.Y + this.size / 2, this.center.Z));
            this.face = new Face( points[0], points[1], points[2], points[3], true, true, true, true);
            this.face.Color = this.c;
        }

    }
}
