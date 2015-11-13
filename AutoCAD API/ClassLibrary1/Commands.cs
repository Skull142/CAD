using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Ussings de AutoCad
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace AutoCadAPI.Lab1
{
    public class Commands
    {
        [CommandMethod("Hello")]
        public void HelloWorld()
        {
            //se accede al docuemnto actual
            Document doc = Application.DocumentManager.MdiActiveDocument;
            //se accede al editor. Es la comunicacion de AutoCad con el usuario
            Editor ed = doc.Editor;
            ed.WriteMessage("Hola Mundo! {0}",DateTime.Now.DayOfYear);

        }
    }
}
