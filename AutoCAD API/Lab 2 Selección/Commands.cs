using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
namespace AutoCADAPI.Lab2
{
    public class Commands
    {
        [CommandMethod("NPoint")]
        public void PrintPoint()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Point3d pt = Selector.Point();
                ed.WriteMessage("\nP: ({0:N3},{1:N3},{2:N3})",
                    pt.X, pt.Y, pt.Z);
            }
            catch (System.Exception exc)
            {
                ed.WriteMessage(exc.Message);
            }
        }

        [CommandMethod("NDistance")]
        public void PrintDistance()
        {
            Point3d pt0, ptf;
            if (Selector.Point("\nSelecciona el punto inicial", out pt0)
                && Selector.Point("\nSelecciona el punto final", pt0, out ptf))
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("\nD: {0:N3}", pt0.DistanceTo(ptf));
            }
        }

        [CommandMethod("NPoints")]
        public void PrintPoints()
        {
            Point3dCollection pts;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            if (Selector.Point("\nSelecciona un punto", out pts))
                foreach (Point3d pt in pts)
                    ed.WriteMessage("\nP: ({0:N3},{1:N3},{2:N3})",
                        pt.X, pt.Y, pt.Z);
        }

        [CommandMethod("NEntity")]
        public void PrintId()
        {
            ObjectId id;
            if (Selector.ObjectId("\nSelecciona una entidad", out id))
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("\n" + id.ToString());
            }
        }

        [CommandMethod("NSelPoly")]
        public void PrintIdPoly()
        {
            ObjectId id;
            if (Selector.ObjectId("\nSelecciona una poli-línea",
                "\nNo es poli-línea", typeof(Polyline), out id))
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("\n" + id.ToString());
            }
        }


    }
}
