using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//AutoCAD References
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace AutoCADAPI.Lab3
{
    public class DManager
    {
        public Editor ed { get { return AcadApp.DocumentManager.MdiActiveDocument.Editor; } }
        public ObjectId NOD_ID = AcadApp.DocumentManager.MdiActiveDocument.Database.NamedObjectsDictionaryId;
        /// <summary>
        /// Agrega un diccionario al diccionario de AutoCAD
        /// </summary>
        /// <param name="dictionaryName">El nombre del diccionario</param>
        /// <returns>El object id del nuevo diccionario</returns>
        public ObjectId AddDictionary(String dictionaryName)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            ObjectId id = new ObjectId();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DBDictionary NOD = (DBDictionary)NOD_ID.GetObject(OpenMode.ForWrite);
                    DBDictionary d = new DBDictionary();
                    NOD.SetAt(dictionaryName, d);
                    tr.AddNewlyCreatedDBObject(d, true);
                    id = d.ObjectId;
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return id;
        }
        /// <summary>
        /// Agrega un diccionario a otro diccionario, si el id es de una entidad el
        /// diccionario que se abre es el del extensión
        /// </summary>
        /// <param name="dName">El nombre del diccionario</param>
        /// <param name="objId">El id del objeto</param>
        /// <returns>El object id del diccionario del nuevo diccionario</returns>
        public ObjectId AddDictionary(ObjectId objId, String dName)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            ObjectId id = new ObjectId();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DBObject obj = objId.GetObject(OpenMode.ForRead);
                    DBDictionary dic = null;
                    if (obj is Entity)
                    {
                        Entity ent = obj as Entity;
                        if (ent.ExtensionDictionary.IsValid)
                            dic = (DBDictionary)ent.ExtensionDictionary.GetObject(OpenMode.ForWrite);
                        else
                        {
                            ent.UpgradeOpen();
                            ent.CreateExtensionDictionary();
                            dic = (DBDictionary)ent.ExtensionDictionary.GetObject(OpenMode.ForWrite);
                        }
                    }
                    else if (obj is DBDictionary)
                        dic = (DBDictionary)objId.GetObject(OpenMode.ForWrite);
                    //Si el id fue de una entidad o de otro diccionario, 
                    //se tuvo que abrir algun diccionario.
                    if (dic != null)
                    {
                        DBDictionary newDic = new DBDictionary();
                        dic.SetAt(dName, newDic);
                        tr.AddNewlyCreatedDBObject(newDic, true);
                        id = newDic.ObjectId;
                    }
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return id;
        }
        /// <summary>
        /// Agrega un xrecord en un diccionario
        /// </summary>
        /// <param name="xName">El nombre del registro</param>
        /// <param name="dicId">El id del diccionario.</param>
        /// <returns>El object id del nuevo registro</returns>
        public ObjectId AddXRecord(ObjectId dicId, String xName)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            ObjectId id = new ObjectId();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DBDictionary dic = (DBDictionary)dicId.GetObject(OpenMode.ForWrite);
                    Xrecord x = new Xrecord();
                    dic.SetAt(xName, x);
                    tr.AddNewlyCreatedDBObject(x, true);
                    id = x.ObjectId;
                    tr.Commit();
                }
                catch (System.Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return id;
        }
        /// <summary>
        /// Agrega información a un Xrecord
        /// </summary>
        /// <param name="recId">El id del Xrecord.</param>
        /// <param name="data">La información a guardar</param>
        public void AddData(ObjectId recId, params String[] data)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Xrecord oldX = (Xrecord)recId.GetObject(OpenMode.ForWrite);
                    List<TypedValue> typedValueData = new List<TypedValue>();
                    for (int i = 0; i < data.Length; i++)
                        typedValueData.Add(new TypedValue((int)DxfCode.Text, data[i]));
                    oldX.Data = new ResultBuffer(typedValueData.ToArray());
                    tr.Commit();
                }
                catch (System.Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }
        /// <summary>
        /// Remueve un elemento de un diccionario, ya sea un registro
        /// u otro diccionario. El elemento es seleccionado por nombre
        /// </summary>
        /// <param name="key">La llave del elemento</param>
        /// <param name="dicId">El id del diccionario</param>
        public void Remove(ObjectId dicId, String key)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DBDictionary oldD = (DBDictionary)dicId.GetObject(OpenMode.ForWrite);
                    oldD.Remove(key);
                    tr.Commit();
                }
                catch (System.Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }
        /// <summary>
        /// Obtiene un diccionario o un registro, contenido en otro diccionario.
        /// Si el id es de una entidad, el diccionario abrir será el de extensión
        /// </summary>
        /// <param name="dName">El nombre del diccionario</param>
        /// <param name="objId">El id del objeto</param>
        /// <returns>El object id del diccionario seleccionado</returns>
        public ObjectId Get(ObjectId objId, String dName)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            ObjectId id = new ObjectId();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DBObject obj = objId.GetObject(OpenMode.ForRead);
                    DBDictionary dic = null;
                    if (obj is Entity && (obj as Entity).ExtensionDictionary.IsValid)
                        dic = (DBDictionary)(obj as Entity).ExtensionDictionary.GetObject(OpenMode.ForWrite);
                    else if (obj is DBDictionary)
                        dic = (DBDictionary)objId.GetObject(OpenMode.ForRead);
                    if (dic != null)
                        id = dic.GetAt(dName);
                    tr.Commit();
                }
                catch (System.Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return id;
        }
        /// <summary>
        /// Obtiene información de un Xrecord
        /// </summary>
        /// <param name="xId">El id de un Xrecord</param>
        /// <returns>La información del registro</returns>
        public string[] Extract(ObjectId xId)
        {
            Database db = AcadApp.DocumentManager.MdiActiveDocument.Database;
            List<string> values = new List<string>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Xrecord xRec = (Xrecord)xId.GetObject(OpenMode.ForWrite);
                    TypedValue[] tps = xRec.Data.AsArray();
                    foreach (TypedValue tp in tps)
                        values.Add((string)tp.Value);
                }
                catch (System.Exception exc)
                {
                    ed.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
            return values.ToArray();
        }
    }
}
