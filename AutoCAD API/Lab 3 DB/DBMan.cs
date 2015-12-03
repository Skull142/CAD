using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Colors;

namespace AutoCADAPI.Lab3
{
    public class DBMan
    {

        public static ObjectId _Line(Point3d pt0,
            Point3d ptf)
        {
            ObjectId id = new ObjectId();
            //Abrimos el documento activo
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Abrimos la BD
            Database dwg = doc.Database;
            //Se utiliza el using para cerrar la
            //transacción 
            //El Transaction Manager administra las transacciones y se
            //encuentra dentro de la BD
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    DBObject obj = dwg.BlockTableId.GetObject(OpenMode.ForRead);
                    //1: Abrir la tabla de bloques
                    BlockTable blkTab = (BlockTable)obj;
                    string modelSpaceName = BlockTableRecord.ModelSpace;
                    //2: Abrir el registro de bloque modelspace
                    //Se abre en modo escritura por que se dibujará en el
                    BlockTableRecord modelSpace =
                        (BlockTableRecord)blkTab[modelSpaceName].GetObject(OpenMode.ForWrite);
                    //3: Dibujar la línea
                    Line l = new Line(pt0, ptf);
                    l.Color = Color.FromRgb(255, 0, 0);
                    //4: Anexar al registro
                    modelSpace.AppendEntity(l);
                    //5: Notificar que se creo un nuevo objeto
                    tr.AddNewlyCreatedDBObject(l, true);
                    id = l.ObjectId;
                    tr.Commit();
                }
                catch (System.Exception exc)
                {
                    //Si algo sale mal aborta
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return id;
        }


        public static ObjectId DrawGeometry(Point3dCollection pts,
            Boolean close = true)
        {
            ObjectId id = new ObjectId();
            //Abrimos el documento activo
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Abrimos la BD
            Database dwg = doc.Database;
            //Se utiliza el using para cerrar la
            //transacción 
            //El Transaction Manager administra las transacciones y se
            //encuentra dentro de la BD
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    //1: Abre el espacio activo de la aplicación
                    BlockTableRecord currentSpace = (BlockTableRecord)
                        dwg.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                    //2: Dibujar la geometría como una polilínea
                    Polyline pl = new Polyline();
                    for (int i = 0; i < pts.Count; i++)
                        pl.AddVertexAt(i, pts[i].Convert2d(
                            new Plane(new Point3d(0, 0, 0), Vector3d.ZAxis)),
                            0, 0, 0);
                    pl.Closed = close;
                    pl.Color = Color.FromRgb(0, 255, 0);
                    //3: Anexar geometría al espacio
                    currentSpace.AppendEntity(pl);
                    tr.AddNewlyCreatedDBObject(pl, true);
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    //Si algo sale mal aborta
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return id;

        }

        public static void UpdateBlockRotation(double angle, ObjectId blkId)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using(Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockReference blk = (BlockReference)
                        blkId.GetObject(OpenMode.ForWrite);
                    blk.Rotation = angle;
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }
        public static void Erase(ObjectId id)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockReference blk = (BlockReference)
                       id.GetObject(OpenMode.ForWrite);
                    blk.Erase();
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }
        public static void UpdateBlockPosition(Point3d point, ObjectId blkId)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockReference blk = (BlockReference)
                        blkId.GetObject(OpenMode.ForWrite);
                    blk.Position = point;
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }

        public static void Transform(Matrix3d matrix, ObjectId entId)
        {
            
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    Entity ent = entId.GetObject(OpenMode.ForWrite) as Entity;
                                             
                    ent.TransformBy(matrix);
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }
        
        public static void UpdateColor(ObjectId entId, Color color)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    Entity ent = entId.GetObject(OpenMode.ForWrite) as Entity;
                    ent.Color = color;
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }

        public static Entity OpenEnity(ObjectId id)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            //La variable de tipo entidad
            Entity ent = null;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    //Abrir entidad
                    ent = id.GetObject(OpenMode.ForRead) as Entity;
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            //el valor de retorno
            return ent;
        }

        public static ObjectIdCollection Draw(params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            //Abrimos el documento activo
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Abrimos la BD
            Database dwg = doc.Database;
            //Se utiliza el using para cerrar la
            //transacción 
            //El Transaction Manager administra las transacciones y se
            //encuentra dentro de la BD
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTableRecord currentSpace = (BlockTableRecord)
                        dwg.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                    foreach (Entity ent in ents)
                    {
                        //Se borra un elemento
                        //Id.GetObject(OpenMode.ForWrite).Erase()
                        currentSpace.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                        ids.Add(ent.Id);
                    }
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    //Si algo sale mal aborta
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return ids;
        }


        public static ObjectIdCollection Draw(String block, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            //Abrimos el documento activo
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Abrimos la BD
            Database dwg = doc.Database;
            //Se utiliza el using para cerrar la
            //transacción 
            //El Transaction Manager administra las transacciones y se
            //encuentra dentro de la BD
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    //1: Abrir tabla de bloques en modo lectura
                    BlockTable blkTab = (BlockTable)
                        dwg.BlockTableId.GetObject(OpenMode.ForRead);
                    //2: Checar si existe mi bloque
                    BlockTableRecord myRecord;
                    if (!blkTab.Has(block))
                    {
                        //3: Upgrade Open cambia un objeto de lectura 
                        //a escritura
                        blkTab.UpgradeOpen();
                        BlockTableRecord newRecord = new BlockTableRecord();
                        newRecord.Name = block;
                        //4: Agregar a la tabla y notificar
                        blkTab.Add(newRecord);
                        tr.AddNewlyCreatedDBObject(newRecord, true);
                        myRecord = newRecord;
                    }
                    else
                    {
                        //3: Solo lo abro
                        myRecord = (BlockTableRecord)
                            blkTab[block].GetObject(OpenMode.ForWrite);
                    }
                    //Dibujar en myRecord
                    foreach (Entity ent in ents)
                    {
                        myRecord.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                        ids.Add(ent.Id);
                    }
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    //Si algo sale mal aborta
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return ids;
        }


        public static BlockReference GetReference(
            String blockName, Point3d insPt, double size)
        {
            ObjectId id = new ObjectId();
            #region Transaccción para obtener Id
            //Abrimos el documento activo
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Abrimos la BD
            Database dwg = doc.Database;
            //Se utiliza el using para cerrar la
            //transacción 
            //El Transaction Manager administra las transacciones y se
            //encuentra dentro de la BD
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable blkTab = (BlockTable)
                         dwg.BlockTableId.GetObject(OpenMode.ForRead);
                    if (blkTab.Has(blockName))
                        id = blkTab[blockName];
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    //Si algo sale mal aborta
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            #endregion
            if (id.IsValid)
            {
                BlockReference blkRef = new BlockReference(insPt, id);
                blkRef.ScaleFactors = new Scale3d(size);
                return blkRef;
            }
            else
                return null;

        }
    }
}
