using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab3
{
    public class BBox
    {
        public Point3d Min;
        public Point3d Max;

        public Point3dCollection Geometry;

        public BBox(Point3d min, Point3d max)
        {
            this.Min = min;
            this.Max = max;
            this.Geometry = new Point3dCollection();
            this.Geometry.Add(min);
            this.Geometry.Add(new Point3d(max.X, min.Y, 0));
            this.Geometry.Add(max);
            this.Geometry.Add(new Point3d(min.X, max.Y, 0));
        }


    }
}
