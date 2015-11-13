using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Tarea4
{
    public class Sierpinski
    {
        public int depth;

        public Polyline Triangle;

        public Point3d Center;

        public Point2d A, B, C;
        public Double Size;

        public Sierpinski(Point3d center, Double size = 100)
        {
            Triangle = new Polyline();
            this.Size = size;
            A = new Point2d(center.X - size / 2, center.Y);
            B = new Point2d(center.X, center.Y + size);
            C = new Point2d(center.X + size / 2, center.Y);
            Triangle.AddVertexAt(0, A, 0, 0, 0);
            Triangle.AddVertexAt(1, B, 0, 0, 0);
            Triangle.AddVertexAt(2, C, 0, 0, 0);
            Triangle.Closed = true;
            depth = 0;
        }
        public Sierpinski(Point2d a, Point2d b, Point2d c)
        {
            Triangle = new Polyline();
            A = a;
            B = b;
            C = c;
            this.Size = B.GetDistanceTo(A);
            Triangle.AddVertexAt(0, A, 0, 0, 0);
            Triangle.AddVertexAt(1, B, 0, 0, 0);
            Triangle.AddVertexAt(2, C, 0, 0, 0);
            Triangle.Closed = true;
            depth = 0;
        }

        public Sierpinski[] GetTriangles()
        {
            Point2d MidAB = MidPoint(A, B),
                    MidBC = MidPoint(B, C),
                    MidAC = MidPoint(A, C);
            Sierpinski t1 = new Sierpinski(A, MidAB, MidAC),
                       t2 = new Sierpinski(B, MidAB, MidBC),
                       t3 = new Sierpinski(C, MidAC, MidBC);
            t1.depth = depth + 1;
            t2.depth = depth + 1;
            t3.depth = depth + 1;
            return new Sierpinski[] { t1, t2, t3 };
        }


        public void Draw(Sierpinski t, ref List<Entity> ents, int maxDepth = 15)
        {
            if (t.depth > maxDepth)
                return;
            else
            {
                ents.Add(t.Triangle);
                foreach (Sierpinski tri in t.GetTriangles())
                    Draw(tri, ref ents, maxDepth);
            }
        }


        private Point3d Centroid(Point2d p, Point2d p0, Point2d p1)
        {
            return new Point3d((p.X + p0.X + p1.X) / 3, (p.Y + p0.Y + p1.Y) / 3, 0);
        }

        private Point2d MidPoint(Point2d p0, Point2d p1)
        {
            return new Point2d((p0.X + p1.X) / 2, (p0.Y + p1.Y) / 2);
        }
    }
}
