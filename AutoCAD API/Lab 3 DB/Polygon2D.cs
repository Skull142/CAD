using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab3
{
    public class Polygon2D
    {
        public int Sides;
        public Point3d Center;
        public Double Apotema;
        public Point3dCollection Geometry;

        public Polyline Poly
        {
            get
            {
                Polyline pl = new Polyline();
                Point2d pt;
                for (int i = 0; i < this.Geometry.Count; i++)
                {
                    pt = new Point2d(this.Geometry[i].X, this.Geometry[i].Y);
                    pl.AddVertexAt(i, pt, 0, 0, 0);
                }
                return pl;
            }
        }

        public Polygon2D(int s, Point3d c, Double a)
        {
            this.Sides = s;
            this.Center = c;
            this.Apotema = a;
            CreateGeometry();
        }

        public void CreateGeometry()
        {
            Geometry = new Point3dCollection();
            Double delta = Math.PI * 2 / Sides;
            Double x, y;

            for (int i = 0; i < Sides; i++)
            {
                x = this.Center.X + this.Apotema * Math.Cos(delta * i);
                y = this.Center.Y + this.Apotema * Math.Sin(delta * i);
                Geometry.Add(new Point3d(x, y, 0));
            }

        }
    }
}
