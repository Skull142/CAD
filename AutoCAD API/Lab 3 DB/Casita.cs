using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//usings de AutoCAD
//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;

namespace AutoCADAPI.Lab3
{
    class Casita
    {
        public Point3dCollection points;
        public List<Face> faces;
        public double size;
        public Point3d insPt;
        public List<Entity> ents;

        public Casita(Point3d insPt, double size)
        {
            this.insPt = insPt;
            this.size = size;
            points = new Point3dCollection();
            points.Add(new Point3d(insPt.X - size / 2, insPt.Y + size / 2, insPt.Z));
            points.Add(new Point3d(insPt.X + size / 2, insPt.Y + size / 2, insPt.Z));
            points.Add(new Point3d(insPt.X + size / 2, insPt.Y - size / 2, insPt.Z));
            points.Add(new Point3d(insPt.X - size / 2, insPt.Y - size / 2, insPt.Z));
            points.Add(new Point3d(insPt.X - size / 2, insPt.Y + size / 2, insPt.Z + size));
            points.Add(new Point3d(insPt.X + size / 2, insPt.Y + size / 2, insPt.Z + size));
            points.Add(new Point3d(insPt.X + size / 2, insPt.Y - size / 2, insPt.Z + size));
            points.Add(new Point3d(insPt.X - size / 2, insPt.Y - size / 2, insPt.Z + size));
            points.Add(new Point3d(insPt.X + size / 4, insPt.Y, insPt.Z + size + size / 3));
            points.Add(new Point3d(insPt.X - size / 4, insPt.Y, insPt.Z + size + size / 3));
            faces = new List<Face>();
            faces.Add(new Face(points[0], points[1], points[5], points[4], true, true, true, true));
            faces.Add(new Face(points[1], points[2], points[6], points[5], true, true, true, true));
            faces.Add(new Face(points[2], points[3], points[7], points[6], true, true, true, true));
            faces.Add(new Face(points[3], points[0], points[4], points[7], true, true, true, true));
            faces.Add(new Face(points[0], points[3], points[2], points[1], true, true, true, true));
            faces.Add(new Face(points[4], points[5], points[8], points[9], true, true, true, true));
            faces.Add(new Face(points[5], points[6], points[8], true, true, true, true));
            faces.Add(new Face(points[6], points[7], points[9], points[8], true, true, true, true));
            faces.Add(new Face(points[7], points[4], points[9], true, true, true, true));
            ents = new List<Entity>();
            foreach(Face f in faces)
            {
                f.Color = Color.FromRgb(255, 0, 0);
                ents.Add(f);
            }
            faces[5].Color = Color.FromRgb(0, 0, 255);
            faces[6].Color = Color.FromRgb(0, 0, 255);
            faces[7].Color = Color.FromRgb(0, 0, 255);
            faces[8].Color = Color.FromRgb(0, 0, 255);
        }
    }
}
