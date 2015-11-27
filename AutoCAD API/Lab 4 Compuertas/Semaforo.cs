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
                return aux;
            }
        }
        public Semaforo(ref ObjectId id, int changeStateLimit, int changeStateLimitPrecaution)
        {
            this.id = id;
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
            if (this.count >= this.changeStateLimitPrecaution && this.state == EstadoSemaforo.precaucion)
                this.state = EstadoSemaforo.alto;
            else
            {
                if (this.count >= this.changeStateLimit)
                {
                    this.count = 0;
                    if (this.state == EstadoSemaforo.alto)
                        this.state = EstadoSemaforo.siga;
                    if (this.state == EstadoSemaforo.siga)
                        this.state = EstadoSemaforo.precaucion;
                }
            }
        }

    }
}
