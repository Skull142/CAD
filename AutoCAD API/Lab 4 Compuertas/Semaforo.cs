using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using AutoCADAPI.Lab3;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;


namespace AutoCADAPI.Lab4
{
    public enum EstadoSemaforo
    {
        alto = -1,
        precaucion,
        siga
    }
    public class Semaforo
    {
        public EstadoSemaforo state;
        public ObjectId id;
        public ObjectId idIndicator;
        public BlockReference block;
        public BlockReference blockIndicator;
        public int indexList;
        private int changeStateLimit;
        private int changeStateLimitPrecaution;
        private int count;
        public string Data
        {
            get
            {
                string aux = "";
                if (this.state == EstadoSemaforo.alto)
                    aux = "Stop";
                if (this.state == EstadoSemaforo.precaucion)
                    aux = "Caution";
                if (this.state == EstadoSemaforo.siga)
                    aux = "Go";
                return this.block.Name+":\t"+aux;
            }
        }
        public Semaforo(ref ObjectId id, int indexList, int changeStateLimit, int changeStateLimitPrecaution, ref ObjectId idI)
        {
            this.id = id;
            this.idIndicator = idI;
            this.block = Lab3.DBMan.OpenEnity(id) as BlockReference;
            this.blockIndicator = Lab3.DBMan.OpenEnity(idIndicator) as BlockReference;
            this.indexList = indexList;
            this.state = EstadoSemaforo.alto;
            this.changeStateLimit = changeStateLimit;
            this.changeStateLimitPrecaution = changeStateLimitPrecaution;
            this.count = 0;
            Lab3.DBMan.UpdateBlockPosition( new Point3d(this.block.Position.X, this.block.Position.Y, this.block.Position.Z+100f), this.idIndicator);
            this.UpdateColor();
        }
        public Semaforo(int changeStateLimit)
        {
            this.state = EstadoSemaforo.alto;
            this.changeStateLimit = changeStateLimit;
            this.changeStateLimitPrecaution = (int)(changeStateLimit * 0.2f);
            this.count = 0;
        }

        public void Update()
        {
            this.count++;
            if (this.count >= this.changeStateLimitPrecaution && this.state.Equals(EstadoSemaforo.precaucion))
            {
                this.state = EstadoSemaforo.alto;
                this.count = 0;
            }
            if (this.count >= this.changeStateLimit && this.state.Equals(EstadoSemaforo.alto))
            {
                this.state = EstadoSemaforo.siga;
                this.count = 0;
            }
            if (this.count >= this.changeStateLimit && this.state.Equals(EstadoSemaforo.siga))
            {
                this.state = EstadoSemaforo.precaucion;
                this.count = 0;
            }
            this.UpdateColor();
        }
        public void UpdateColor()
        {
            Autodesk.AutoCAD.Colors.Color c = new Autodesk.AutoCAD.Colors.Color();
            if (this.state == EstadoSemaforo.siga)
                c = Autodesk.AutoCAD.Colors.Color.FromRgb((byte)0, (byte)255, (byte)0);
            if (this.state == EstadoSemaforo.alto)
                c = Autodesk.AutoCAD.Colors.Color.FromRgb((byte)255, (byte)0, (byte)0);
            if (this.state == EstadoSemaforo.precaucion)
                c = Autodesk.AutoCAD.Colors.Color.FromRgb((byte)255, (byte)255, (byte)0);
            Lab3.DBMan.UpdateColor(this.idIndicator, c);
        }
        public void ChangeExternValues( int changeStateLimit, int changeStateLimitPrecaution, double Zpos)
        {
            this.changeStateLimit = changeStateLimit;
            this.changeStateLimitPrecaution = changeStateLimitPrecaution;
            Lab3.DBMan.UpdateBlockPosition( new Point3d(this.block.Position.X, this.block.Position.Y,Zpos), this.id);
            Lab3.DBMan.UpdateBlockPosition(new Point3d(this.block.Position.X, this.block.Position.Y, this.block.Position.Z + 100f), this.idIndicator);
        }

    }
}
