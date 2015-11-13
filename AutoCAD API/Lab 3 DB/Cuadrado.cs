using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab3
{
    public class Cuadrado
    {
        public int Depth;
        Point3dCollection pts;
        public Cuadrado(int Depth, Point3d pt0, Point3d pt1,
            Point3d pt2, Point3d pt3)
        {
            this.Depth = Depth;
            pts = new Point3dCollection();
            pts.Add(pt0);
            pts.Add(pt1);
            pts.Add(pt2);
            pts.Add(pt3);
            Lab3.DBMan.DrawGeometry(pts, true);
        }

        public Cuadrado GetQuarter()
        {
            Double size = pts[0].DistanceTo(pts[1]) / 4;
            Point3d pt0 = pts[0],
                    pt1 = new Point3d(pts[0].X + size, pts[0].Y, 0),
                    pt2 = new Point3d(pt1.X, pt1.Y + size, 0),
                    pt3 = new Point3d(pts[0].X , pts[0].Y + size, 0);

            return
                new Cuadrado(Depth + 1, pt0, pt1, pt2, pt3);
        }

        public void Draw(int maxDepth = 3)
        {
            Cuadrado c = this;
            while (c.Depth < maxDepth)
                c = c.GetQuarter();
        }

        public Point3d GetQuarterPoint(Point3d pt0, Point3d ptf)
        {
            return new Point3d((pt0.X + ptf.X) / 4, (pt0.Y + ptf.Y) / 4, 0);
        }

    }
}
