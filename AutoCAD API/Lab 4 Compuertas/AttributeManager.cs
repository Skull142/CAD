using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
namespace AutoCADAPI.Lab4
{
    public class AttributeManager
    {
        public ObjectId BlockReferenceId;

        public AttributeManager(ObjectId id)
        {
            this.BlockReferenceId = id;
        }

        public Boolean HasAttribute(String attname, out AttributeReference att)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            List<AttributeReference> attRefs;
            List<AttributeDefinition> attDefs;
            AttributeDefinition att_Def;
            att = null;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockReference blkRef = (BlockReference)
                        this.BlockReferenceId.GetObject(OpenMode.ForRead);
                    //Los atributos son guardados como definiciones, se realiza la selección de
                    //de todos los atributos de definición.
                    //Selección de todas las entidades contenidas en el bloque
                    IEnumerable<DBObject> blockEntities =
                        (blkRef.BlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord).OfType<ObjectId>().
                    Select<ObjectId, DBObject>(x => x.GetObject(OpenMode.ForRead));
                    //Deben existir atributos de definición para poder crear un parámetro
                    if (blockEntities.Where(x => x is AttributeDefinition).Count() > 0)
                    {
                        //Se realiza la selección de los atributos de definición
                        attDefs = blockEntities.Where(x => x is AttributeDefinition).
                            Select<DBObject, AttributeDefinition>(y => y as AttributeDefinition).ToList();
                        //Se abren los atributos de referencia existentes
                        attRefs = blkRef.AttributeCollection.OfType<ObjectId>().
                            Select<ObjectId, AttributeReference>(x => x.GetObject(OpenMode.ForRead) as AttributeReference).ToList();
                        //Los atributos de referencia se guardan en la propiedad del bloque 
                        //AttributeCollection
                        if (blkRef.AttributeCollection.Count > 0 &&
                            attRefs.Where(x => x.Tag.ToUpper() == attname.ToUpper()).Count() > 0)
                            att = attRefs.Where(x => x.Tag.ToUpper() == attname.ToUpper()).FirstOrDefault();
                        else if (attDefs.Where(x => x.Tag.ToUpper() == attname.ToUpper()).Count() > 0)
                        {
                            //Si no existen la referencia se debe crear
                            att_Def = attDefs.Where(x => x.Tag.ToUpper() == attname.ToUpper()).FirstOrDefault();
                            att = new AttributeReference();
                            att.SetAttributeFromBlock(att_Def, blkRef.BlockTransform);
                            blkRef.UpgradeOpen();
                            blkRef.AttributeCollection.AppendAttribute(att);
                            tr.AddNewlyCreatedDBObject(att, true);
                        }
                        else
                            att = null;
                    }
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return att != null;
        }
        public void SetAttribute(String att, String value)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    AttributeReference attRef;
                    if (HasAttribute(att, out attRef))
                    {
                        attRef = (AttributeReference)
                            attRef.Id.GetObject(OpenMode.ForWrite);
                        attRef.TextString = value;

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
        public String GetAttribute(String att)
        {
            String result = String.Empty;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database dwg = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = dwg.TransactionManager.StartTransaction())
            {
                try
                {
                    AttributeReference attRef;
                    if (HasAttribute(att, out attRef))
                    {
                        attRef = (AttributeReference)
                            attRef.Id.GetObject(OpenMode.ForWrite);
                        result = attRef.TextString;
                    }
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return result;
        }


    }
}
