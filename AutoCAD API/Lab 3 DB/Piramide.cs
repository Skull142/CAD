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
using Autodesk.AutoCAD.Colors;
namespace AutoCADAPI.Lab3
{
    public class Piramide
    {
        public Point3d[] Geometry;

        public List<Face> Faces;

        public Color RandomColor
        {
            get
            {
                byte r, g, b;
                Random ran = new Random((int)DateTime.Now.Ticks);
                r = (byte)ran.Next(255);
                g = (byte)ran.Next(255);
                b = (byte)ran.Next(255);
                return Color.FromRgb(r, g, b);
            }
        }


        public Piramide(Point3d insPt, double size)
        {
            //1: Inicializamos las caras
            this.Faces = new List<Face>();
            //2: Definimos los puntos de la geometría
            this.Geometry = new Point3d[]
            {
                insPt,                                                      //E
                new Point3d(insPt.X + size, insPt.Y, 0),                    //D
                new Point3d(insPt.X + size, insPt.Y + size, 0),             //C
                new Point3d(insPt.X, insPt.Y + size, 0),                    //B
                new Point3d(insPt.X + size / 2, insPt.Y + size/ 2, size),   //A
            };
            //3: Crear caras
            this.Faces.Add(QUAD(0, 1, 2, 3, RandomColor));  //Base
            this.Faces.Add(Tri(0, 1, 4, RandomColor));  //Cara 1
            this.Faces.Add(Tri(1, 2, 4, RandomColor));  //Cara 2
            this.Faces.Add(Tri(2, 3, 4, RandomColor));  //Cara 3
            this.Faces.Add(Tri(3, 0, 4, RandomColor));  //Cara 4
        }

        public Face QUAD(int index0, int index1, int index2, int index3, Color col)
        {
            Face f = new Face(this.Geometry[index0],
                this.Geometry[index1],
                this.Geometry[index2],
                this.Geometry[index3], true, true, true, true);
            f.Color = col;
            return f;
        }

        public Face Tri(int index0, int index1, int index2, Color col)
        {
            Face f = new Face(this.Geometry[index0],
                this.Geometry[index1],
                this.Geometry[index2],
                true, true, true, true);
            f.Color = col;
            return f;
        }


    }
}
