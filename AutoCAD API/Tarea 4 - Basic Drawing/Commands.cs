using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
namespace AutoCADAPI.Tarea4
{
    public class Commands
    {
        /// <summary>
        /// Dibuja la casita
        /// </summary>
        [CommandMethod("NCasita")]
        public void DrawCasita()
        {
            Point3d insPt;
            Double size;
            if (Lab2.Selector.Point("Punto de inserción", out insPt) &&
                Lab2.Selector.Double("El tamaño de la casita", out size))
            {
                Casita c = new Casita(size, insPt);
                Lab3.DBMan.Draw(c.Faces.ToArray());
            }
        }
        [CommandMethod("NParseo")]
        public void DrawImg()
        {
            const String PATH = @"C:\Users\Miguel\Documents\Visual Studio 2015\Projects\AutoCAD API\Tarea 4 - Basic Drawing\ima.txt";
            ColorParser cp = new ColorParser(PATH);
            Point3d insPt, c;
            Double size = 100;
            if (Lab2.Selector.Point("Punto de inserción", out insPt))
            {
                for (int x = 0, index = 0; x < cp.Width; x++)
                    for (int y = 0; y < cp.Height; y++)
                    {
                        c = new Point3d(insPt.X + size * x * 2, insPt.Y + size * (-2) * y, 0);
                        Lab3.DBMan.Draw(new Pixel(cp.Colors[index], c, size).QUAD);
                        index++;
                    }
            }
        }

        [CommandMethod("NSier")]
        public void DrawTriangle()
        {
            Point3d insPt;
            Double size;
            if (Lab2.Selector.Point("Punto de inserción", out insPt) &&
                Lab2.Selector.Double("El tamaño del triangulo", out size))
            {
                Sierpinski triangle = new Sierpinski(insPt, size);
                List<Entity> ents = new List<Entity>();
                triangle.Draw(triangle, ref ents, 5);
                Lab3.DBMan.Draw(ents.ToArray());
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage("Total de triangulos", ents.Count);

            }
        }
    }
}
