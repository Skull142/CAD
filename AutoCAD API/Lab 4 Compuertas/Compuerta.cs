using AutoCADAPI.Lab3;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab4
{
    public class Compuerta
    {
        BBox BoxInputA, BoxInputB, BoxOutput;
        /// <summary>
        /// El id de la compuerta
        /// </summary>
        public ObjectId Id;
        /// <summary>
        /// El nombre de la compuerta
        /// </summary>
        public String Name;

        /// <summary>
        /// El filtro de selección que permite seleccionar solo líneas
        /// </summary>
        public SelectionFilter LineFilter
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
        /// <summary>
        /// El filtro de selección que permite seleccionar solo textos
        /// </summary>
        public SelectionFilter TextFilter
        {
            get
            {
                TypedValue[] tps
                    = new TypedValue[]
                    {
                        //Usamos Start para filtrar por tipo de entidad
                        new TypedValue((int)DxfCode.Start,
                        RXClass.GetClass(typeof(DBText)).DxfName)
                    };
                return new SelectionFilter(tps);
            }
        }
        /// <summary>
        /// Crea una nueva compuerta
        /// </summary>
        /// <param name="blkRefId">El id de la referencia del bloque</param>
        public Compuerta(ObjectId blkRefId)
        {
            this.Id = blkRefId;
            this.Load();
        }
        /// <summary>
        /// Encuentra el valor de texto conectado por una línea conectada
        /// a la entrada A
        /// </summary>
        /// <returns>El valor del texto conectado a la entrada A</returns>
        public String FindTextInputA()
        {
            return FindTextInput(this.BoxInputA);
        }
        /// <summary>
        /// Encuentra el valor de texto conectado por una línea conectada
        /// a la entrada B
        /// </summary>
        /// <returns>El valor del texto conectado a la entrada B</returns>
        public String FindTextInputB()
        {
            return FindTextInput(this.BoxInputB);
        }
        /// <summary>
        /// Establece el resultado de las entradas
        /// </summary>
        public void Solve()
        {
            String inputA = FindTextInputA(),
                   inputB = FindTextInputB(),
                   output = String.Empty;
            if (inputA.Length == inputB.Length && inputA.Length == 1)
                output = this.GetOutput(inputA, inputB, this.Name) ? "1" : "0";
            else if (inputA.Length == inputB.Length)
            {
                string[] a = inputA.Split(','),
                         b = inputB.Split(',');
                for (int i = 0; i < a.Length; i++)
                    output += this.GetOutput(a[i], b[i], this.Name) ? "1" : "0";
            }
            else if (inputA.Length > inputB.Length && inputB.Length == 1)
            {
                string[] a = inputA.Split(',');
                for (int i = 0; i < a.Length; i++)
                    output += this.GetOutput(a[i], inputB, this.Name) ? "1" : "0";
            }
            else if (inputA.Length < inputB.Length && inputA.Length == 1)
            {
                string[] b = inputB.Split(',');
                for (int i = 0; i < b.Length; i++)
                    output += this.GetOutput(inputA, b[i], this.Name) ? "1" : "0";
            }
            else
            {
                string[] a = inputA.Split(','),
                         b = inputB.Split(',');
                for (int i = 0, j = 0; i < a.Length && j < b.Length; i++, j++)
                {
                    if (i < a.Length && j < b.Length)
                        output += this.GetOutput(a[i], b[j], this.Name) ? "1" : "0";
                    else if (i < a.Length && j > b.Length)
                        output += this.GetOutput(a[i], b[b.Length - 1], this.Name) ? "1" : "0";
                    else if (i < a.Length && j > b.Length)
                        output += this.GetOutput(a[a.Length - 1], b[j], this.Name) ? "1" : "0";
                }
            }
            AttributeManager attMan = new AttributeManager(this.Id);
            if (this.Name == "NOT")
                attMan.SetAttribute("INPUT", inputA);
            else
            {
                attMan.SetAttribute("INPUTA", inputA.Replace(",", ""));
                attMan.SetAttribute("INPUTB", inputB.Replace(",", ""));
            }
            attMan.SetAttribute("OUTPUT", output);
        }
        /// <summary>
        /// Encuentra el valor de texto conectado por una línea conectada
        /// alguna entrada
        /// </summary>
        /// <returns>El valor del texto conectado alguna entrada</returns>
        public String FindTextInput(BBox inputBox)
        {
            ObjectIdCollection ids;
            if (Lab2.Selector.ByBox(inputBox.Min, inputBox.Max, this.LineFilter, out ids))
            {
                Line l = Lab3.DBMan.OpenEnity(ids[0]) as Line;
                if (SelectByPoint(l.StartPoint, this.TextFilter, out ids))
                    return (Lab3.DBMan.OpenEnity(ids[0]) as DBText).TextString;
                else if (SelectByPoint(l.EndPoint, this.TextFilter, out ids))
                    return (Lab3.DBMan.OpenEnity(ids[0]) as DBText).TextString;
                else
                    return "0";
            }
            else
                return "0";
        }
        /// <summary>
        /// Un metodo de selección que busca un elemento por un punto
        /// </summary>
        /// <param name="center">El centro del punto a buscar</param>
        /// <param name="filter">El filtro de selección</param>
        /// <param name="objIds">Los ids encontrados en el área</param>
        /// <param name="crossing">Selección por crossing, otro caso window</param>
        /// <returns>El metodo de selección</returns>
        public Boolean SelectByPoint(Point3d center, SelectionFilter filter, out ObjectIdCollection objIds, Boolean crossing = true)
        {
            objIds = new ObjectIdCollection();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Polygon2D pol = new Polygon2D(15, center, 1);
            PromptSelectionResult res =
                crossing ? ed.SelectCrossingPolygon(pol.Geometry, filter) : ed.SelectWindowPolygon(pol.Geometry, filter);
            if (res.Status == PromptStatus.OK)
                objIds = new ObjectIdCollection(res.Value.GetObjectIds());
            return objIds.Count > 0;
        }
        /// <summary>
        /// Crea las cajas de colisión de la aplicación
        /// </summary>
        public void Load()
        {
            BlockReference blkRef = DBMan.OpenEnity(this.Id) as BlockReference;
            this.Name = blkRef.Name;
            Point3d min = blkRef.GeometricExtents.MinPoint,
                    max = blkRef.GeometricExtents.MaxPoint;
            BoxInputA = new BBox(new Point3d(min.X, (min.Y + max.Y) / 2, 0),
                                 new Point3d((min.X + max.X) / 2, max.Y, 0));
            BoxInputB = new BBox(min, new Point3d((min.X + max.X) / 2, (min.Y + max.Y) / 2, 0));
            BoxOutput = new BBox(new Point3d((min.X + max.X) / 2, min.Y, 0), max);
        }
        /// <summary>
        /// Calcula la salida con dos valores de entrada
        /// </summary>
        /// <param name="a">El valor de la primera entrada</param>
        /// <param name="b">El valor de la segunda entrada</param>
        /// <param name="blockName">El nombre del bloque</param>
        /// <returns>El valor de la salida</returns>
        public bool GetOutput(String a, String b, String blockName)
        {
            bool inputA = a == "1" ? true : false,
                 inputB = b == "1" ? true : false;
            if (blockName == "AND")
                return inputA && inputB;
            else if (blockName == "NAND")
                return !(inputA && inputB);
            else if (blockName == "OR")
                return inputA || inputB;
            else if (blockName == "NOR")
                return !(inputA || inputB);
            else if (blockName == "NOT")
                return !inputA;
            else if (blockName == "XOR")
                return inputA ^ inputB;
            else if (blockName == "XNOR")
                return !(inputA ^ inputB);
            else
                return false;
        }

    }
}
