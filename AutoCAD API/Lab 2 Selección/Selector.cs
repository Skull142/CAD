using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCADAPI.Lab2
{
    public class Selector
    {
        public static Point3d Point()
        {
            Point3d pt = new Point3d();
            //1: Definir opciones de selección
            PromptPointOptions opt =
                new PromptPointOptions("\nSelecciona un punto.");
            opt.AllowNone = false;
            //2: Obtener el resultado de selección
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            PromptPointResult res = ed.GetPoint(opt);
            if (res.Status == PromptStatus.OK)
                pt = res.Value;
            else
                throw new Exception("Error al seleccionar un punto");
            return pt;
        }


        public static Boolean ByBox(Point3d min, Point3d max, out ObjectIdCollection objIds,
                                    Boolean crossing = true)
        {
            Boolean flag = false;
            objIds = new ObjectIdCollection();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult res =
                crossing ? ed.SelectCrossingWindow(min, max) : ed.SelectWindow(min, max);
            if (res.Status == PromptStatus.OK)
            {
                objIds = new ObjectIdCollection(res.Value.GetObjectIds());
                flag = objIds.Count > 0;
            }
            return flag;
        }

        public static Boolean ByBox(Point3d min, Point3d max, SelectionFilter filter,
                                    out ObjectIdCollection objIds,
                                    Boolean crossing = true)
        {
            Boolean flag = false;
            objIds = new ObjectIdCollection();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult res =
                crossing ? ed.SelectCrossingWindow(min, max, filter) : ed.SelectWindow(min, max,filter);
            if (res.Status == PromptStatus.OK)
            {
                objIds = new ObjectIdCollection(res.Value.GetObjectIds());
                flag = objIds.Count > 0;
            }
            return flag;
        }


        public static Boolean Point(String msg, out Point3d pt)
        {
            //1: Definir valor de retorno
            Boolean flag = false;
            pt = new Point3d();
            //2: Definir opciones de selección
            PromptPointOptions opt = new PromptPointOptions(msg);
            //3: Seleccionar el resultado del punto
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointResult res = ed.GetPoint(opt);
            if (res.Status == PromptStatus.OK)
            {
                pt = res.Value;
                flag = true;
            }
            return flag;
        }
        /// <summary>
        /// Selecciona un punto, permitiendo definir si permite la
        /// seleccion nula o no desde un punto base
        /// </summary>
        /// <param name="msg">El mensaje de selección</param>
        /// <param name="basePt">El punto base para seleccionar un punto</param>
        /// <param name="pt">El punto seleccionado</param>
        /// <param name="allowNone">Verdadero si se permite la selección nula</param>
        /// <returns>Verdadero si se realiza la selección</returns>
        public static Boolean Point(String msg, Point3d basePt,
            out Point3d pt, Boolean allowNone = false)
        {
            //1: Definir los valores de retorno
            Boolean flag = false;
            pt = new Point3d();
            //2: Definir las opciones de selección
            PromptPointOptions opt = new PromptPointOptions(msg);
            opt.AllowNone = allowNone;
            opt.BasePoint = basePt;
            opt.UseBasePoint = true;
            opt.UseDashedLine = true;
            //3: Obtener el resultado
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptPointResult res = ed.GetPoint(opt);
            if (res.Status == PromptStatus.OK)
            {
                pt = res.Value;
                flag = true;
            }
            return flag;
        }



        public static Boolean Point(String msg, out Point3dCollection pts)
        {
            pts = new Point3dCollection();
            Point3d pt;
            //Agrego el primer punto de la colección
            if (Point(msg, out pt))
            {
                pts.Add(pt);
                //Realizamos una selección continua
                while (Point("", pt, out pt, true))
                    pts.Add(pt);
            }
            return pts.Count > 0;
        }

        /// <summary>
        /// Seleccionar solo numeros mayores a 0
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Boolean Integer(String msg, out int n)
        {
            Boolean flag = false;
            n = 0;
            PromptIntegerOptions opt = new PromptIntegerOptions(msg);
            opt.AllowNegative = false;
            opt.AllowZero = false;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptIntegerResult res = ed.GetInteger(opt);
            if (res.Status == PromptStatus.OK)
            {
                n = res.Value;
                flag = true;
            }
            return flag;
        }
        public static Boolean Double(String msg, out double d)
        {
            Boolean flag = false;
            d = 0;
            PromptDoubleOptions opt = new PromptDoubleOptions(msg);
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptDoubleResult res = ed.GetDouble(opt);
            if (res.Status == PromptStatus.OK)
            {
                d = res.Value;
                flag = true;
            }
            return flag;
        }
        /// <summary>
        /// No se permiten cadenas vacias
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Boolean String(String msg, out String s)
        {
            Boolean flag = false;
            s = "";
            PromptStringOptions opt = new PromptStringOptions(msg);
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult res = ed.GetString(opt);
            if (res.Status == PromptStatus.OK)
            {
                s = res.StringResult;
                flag = s != System.String.Empty;
            }
            return flag;
        }

        public static Boolean ObjectId(String msg, out ObjectId id)
        {
            Boolean flag = false;
            id = new ObjectId();
            PromptEntityOptions opt = new PromptEntityOptions(msg);
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityResult res = ed.GetEntity(opt);
            if (res.Status == PromptStatus.OK)
            {
                id = res.ObjectId;
                flag = true;
            }
            return flag;
        }

        public static bool ObjectId(string msg, string rejMsg,
            Type tp, out ObjectId id)
        {
            Boolean flag = false;
            id = new ObjectId();
            PromptEntityOptions opt = new PromptEntityOptions(msg);
            opt.SetRejectMessage(rejMsg);
            opt.AddAllowedClass(tp, true);
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityResult res = ed.GetEntity(opt);
            if (res.Status == PromptStatus.OK)
            {
                id = res.ObjectId;
                flag = true;
            }
            return flag;
        }

    }
}
