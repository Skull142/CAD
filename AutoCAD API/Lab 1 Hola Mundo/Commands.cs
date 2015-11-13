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

namespace AutoCADAPI.Lab1
{
    public class Commands
    {
        [CommandMethod("Hello")]
        public void HelloWorld()
        {
            //Acceden a la aplicación de manera estatica
            //Accedemos al Documento Actual, doc Dwg
            Document doc = 
                Application.DocumentManager.MdiActiveDocument;
            //El editor es la comunicación de AutoCAD con el 
            //usuario
            Editor ed = doc.Editor;
            ed.WriteMessage("Hola mundo {0}", DateTime.Now.Year);
        }
    }
}
