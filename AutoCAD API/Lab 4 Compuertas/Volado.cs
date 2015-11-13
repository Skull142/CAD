using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab4
{
    public class Volado
    {
        public ObjectIdCollection monedasIds;

        public Volado(int numMonedas, Point3d insPt)
        {
            monedasIds = new ObjectIdCollection();
            Circle c;
            Double x = 0, y = 0;
            Point3d center;
            for (int i = 0; i < numMonedas; i++)
            {
                if (i % 4 == 0 && i!=0)     //En múltiplos de 4 salta de línea
                {
                    y++;
                    x = 0;
                }
                //Seleccionar el centro
                if (i == 0)
                    center = insPt;
                else
                    center = new Point3d(insPt.X + x * 4, insPt.Y + y * 4, 0);
                x++;
                //Dibujar el circulo
                c = new Circle(center, Vector3d.ZAxis, 2);
                monedasIds.Add(Lab3.DBMan.Draw(c)[0]);
            }
        }


        public void AddData()
        {
            String[] caras
                = new String[]
                {
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila",
                    "Sol", "Aguila","Sol", "Aguila"
                };
            int mSize = caras.Length;
            Random r = new Random((int)DateTime.Now.Ticks);
            Lab3.DManager dman = new Lab3.DManager();
            foreach (ObjectId monedaId in this.monedasIds)
            {
                ObjectId dicMonedaId =
                dman.AddDictionary(monedaId, "Volado");
                ObjectId regCaraId =
                dman.AddXRecord(dicMonedaId, "Cara");
                dman.AddData(regCaraId, caras[r.Next(mSize - 1)]);
            }

        }


        public void Test()
        {
            Lab3.DManager dMan = new Lab3.DManager();
            string res;
            int aguila = 0, sol = 0;
            foreach (ObjectId monedaId in this.monedasIds)
            {
                res = dMan.Extract(dMan.Get(dMan.Get(monedaId, "Volado"), "Cara"))[0];
                if (res == "Sol")
                {
                    sol++;
                    Lab3.DBMan.UpdateColor(monedaId, Color.FromRgb(255, 0, 0));
                }
                else
                {
                    aguila++;
                    Lab3.DBMan.UpdateColor(monedaId, Color.FromRgb(0, 255, 0));
                }
            }
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("{0} {1}", aguila != sol ? "Gano" : "Empate",
                aguila > sol ? "Aguila" : sol > aguila ? "Sol" : "");
        }

    }
}
