using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using AutoCADAPI.Lab3;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCADAPI.Lab4
{
    class Movil
    {
        Double d = 10;
        //
        ObjectId line;
        ObjectId mobile;
        //
        Polyline ruta;
        LineSegment2d segmentoActual;
        int segmentoActualIndex;
        int numeroSegmentos;
        //
        BlockReference bloque;
        Point3d bloqueCentro;


        public Movil(ref ObjectId line, ref ObjectId mobile)
        {
            this.line = line;
            this.mobile = mobile;
            this.ruta = Lab3.DBMan.OpenEnity(line) as Polyline;
            this.bloque = Lab3.DBMan.OpenEnity(mobile) as BlockReference;
            this.bloqueCentro = new Point3d((bloque.GeometricExtents.MinPoint.X +
                             bloque.GeometricExtents.MaxPoint.X) / 2,
                             (bloque.GeometricExtents.MinPoint.Y +
                             bloque.GeometricExtents.MaxPoint.Y) / 2,
                             0);
            numeroSegmentos = this.ruta.NumberOfVertices - 1;
            segmentoActualIndex = 0;
            segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
            Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);

        }
        public void Move()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //
            if (line.IsValid && mobile.IsValid)
            {
                if (bloque.Position.DistanceTo(new Point3d(segmentoActual.EndPoint.X, segmentoActual.EndPoint.Y, 0)) < 10)
                {
                    segmentoActualIndex++;
                    if (segmentoActualIndex + 1 > numeroSegmentos)
                    {
                        segmentoActualIndex = 0;
                        ed.WriteMessage("REINICIO!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                        segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
                        Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);
                        return;
                    }
                    else
                    {
                        ed.WriteMessage("iNCREMENTANDO!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                        segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
                        Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);
                    }
                    ed.WriteMessage("{0}", segmentoActualIndex);
                }
                else
                {
                    ed.WriteMessage("MOVIENDOSE!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                }
                ApplyTransforms();
                //
                //ed.WriteMessage("{0}\n", bloque.Position.DistanceTo(new Point3d(segmentoActual.EndPoint.X, segmentoActual.EndPoint.Y, 0)));

            }
        }

        private void ApplyTransforms()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Vector de movimiento
            Vector3d v;
            Point3d centroCurva;
            if (ruta.GetBulgeAt(segmentoActualIndex) == 0 )
            {
                v = new Vector3d(
                    segmentoActual.EndPoint.X - segmentoActual.StartPoint.X,
                    segmentoActual.EndPoint.Y - segmentoActual.StartPoint.Y,
                    0);
                ed.WriteMessage("1: {0}\n", v);
                //Hago unitario a mi vector
                v = v.MultiplyBy(1 / segmentoActual.Length);
                ed.WriteMessage("2: {0}\n", v);
                v = v.MultiplyBy(d);
                ed.WriteMessage("3: {0}\n", v);
            }
            else
            {
                centroCurva = ruta.GetArcSegmentAt(segmentoActualIndex).Center;
                v = new Vector3d(
                   bloque.Position.X - centroCurva.X,
                   bloque.Position.Y - centroCurva.Y,
                   0);
                v = v.GetPerpendicularVector();
                ed.WriteMessage("1: {0}\n", v);
                v = v.MultiplyBy(d*0.60f);
                ed.WriteMessage("2: {0}\n", v);
                //v = v.MultiplyBy(1/d);
            }           
            //Crear una matriz de transformación
            Matrix3d matrix = Matrix3d.Displacement(v);
            Matrix3d rotMatrix =
                Matrix3d.Rotation(new Vector2d(v.X, v.Y).Angle, Vector3d.ZAxis, bloqueCentro);

            //Modo 2 Rotando el bloque a la dirección del vector de ruta.
            Lab3.DBMan.UpdateBlockRotation(new Vector2d(v.X, v.Y).Angle, mobile);
            //Realizo la transformación del vehiculo
            Lab3.DBMan.Transform(matrix, mobile);
           
        }
        public void MoveCycle()
        {
            while( true )
            {
                Move();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
