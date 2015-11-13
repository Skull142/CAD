//ONDARZA ORTEGA JOAQUIN
//DISEÑO ASISTIDO POR COMPUTADORA
//SEMESTRE 2016-1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;


namespace AutoCADAPI.Lab3
{
    class Sierpinski
    {
        Point3d center;
        float rad;
        Point3dCollection points;
        List<Entity> ents;
        double deg2rad = Math.PI / 180f;

        public Sierpinski(Point3d center, float rad)
        {
            this.center = center;
            this.rad = rad;
            points = new Point3dCollection();
            ents = new List<Entity>();
            points.Add( new Point3d(center.X + (rad * Math.Cos(90 * deg2rad)),  center.Y + (rad * Math.Sin(90 * deg2rad)),  center.Z));
            points.Add( new Point3d(center.X + (rad * Math.Cos(210 * deg2rad)), center.Y + (rad * Math.Sin(210 * deg2rad)), center.Z));
            points.Add( new Point3d(center.X + (rad * Math.Cos(330 * deg2rad)), center.Y + (rad * Math.Sin(330 * deg2rad)), center.Z));
        }
        public ObjectIdCollection Draw(int deep)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            //abrir el docuemnto activo
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //abrimos la base de datos
            Database dwg = doc.Database;

            //Manejar Transacciones
            //se utiliza el using para cerrar la transaccion 
            //El 
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    //Abre el espacio activo de la aplicacion
                    BlockTableRecord currentSpace = (BlockTableRecord)dwg.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                    Draw(this.points, deep, ref this.ents);
                    foreach (Entity ent in ents)
                    {
                        currentSpace.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                        ids.Add(ent.Id);
                    }

                    //to do
                    tr.Commit();
                }
                catch (System.Exception exc)
                {
                    //si algo sale mal aborta 
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
                ed.WriteMessage("Número de Triangulos Dibujados: "+Math.Pow(3,deep));
                return ids;
            }
        }
        public static void Draw(Point3dCollection points, int deep, ref List<Entity> ents)
        {
            if (deep == 0)
            { 
                DrawPolygon(points, ref ents);
                return;
            }
            else
            {
                Point3dCollection middlePoints = CalculateMiddlePoints(points);
                Point3dCollection aux = new Point3dCollection();
                aux.Add( points[0] );
                aux.Add( middlePoints[0] );
                aux.Add( middlePoints[2] );
                Draw(aux, deep-1, ref ents);
                //
                aux.Clear();
                aux = new Point3dCollection();
                aux.Add( middlePoints[0] );
                aux.Add( points[1] );
                aux.Add( middlePoints[1]);
                Draw(aux, deep - 1, ref ents);
                //
                aux.Clear();
                aux = new Point3dCollection();
                aux.Add( middlePoints[2]);
                aux.Add( middlePoints[1] );
                aux.Add( points[2]);
                Draw(aux, deep - 1, ref ents);
            }
        }
        public static void DrawTriangule(Point3dCollection points, ref List<Entity> ents)
        {
            Polyline pl = new Polyline();
            for (int i = 0; i < points.Count; i++)
                pl.AddVertexAt(i, points[i].Convert2d(new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis)), 0, 0, 0);
            pl.Color = Color.FromRgb(255, 0, 0);
            pl.Closed = true;
            ents.Add(pl);
        }
        public static void DrawPolygon(Point3dCollection points, ref List<Entity> ents)
        {
            Face f = new Face(points[0] , points[1], points[2], true, true, true, true);
            f.Color = Color.FromRgb(0, 255, 0);
            ents.Add(f);
        }

        public static Point3dCollection CalculateMiddlePoints(Point3dCollection basePoints)
        {
            Point3dCollection auxC= new Point3dCollection();
            auxC.Add( PointBetweenPoints(basePoints[0], basePoints[1]) );
            auxC.Add( PointBetweenPoints(basePoints[1], basePoints[2]) );
            auxC.Add( PointBetweenPoints(basePoints[2], basePoints[0]) );
            return auxC;
        }
        public static Point3d PointBetweenPoints(Point3d a, Point3d b)
        {
            return new Point3d((a.X + b.X) / 2, (a.Y + b.Y) / 2, (a.Z + a.Z) / 2);
        }


    }

}
