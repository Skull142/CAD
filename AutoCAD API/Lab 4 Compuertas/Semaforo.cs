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
        public BlockReference block;
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
                    aux = "Alto";
                if (this.state == EstadoSemaforo.precaucion)
                    aux = "Precaución";
                if (this.state == EstadoSemaforo.siga)
                    aux = "Siga";
                return this.block.Name+":\t"+aux;
            }
        }
        public Semaforo(ref ObjectId id, int indexList, int changeStateLimit, int changeStateLimitPrecaution)
        {
            this.id = id;
            this.block = Lab3.DBMan.OpenEnity(id) as BlockReference;
            this.indexList = indexList;
            this.state = EstadoSemaforo.alto;
            this.changeStateLimit = changeStateLimit;
            this.changeStateLimitPrecaution = changeStateLimitPrecaution;
            this.count = 0;
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
        }

        public void ChangeExternValues( int changeStateLimit, int changeStateLimitPrecaution, double Zpos)
        {
            this.changeStateLimit = changeStateLimit;
            this.changeStateLimitPrecaution = changeStateLimitPrecaution;
            Lab3.DBMan.UpdateBlockPosition( new Point3d(this.block.Position.X, this.block.Position.Y,Zpos),this.id);
        }

    }
}
