using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab2
{
    public class Filter
    {
        /// <summary>
        /// El filtro de selección que permite seleccionar solo líneas
        /// </summary>
        public static SelectionFilter FilterLine
        {
            get
            {
                TypedValue[] tps
                    = new TypedValue[]
                    {
                        //Usamos Start para filtrar por tipo de entidad
                        new TypedValue((int)DxfCode.Start,
                        RXClass.GetClass(typeof(Line)).DxfName)
                    };
                return new SelectionFilter(tps);
            }
        }


    }
}
