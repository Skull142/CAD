using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Lab3
{
    public class BlockManager
    {
        /// <summary>
        /// El archivo del bloque
        /// </summary>
        public FileInfo BlockFile;
        /// <summary>
        /// El nombre del registro de bloque en AutoCAD
        /// </summary>
        public String Blockname;

        /// <summary>
        /// Inicializa el manejador de bloques
        /// </summary>
        /// <param name="pth">La ruta del archivo de bloque a insertar</param>
        public BlockManager(String pth)
        {
            if (File.Exists(pth))
            {
                this.BlockFile = new FileInfo(pth);
                this.Blockname =
                    this.BlockFile.Name.ToUpper().Replace(this.BlockFile.Extension.ToUpper(), "");
            }
            else
                throw new FileNotFoundException("El archivo de bloque no existe");
        }
        /// <summary>
        /// Esta función se encarga de cargar el bloque
        /// </summary>
        public void Load( string nameTheNewBlock )
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable blkTab = (BlockTable)dwg.BlockTableId.GetObject(OpenMode.ForRead);
                    if (!blkTab.Has(this.Blockname))
                    {
                        //Se inserta un registro de bloque vacio
                        blkTab.UpgradeOpen();
                        BlockTableRecord newRecord = new BlockTableRecord();
                        //newRecord.Name = this.Blockname;
                        newRecord.Name = nameTheNewBlock;
                        this.Blockname = newRecord.Name; //
                        blkTab.Add(newRecord);
                        tr.AddNewlyCreatedDBObject(newRecord, true);
                        //Abrir el archivo dwg como base de datos
                        Database externalDB = new Database();
                        externalDB.ReadDwgFile(this.BlockFile.FullName, FileShare.Read, true, null);
                        ObjectIdCollection ids;
                        //Con una subtransacción se clonaran los elementos del espacio del modelo
                        //de la bd externa
                        using (Transaction subTr = externalDB.TransactionManager.StartTransaction())
                        {
                            //Se accede al espacio de modelo de la  base de datos externa
                            ObjectId modelId = SymbolUtilityServices.GetBlockModelSpaceId(externalDB);
                            BlockTableRecord model = subTr.GetObject(modelId, OpenMode.ForRead)
                                as BlockTableRecord;
                            //Se extraen los elementos y se clonan con la función
                            //IdMapping
                            ids = new ObjectIdCollection(model.OfType<ObjectId>().ToArray());
                            using (IdMapping iMap = new IdMapping())
                                dwg.WblockCloneObjects(ids, newRecord.Id, iMap, DuplicateRecordCloning.Replace, false);
                        }
                        //Muy lento para 206
                        //dwg.Insert(this.Blockname, externalDB, true);
                    }
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }

        /// <summary>
        /// Inserta el bloque
        /// </summary>
        /// <param name="inspt">El punto de inserción del bloque</param>
        /// <returns>El id del objeto insertado</returns>
        public ObjectId Insert(Point3d inspt)
        {
            BlockReference refBlk = DBMan.GetReference(this.Blockname, inspt, 1);
            return DBMan.Draw(refBlk)[0];
        }

        public BBox Box, BoxInputA, BoxInputB, BoxOutput;

        public void CreateBBox(ObjectId blkId)
        {
            BlockReference blkRef = DBMan.OpenEnity(blkId) as BlockReference;
            Point3d min = blkRef.GeometricExtents.MinPoint,
                    max = blkRef.GeometricExtents.MaxPoint;
            Box = new BBox(min, max);
            BoxInputA = new BBox(new Point3d(min.X, (min.Y + max.Y) / 2, 0),
                                 new Point3d((min.X + max.X) / 2, max.Y, 0));
            BoxInputB = new BBox(min, new Point3d((min.X + max.X) / 2, (min.Y + max.Y) / 2, 0));
            BoxOutput = new BBox(new Point3d((min.X + max.X) / 2, min.Y, 0), max);
        }

        public static void Find(ObjectId blkId, ObjectId objectTest)
        {
            BBox _BoxInputA, _BoxInputB, _BoxOutput;
            BlockReference blkRef = DBMan.OpenEnity(blkId) as BlockReference;
            Point3d min = blkRef.GeometricExtents.MinPoint,
                    max = blkRef.GeometricExtents.MaxPoint;
            _BoxInputA = new BBox(new Point3d(min.X, (min.Y + max.Y) / 2, 0),
                                 new Point3d((min.X + max.X) / 2, max.Y, 0));
            _BoxInputB = new BBox(min, new Point3d((min.X + max.X) / 2, (min.Y + max.Y) / 2, 0));
            _BoxOutput = new BBox(new Point3d((min.X + max.X) / 2, min.Y, 0), max);
            ObjectIdCollection selObjs;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            if (Lab2.Selector.ByBox(_BoxInputA.Min, _BoxInputA.Max, Lab2.Filter.FilterLine, out selObjs))
                ed.WriteMessage("\nEsta más cerca de la entrada A");
            if (Lab2.Selector.ByBox(_BoxInputB.Min, _BoxInputB.Max, Lab2.Filter.FilterLine, out selObjs))
                ed.WriteMessage("\nEsta más cerca de la entrada B");
            if (Lab2.Selector.ByBox(_BoxOutput.Min, _BoxOutput.Max, Lab2.Filter.FilterLine, out selObjs))
                ed.WriteMessage("\nEsta más cerca de la salida");
            if (selObjs != null && selObjs.Count > 0)
                ed.SetImpliedSelection(selObjs.OfType<ObjectId>().ToArray());
        }



    }
}
