using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Colors;
using System.IO;

namespace AutoCADAPI.Lab3
{
    public class Commands
    {
        [CommandMethod("NLine")]
        public void DrawLine()
        {
            Point3d pt0, ptf;
            if (Lab2.Selector.Point("Selecciona el punto inicial\n", out pt0) &&
                Lab2.Selector.Point("Selecciona el punto final\n", out ptf))
            {
                DBMan._Line(pt0, ptf);
            }
        }

        [CommandMethod("NPolygon")]
        public void DrawPolygon()
        {
            int sides;
            Point3d center, end;
            Double ap;
            if (Lab2.Selector.Integer("\nDame el número de lados", out sides) &&
               Lab2.Selector.Point("\nSelecciona el centro", out center) &&
               Lab2.Selector.Point("\nSelecciona el tamaño del poligono",
               center, out end))
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ap = center.DistanceTo(end);
                if (sides < 3)
                    ed.WriteMessage("\nEl número mínimo de lados es tres.");
                else
                {
                    Polygon2D pl = new Polygon2D(sides, center, ap);
                    DBMan.DrawGeometry(pl.Geometry);
                }
            }
        }

        [CommandMethod("NDraw")]
        public void DrawEnts()
        {
            Circle c = new Circle(new Point3d(), Vector3d.ZAxis, 20);
            Line l = new Line(new Point3d(), new Point3d(20, 20, 0));
            Face quad = new Face(
                new Point3d(10, 10, 0),
                new Point3d(10, 10, 10),
                new Point3d(20, 10, 10),
                new Point3d(20, 20, 0), true, true, true, true);
            Face tri = new Face(
                new Point3d(20, 20, 20),
                new Point3d(40, 40, 20),
                new Point3d(25, 20, 25), true, true, true, true);
            c.Color = Color.FromRgb(100, 255, 0);
            //Polyline, Spline, Cube, Sphere

            DBMan.Draw(c, l, quad);
            DBMan.Draw(tri);
        }

        [CommandMethod("NPiramide")]
        public void DrawPiramide()
        {
            Point3d insPt, endPt;
            Double size;
            if (Lab2.Selector.Point("\nPunto de inserción", out insPt)
               && Lab2.Selector.Point("\nTamaño piramide", insPt, out endPt))
            {
                size = insPt.DistanceTo(endPt);
                Piramide p = new Piramide(insPt, size);
                Lab3.DBMan.Draw(p.Faces.ToArray());
            }
        }

        [CommandMethod("CreateBlock")]
        public void CreateBlock()
        {
            Polygon2D pl = new Polygon2D(5, new Point3d(0, 0, 0), 10);
            Polyline pol = pl.Poly;
            pol.Closed = true;
            Lab3.DBMan.Draw("Pentagono", pol);
        }

        [CommandMethod("NInsertBlock")]
        public void InsertBlock()
        {
            Point3d insPt;
            if (Lab2.Selector.Point("\nPunto de inserción", out insPt))
            {
                BlockReference blk = DBMan.GetReference("Pentagono", insPt, 1);
                if (blk != null)
                    DBMan.Draw(blk);
            }
        }

        [CommandMethod("NCuadrado")]
        public void Cuadrado()
        {
            Point3d insPt;
            if (Lab2.Selector.Point("\nPunto de inserción", out insPt))
            {
                Cuadrado c = new Lab3.Cuadrado(0,
                    new Point3d(insPt.X - 100, insPt.Y - 100, 0),
                    new Point3d(insPt.X + 100, insPt.Y - 100, 0),
                    new Point3d(insPt.X + 100, insPt.Y + 100, 0),
                    new Point3d(insPt.X - 100, insPt.Y + 100, 0));
                c.Draw(5);
            }
        }

        [CommandMethod("NInsertAND")]
        public void InsertAnd()
        {
            Point3d insPt;
            if (Lab2.Selector.Point("\nPunto de inserción", out insPt))
            {
                String dllPath =
                System.Reflection.Assembly.GetAssembly(typeof(Commands)).Location;
                String dir = dllPath.Substring(0, dllPath.LastIndexOf('\\'));
                string file = Path.Combine(dir, "Bloques", "AND.dwg");
                BlockManager blkMan;
                blkMan = new BlockManager(file);
                try
                {
                    blkMan.Load();
                    ObjectId refId = blkMan.Insert(insPt);
                    blkMan.CreateBBox(refId);
                    //Dibujar las cajas
                    DBMan.DrawGeometry(blkMan.Box.Geometry, true);
                    DBMan.DrawGeometry(blkMan.BoxInputA.Geometry, true);
                    DBMan.DrawGeometry(blkMan.BoxInputB.Geometry, true);
                    DBMan.DrawGeometry(blkMan.BoxOutput.Geometry, true);
                }
                catch (System.Exception e)
                {
                    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    ed.WriteMessage("\n", e.Message);
                }
            }
        }

        ObjectId blkId = new ObjectId(), 
                 entId= new ObjectId();

        [CommandMethod("Test")]
        public void TestColl()
        {
            if (!blkId.IsValid)
                Lab2.Selector.ObjectId("Selecciona el bloque", out blkId);
            if(!entId.IsValid)
                Lab2.Selector.ObjectId("Selecciona la entidad a validar", out entId);
            BlockManager.Find(blkId, entId);
        }


    }
}
