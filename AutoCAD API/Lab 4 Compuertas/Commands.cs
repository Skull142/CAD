using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Geometry;
using System.IO;
using AutoCADAPI.Lab3;

namespace AutoCADAPI.Lab4
{
    public class Commands
    {
        PaletteSet mipaleta;
        Palette blockTab;
        BlockTab ctrl_blockTab;

        [CommandMethod("nGUI")]
        public void loadUI()
        {
            if (blockTab == null)
            {
                mipaleta = new PaletteSet("Evaluador de Semaforo");
                ctrl_blockTab = new BlockTab();
                blockTab = mipaleta.Add("Insertar", ctrl_blockTab);
                mipaleta.Visible = true;
            }
            else
                mipaleta.Visible = true;
        }

        [CommandMethod("SOLVECOMPUERTA")]
        public void Solve_Compuerta()
        {
            ObjectId refId;
            if (Lab2.Selector.ObjectId("Selecciona la compuerta", out refId))
            {
                Compuerta c = new Compuerta(refId);
                c.Solve();
            }
        }

        Double d = 10;
        ObjectId rutaId = new ObjectId();
        ObjectId movilId = new ObjectId();
        [CommandMethod("MoveItem")]
        public void MoveItem()
        {
            if (!rutaId.IsValid && !movilId.IsValid)
            {
                Lab2.Selector.ObjectId("Selecciona la ruta", "",
                    typeof(Line), out rutaId);
                Lab2.Selector.ObjectId("Selecciona el movil", "",
                    typeof(BlockReference), out movilId);
            }
            if (rutaId.IsValid && movilId.IsValid)
            {
                //Abrimos la geometría de la ruta
                Line ruta = Lab3.DBMan.OpenEnity(rutaId) as Line;
                BlockReference blkRef =
                    Lab3.DBMan.OpenEnity(movilId) as BlockReference;
                Point3d cent =
                    new Point3d((blkRef.GeometricExtents.MinPoint.X +
                                 blkRef.GeometricExtents.MaxPoint.X) / 2,
                                 (blkRef.GeometricExtents.MinPoint.Y +
                                 blkRef.GeometricExtents.MaxPoint.Y) / 2, 
                                 0);
                //Vector de movimiento
                Vector3d v = new Vector3d(
                    ruta.EndPoint.X - ruta.StartPoint.X,
                    ruta.EndPoint.Y - ruta.StartPoint.Y, 0);
                //Hago unitario a mi vector
                v = v.MultiplyBy(1 / ruta.Length);
                v = v.MultiplyBy(d);
                //Crear una matriz de transformación
                Matrix3d matrix = Matrix3d.Displacement(v);
                Matrix3d rotMatrix =
                    Matrix3d.Rotation(new Vector2d(v.X, v.Y).Angle, Vector3d.ZAxis, cent);
                //Realizo la transformación del vehiculo
                Lab3.DBMan.Transform(matrix, movilId);

                //Se acomada al segmento regresando la rotación al origen
                //Matrix3d rotMatrixOrigin =
                //    Matrix3d.Rotation(blkRef.Rotation * -1, Vector3d.ZAxis, cent);
                //Lab3.DBMan.Transform(rotMatrixOrigin, movilId);
                //Lab3.DBMan.Transform(rotMatrix, movilId);

                //Modo 2 Rotando el bloque a la dirección del vector de ruta.
                Lab3.DBMan.UpdateBlockRotation(new Vector2d(v.X, v.Y).Angle, movilId);

            }
        }

        Movil m;
        [CommandMethod("MoveOnPolyLine")]
        public void MobilPolyline()
        {
            if (!rutaId.IsValid && !movilId.IsValid)
                ConfigMovePolyLine();
            m.Move();
        }

        [CommandMethod("ResetMoveOnPolyLine")]
        public void ConfigMovePolyLine()
        {
            Lab2.Selector.ObjectId("Selecciona la ruta (Polyline)", "",
                   typeof(Polyline), out rutaId);
            Lab2.Selector.ObjectId("Selecciona el movil", "",
                typeof(BlockReference), out movilId);
            m = new Movil(ref rutaId, ref movilId);
        }

        [CommandMethod("MoveItem2")]
        public void MoveItem2()
        {
            ObjectId cId, lId;
            if (Lab2.Selector.ObjectId("Selecciona el movil", "", typeof(Circle), out cId) &&
                Lab2.Selector.ObjectId("Selecciona la ruta", "", typeof(Line), out lId))
            {
                Line l = Lab3.DBMan.OpenEnity(lId) as Line;
                Circle c = Lab3.DBMan.OpenEnity(cId) as Circle;
                Vector3d v = new Vector3d(l.EndPoint.X - l.StartPoint.X, l.EndPoint.Y - l.StartPoint.Y, 0);
                v = v.MultiplyBy(d / v.Length);
                Matrix3d m = Matrix3d.Displacement(v);
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
                using (Transaction tr = dwg.TransactionManager.StartTransaction())
                {
                    try
                    {
                        c = c.Id.GetObject(OpenMode.ForWrite) as Circle;
                        c.TransformBy(m);
                        tr.Commit();
                    }
                    catch (System.Exception exc)
                    {
                        ed.WriteMessage(exc.Message);
                        tr.Abort();
                    }
                }

            }
            d = 50;
        }

        Volado vol;
        [CommandMethod("InsertarVolado")]
        public void InsertVolado()
        {
            int num;
            Point3d pt;
            if (Lab2.Selector.Integer("Total de monedas", out num) &&
                Lab2.Selector.Point("Punto de inserción", out pt))
                vol = new Volado(num, pt);
        }

        [CommandMethod("JugarVolado")]
        public void JugarVolado()
        {
            if (vol != null)
            {
                vol.AddData();
                vol.Test();
            }
        }
        [CommandMethod("INSERTCOMPUERTA")]
        public void InsertCompuerta()
        {
            //validar la carga de la interfaz
            if (ctrl_blockTab == null)
                return;
            String pth = System.IO.Path.Combine(ctrl_blockTab.Directory_Path,
                this.ctrl_blockTab.Blockname);

            if (File.Exists(pth))
            {
                Lab3.BlockManager blkMan = new Lab3.BlockManager(pth);
                Point3d pt;
                if (Lab2.Selector.Point("Selecciona el punto de inserción", out pt))
                {
                    blkMan.Load();
                    ObjectId id = blkMan.Insert(pt);
                    AttributeManager attMan = new AttributeManager(id);

                    attMan.SetAttribute("InputA", "1,1,0,0");
                    attMan.SetAttribute("InputB", "1,0,1,0");
                    attMan.SetAttribute("OUTPUT", "1,0,0,0");
                    String strA = attMan.GetAttribute("InputA");
                }
            }
            else
            {

            }
        }





    }
}
