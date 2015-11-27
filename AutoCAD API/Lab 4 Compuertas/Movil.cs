using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using AutoCADAPI.Lab3;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCADAPI.Lab4
{
    public class Movil
    {
        Double d = 10;
        private float vMax = 50;
        int dS = 25;
        double dPromMin = 600f;
        double dPromMax = 250f;
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
        public Vector3d dir;
        public Vector3d velocity;
        private int pointActualCurve;
        public float velocityScale = 1f;

        public string Data
        {
            get
            {
                AttributeManager attribute = new AttributeManager(mobile);
                //return attribute.GetAttribute("ID").ToString() + ": " + velocity.ToString() + " [Kms/hr]";
                return this.bloque.Name + ": " + ( this.velocityScale < 0.001 ? "0":(this.velocity.Length*(this.vMax/this.d)).ToString("N") ) + " [Km/hr]";
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
            AttributeManager attribute = new AttributeManager(mobile);
            this.velocity = new Vector3d(0f, 0f, 0f);
            attribute.SetAttribute("Velocity", this.velocity+" [Kms/hr]");
            //
            this.pointActualCurve = 0;
            this.Move();
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
                this.dir = v;
                v = v.MultiplyBy( this.d * this.velocityScale);
            }
            else
            {
                Point3d centroCurva = ruta.GetArcSegmentAt(segmentoActualIndex).Center;
                v = new Vector3d(
                   this.bloque.Position.X - centroCurva.X,
                   this.bloque.Position.Y - centroCurva.Y,
                   0);
                PointOnCurve3d[] pts = ruta.GetArcSegmentAt(segmentoActualIndex).GetSamplePoints( (int)(this.d * this.dS) );
                if (pointActualCurve < (int)(this.d * this.dS) && this.velocityScale >= 1f)
                { 
                    Lab3.DBMan.UpdateBlockPosition(new Point3d(pts[pointActualCurve].Point.X, pts[pointActualCurve].Point.Y, 0), mobile);
                    v = ruta.GetArcSegmentAt(segmentoActualIndex).GetTangent(pts[pointActualCurve].Point).Direction.Negate();
                    pointActualCurve++;
                }
                else
                    v = ruta.GetArcSegmentAt(segmentoActualIndex).GetTangent(pts[pointActualCurve].Point).Direction.Negate();
                this.dir = v;
                v = v.MultiplyBy( this.d*(1-(this.dS/100f)) * this.velocityScale);
            }
            //
            this.velocity = v;
            //
            Lab3.DBMan.UpdateBlockRotation(new Vector2d(v.X, v.Y).Angle, mobile);
            if (ruta.GetBulgeAt(segmentoActualIndex) == 0)
            { 
                Matrix3d matrix = Matrix3d.Displacement( v );
                //ed.WriteMessage("{0}: {1}v\n", this.bloque.Name, this.velocityScale);
                Lab3.DBMan.Transform(matrix, mobile);
            }
        }

        public void MobilesAround( List<Movil> mobiles )
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            velocityScale = 1f;
            float vsAux = 1f; 
            foreach (Movil m in mobiles)
            {
                Vector3d vDir = m.bloque.Position - this.bloque.Position;
                if (vDir.Length < dPromMin && vDir.Length > 0)
                {
                    if (this.dir.GetAngleTo(vDir) < (Math.PI / 3))
                    {
                        if (vDir.Length <= this.dPromMax)
                            vsAux = 0.00001f;
                        else
                        {
                            if (vDir.Length <= (this.dPromMin - this.dPromMax)*0.5f)
                                vsAux = 0.25f;
                            else
                            {
                                if (vDir.Length <= (this.dPromMin - this.dPromMax) * 0.8f)
                                    vsAux = 0.4f;
                            }
                        }
                        ed.WriteMessage("{0}:{1}° {2}vM {3}vS\n", this.bloque.Name, (this.dir.GetAngleTo(vDir) * (180 / Math.PI)).ToString("N"), vDir.Length.ToString("N"), this.velocityScale);
                    }
                }
                if (this.velocityScale > vsAux)
                    this.velocityScale = vsAux;
            }
        }
    }
}
