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
        private Double d = 17;
        private float vMax = 60;
        private int dS = 10;
        public double dPromMin;
        public double dPromMax;
        public bool goal;
        public bool loopTravel;
        //
        private ObjectId line;
        public ObjectId mobile;
        //
        private Polyline ruta;
        private LineSegment2d segmentoActual;
        public int segmentoActualIndex;
        private int numeroSegmentos;
        //
        public BlockReference bloque;
        private Point3d bloqueCentro;
        //
        public Vector3d dir;
        public Vector3d velocity;
        private int pointActualCurve;
        public float velocityScale = 1f;
        private double angleRaycast = Math.PI / 4;

        public string Data
        {
            get
            {
                AttributeManager attribute = new AttributeManager(mobile);
                //return attribute.GetAttribute("ID").ToString() + ": " + velocity.ToString() + " [Kms/hr]";
                return this.bloque.Name + ":\t" + ( this.velocityScale < 0.001 ? "0":(this.velocity.Length*(this.vMax/this.d)).ToString("N02") ) + " [Km/hr]";
            }
        }
        public Movil(ref ObjectId line, ref ObjectId mobile, double minSeparation, double maxSeparation, bool loopTravel)
        {
            this.line = line;
            this.mobile = mobile;
            this.dPromMin = minSeparation;
            this.dPromMax = maxSeparation;
            this.loopTravel = loopTravel;
            this.goal = false;
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
            attribute.SetAttribute("Velocity", this.velocity+" [Kms/hr]");
            //
            this.pointActualCurve = 0;
            this.velocityScale = 0.00001f;
            this.velocity = this.UpdateDireccion();
            Lab3.DBMan.UpdateBlockRotation(new Vector2d(this.velocity.X, this.velocity.Y).Angle, this.mobile);
        }
        public void Move()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //
            if (this.goal)
                return;
            ApplyTransforms();
            //
            if (line.IsValid && mobile.IsValid)
            {
                if (bloque.Position.DistanceTo(new Point3d(segmentoActual.EndPoint.X, segmentoActual.EndPoint.Y, 0)) < 10)
                {
                    segmentoActualIndex++;
                    if (segmentoActualIndex + 1 > numeroSegmentos)
                    {
                        if (loopTravel)
                        {
                            pointActualCurve = 0;
                            segmentoActualIndex = 0;
                            this.goal = false;
                            //ed.WriteMessage("REINICIO!. {0} de {1}\n", segmentoActualIndex + 1, numeroSegmentos);
                            segmentoActual = this.ruta.GetLineSegment2dAt(segmentoActualIndex);
                            Lab3.DBMan.UpdateBlockPosition(new Point3d(this.segmentoActual.StartPoint.X, this.segmentoActual.StartPoint.Y, 0), mobile);
                            return;
                        }
                        else
                        {
                            this.goal = true;
                            return;
                        }
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
            
            //
            this.velocity = this.UpdateDireccion();
            //
            Lab3.DBMan.UpdateBlockRotation(new Vector2d(this.velocity.X, this.velocity.Y).Angle, this.mobile);
            if (this.ruta.GetBulgeAt(segmentoActualIndex) == 0)
            { 
                Matrix3d matrix = Matrix3d.Displacement( this.velocity * (this.velocityScale < 0.001 ? 0:1) );
                //ed.WriteMessage("{0}: {1}v\n", this.bloque.Name, this.velocityScale);
                Lab3.DBMan.Transform(matrix, mobile);
            }
        }

        public Vector3d UpdateDireccion()
        {
            Vector3d v;
            if (this.ruta.GetBulgeAt(segmentoActualIndex) == 0)
            {
                if (pointActualCurve > 0)
                    pointActualCurve = 0;
                v = new Vector3d(
                    segmentoActual.EndPoint.X - segmentoActual.StartPoint.X,
                    segmentoActual.EndPoint.Y - segmentoActual.StartPoint.Y,
                    0);
                v = v.MultiplyBy(1 / this.segmentoActual.Length);
                this.dir = v;
                v = v.MultiplyBy(this.d * this.velocityScale);
            }
            else
            {
                Point3d centroCurva = this.ruta.GetArcSegmentAt(segmentoActualIndex).Center;
                v = new Vector3d(
                   this.bloque.Position.X - centroCurva.X,
                   this.bloque.Position.Y - centroCurva.Y,
                   0);
                PointOnCurve3d[] pts = ruta.GetArcSegmentAt(segmentoActualIndex).GetSamplePoints((int)(this.d * this.dS));
                if (pointActualCurve < (int)(this.d * this.dS) && this.velocityScale >= 1f)
                {
                    Lab3.DBMan.UpdateBlockPosition(new Point3d(pts[pointActualCurve].Point.X, pts[pointActualCurve].Point.Y, 0), mobile);
                    v = ruta.GetArcSegmentAt(segmentoActualIndex).GetTangent(pts[pointActualCurve].Point).Direction.Negate();
                    pointActualCurve++;
                }
                else
                    v = ruta.GetArcSegmentAt(segmentoActualIndex).GetTangent(pts[pointActualCurve].Point).Direction.Negate();
                this.dir = v;
                v = v.MultiplyBy(this.d * (1 - (this.dS / 100f)) * this.velocityScale);
            }
            return v;
        }

        public void CheckVelocity( List<Movil> mobiles, List<Semaforo> trafficLights)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            velocityScale = 1f;
            this.CheckMobilesAround(mobiles);
            if (this.velocityScale < 0.001)
                return;
            this.CheckTrafficLightsAround(trafficLights);
        }
        private void CheckMobilesAround(List<Movil> mobiles)
        {
            float vsAux = 1f;
            foreach (Movil m in mobiles)
            {
                Vector3d vDir = m.bloque.Position - this.bloque.Position;
                if (vDir.Length < this.dPromMin && vDir.Length > 0)
                {
                    if (this.dir.GetAngleTo(vDir) < this.angleRaycast && this.dir.GetAngleTo(m.dir) < (Math.PI*3f)/4f)
                    {
                        if (vDir.Length <= this.dPromMax)
                            vsAux = 0.00001f;
                        else
                        {
                            if (vDir.Length <= (this.dPromMin - this.dPromMax) * 0.5f)
                                vsAux = 0.25f;
                            else
                            {
                                if (vDir.Length <= (this.dPromMin - this.dPromMax) * 0.8f)
                                    vsAux = 0.4f;
                            }
                        }
                    }
                }
                if (this.velocityScale > vsAux)
                    this.velocityScale = vsAux;
                if (this.velocityScale < 0.001)
                    return;
            }
        }
        private void CheckTrafficLightsAround(List<Semaforo> trafficLights)
        {
            float vsAux = 1f;
            foreach (Semaforo s in trafficLights)
            {
                Vector3d vDir = s.block.Position - this.bloque.Position;
                vDir = new Vector3d(vDir.X, vDir.Y, 0f);
                if (vDir.Length < this.dPromMin && vDir.Length > 0)
                {
                    if ( this.dir.GetAngleTo(vDir) < this.angleRaycast )
                    {
                        if (vDir.Length <= this.dPromMax)
                        {
                            if (s.state == EstadoSemaforo.alto)
                                vsAux = 0.00001f;
                            if (s.state == EstadoSemaforo.precaucion)
                                vsAux = 0.00001f;
                            if (s.state == EstadoSemaforo.siga)
                                vsAux = 1f;
                        }
                        else
                        {
                            if (vDir.Length <= (this.dPromMin - this.dPromMax))
                            {
                                if (s.state == EstadoSemaforo.alto)
                                    vsAux = 0.00001f;
                                if (s.state == EstadoSemaforo.precaucion)
                                    vsAux = 0.2f;
                                if (s.state == EstadoSemaforo.siga)
                                    vsAux = 1f;
                            }
                        }
                    }
                }
                if (this.velocityScale > vsAux)
                    this.velocityScale = vsAux;
                if (this.velocityScale < 0.001)
                    return;
            }
        }

        public void ChangeExternValues(double minSeparation, double maxSeparation, bool loopTravel)
        {
            this.dPromMin = minSeparation;
            this.dPromMax = maxSeparation;
            this.loopTravel = loopTravel;
        }
    }
}
