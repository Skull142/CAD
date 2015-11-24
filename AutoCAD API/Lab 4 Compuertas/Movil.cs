using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using AutoCADAPI.Lab3;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCADAPI.Lab4
{
    public class Movil
    {
        Double d = 10;
        private int dS = 25;
        //
        ObjectId line;
        public ObjectId mobile;
        //
        Polyline ruta;
        LineSegment2d segmentoActual;
        public int segmentoActualIndex;
        int numeroSegmentos;
        //
        public BlockReference bloque;
        Point3d bloqueCentro;
        //
        public Vector3d direccion;
        public Vector3d velocity;
        private int pointActualCurve;

        public string Data
        {
            get
            {
                AttributeManager attribute = new AttributeManager(mobile);
                //return attribute.GetAttribute("ID").ToString() + ": " + velocity.ToString() + " [Kms/hr]";
                return this.bloque.Name + ": " + this.velocity.Length.ToString("N") + " [Kms/hr]";
            }
        }
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
            this.numeroSegmentos = this.ruta.NumberOfVertices - 1;
            this.segmentoActualIndex = 0;
            this.segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
            Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);
            //
            Vector3d v = new Vector3d(
                    this.segmentoActual.EndPoint.X - this.segmentoActual.StartPoint.X,
                    this.segmentoActual.EndPoint.Y - this.segmentoActual.StartPoint.Y,
                    0);
            Lab3.DBMan.UpdateBlockRotation(new Vector2d(v.X, v.Y).Angle, mobile);
            direccion = v.MultiplyBy(1/this.segmentoActual.Length);
            //
            AttributeManager attribute = new AttributeManager(mobile);
            this.velocity = new Vector3d(0f, 0f, 0f);
            attribute.SetAttribute("Velocity", this.velocity+" [Kms/hr]");
            pointActualCurve = 0;
        }
        public void Move()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //
            ApplyTransforms();
            //
            if (line.IsValid && mobile.IsValid)
            {
                if (bloque.Position.DistanceTo(new Point3d(segmentoActual.EndPoint.X, segmentoActual.EndPoint.Y, 0)) < 10)
                {
                    segmentoActualIndex++;
                    if (segmentoActualIndex + 1 > numeroSegmentos)
                    {
                        pointActualCurve = 0;
                        segmentoActualIndex = 0;
                        //ed.WriteMessage("REINICIO!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                        segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
                        Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);
                        return;
                    }
                    else
                    {
                        //ed.WriteMessage("iNCREMENTANDO!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                        pointActualCurve = 0;
                        segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
                        Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);
                    }
                }
                //else
                //{
                    //ed.WriteMessage("MOVIENDOSE!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                //}
                //
            }
        }

        private void ApplyTransforms()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //ed.WriteMessage("{0}\n", this.direccion.ToString());
            Vector3d v;
            if (ruta.GetBulgeAt(segmentoActualIndex) == 0)
            {
                if (pointActualCurve > 0)
                    pointActualCurve = 0;
                v = new Vector3d(
                    segmentoActual.EndPoint.X - segmentoActual.StartPoint.X,
                    segmentoActual.EndPoint.Y - segmentoActual.StartPoint.Y,
                    0);
                v = v.MultiplyBy(1 / this.segmentoActual.Length);
                this.direccion = v;
                v = v.MultiplyBy(this.d);
            }
            else
            {
                Point3d centroCurva = ruta.GetArcSegmentAt(segmentoActualIndex).Center;
                v = new Vector3d(
                   this.bloque.Position.X - centroCurva.X,
                   this.bloque.Position.Y - centroCurva.Y,
                   0);
                PointOnCurve3d[] pts = ruta.GetArcSegmentAt(segmentoActualIndex).GetSamplePoints((int)this.d * this.dS);
                if (pointActualCurve < (int)this.d * this.dS)
                { 
                    Lab3.DBMan.UpdateBlockPosition(new Point3d(pts[pointActualCurve].Point.X, pts[pointActualCurve].Point.Y, 0), mobile);
                    v = ruta.GetArcSegmentAt(segmentoActualIndex).GetTangent(pts[pointActualCurve].Point).Direction.Negate();
                    pointActualCurve++;
                }
                this.direccion = v;
                v = v.MultiplyBy(this.d*(1-(this.dS/100f)));
            }
            this.velocity = v;
            //
            AttributeManager attribute = new AttributeManager(mobile);
            attribute.SetAttribute("Velocity", this.velocity.Length.ToString("N") + " [Kms/hr]");
            //ed.WriteMessage("{0}\n", attribute.GetAttribute("Velocity"));
            //
            Lab3.DBMan.UpdateBlockRotation(new Vector2d(v.X, v.Y).Angle, mobile);
            if (ruta.GetBulgeAt(segmentoActualIndex) == 0)
            { 
                Matrix3d matrix = Matrix3d.Displacement(v);
                Lab3.DBMan.Transform(matrix, mobile);
            }
        }
    }
}
