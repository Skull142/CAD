using System;
using System.Threading;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//Usings de AutoCAD
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Geometry;
using System.IO;
using AutoCADAPI.Lab2;
using AutoCADAPI.Lab3;
using AutoCADAPI.Lab4;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;


namespace ProyectoCAD_041215
{
    public class Commands
    {
        private PaletteSet mipaleta;
        private Palette blockTab;
        private BlockTab ctrl_blockTab;
        //
        private List<Movil> moviles = new List<Movil>();
        private int movilesCounter = 0;
        private List<Semaforo> semaforos = new List<Semaforo>();
        private int semaforosCounter = 0;
        private List<Polyline> paths = new List<Polyline>();

        [CommandMethod("GUI")]
        public void LoadGUI()
        {
            if (blockTab == null)
            {
                this.mipaleta = new PaletteSet("Traffic Simulator");
                this.ctrl_blockTab = new BlockTab();
                this.blockTab = this.mipaleta.Add("Insertar", this.ctrl_blockTab);
                this.mipaleta.Visible = true;
            }
            else
                this.mipaleta.Visible = !this.mipaleta.Visible;
        }
        [CommandMethod("InsertVehicle")]
        public void InsertVehicle()
        {
            //validar la carga de la interfaz
            if (this.ctrl_blockTab == null)
                return;
            String pth = System.IO.Path.Combine(this.ctrl_blockTab.Directory_Path,
                "vehicle.dwg");

            if (File.Exists(pth))
            {
                BlockManager blkMan = new BlockManager(pth);
                ObjectId rutaId;
                if (Selector.ObjectId("Select the Path to insert the Vehicle (Polyline)", "", typeof(Polyline), out rutaId))
                {
                    blkMan.Load("V"+this.movilesCounter.ToString("D3"));
                    ObjectId id = blkMan.Insert(Point3d.Origin);
                    AttributeManager attMan = new AttributeManager(id);
                    //
                    this.moviles.Add(new Movil(ref rutaId, ref id, double.Parse(StringNull(this.ctrl_blockTab.tbMin.Text)), double.Parse(StringNull(this.ctrl_blockTab.tbMax.Text)), moviles.Count));
                    this.movilesCounter++;
                    attMan.SetAttribute("ID", "V" + this.moviles.Count);
                    //
                    this.ctrl_blockTab.PrintValues(this.moviles, this.semaforos);
                }
            }
        }
        [CommandMethod("InsertTrafficLight")]
        public void InsertTrafficLight()
        {
            //validar la carga de la interfaz
            if (this.ctrl_blockTab == null)
                return;
            String pth = System.IO.Path.Combine(this.ctrl_blockTab.Directory_Path,
                "trafficLight.dwg");

            if (File.Exists(pth))
            {
                BlockManager blkMan = new BlockManager(pth);
                Point3d pos;
                if (Selector.Point("Select the point to insert the Traffic Light (Point3D)", out pos))
                {
                    blkMan.Load("TL"+this.semaforosCounter.ToString("D3"));
                    pos = new Point3d(pos.X, pos.Y, float.Parse(StringNull( this.ctrl_blockTab.tbZpos.Text) ));
                    ObjectId id = blkMan.Insert( pos );
                    this.semaforos.Add( new Semaforo( ref id, semaforos.Count,int.Parse(StringNull( this.ctrl_blockTab.tbStopGo.Text )), int.Parse( StringNull( this.ctrl_blockTab.tbCaution.Text) ) ));
                    semaforosCounter++;
                    //
                    this.ctrl_blockTab.PrintValues(this.moviles, this.semaforos);
                }
            }
        }
        [CommandMethod("UpdateScene")]
        public void UpdateScene()
        {
            if (this.ctrl_blockTab == null)
                return;
            if (this.moviles.Count == 0 && this.semaforos.Count == 0)
                return;
            foreach (Movil m in this.moviles)
            {
                m.CheckVelocity(this.moviles, this.semaforos);
                m.Move();
            }
            foreach (Semaforo s in this.semaforos)
            {
                s.Update();
            }
            this.ctrl_blockTab.PrintValues( this.moviles, this.semaforos );
        }

        [CommandMethod("ChangeExternParameters")]
        public void ChangeExternParameters()
        {
            if (this.ctrl_blockTab == null)
                return;
            if (this.moviles.Count == 0 && this.semaforos.Count == 0)
                return;
            foreach (Movil m in this.moviles)
                m.ChangeExternValues(double.Parse(StringNull(this.ctrl_blockTab.tbMin.Text)), double.Parse(StringNull(this.ctrl_blockTab.tbMax.Text)));
            foreach (Semaforo s in this.semaforos)
                s.ChangeExternValues( int.Parse(StringNull( this.ctrl_blockTab.tbStopGo.Text) ), int.Parse(StringNull( this.ctrl_blockTab.tbCaution.Text) ), float.Parse(StringNull( this.ctrl_blockTab.tbZpos.Text) ));
        }

        [CommandMethod("FocusElement")]
        public void Focus()
        {
            if (this.ctrl_blockTab == null)
                return;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Point3d pos;
            if (Selector.Point("Selecciona el lugar a colorcarlo (Point3D)", out pos))
            { 
                ViewTableRecord vw = new ViewTableRecord();
                vw.CenterPoint = new Point2d(pos.X, pos.Y);
                //vw.Elevation = pos.Z;
                vw.Height = 10;
                vw.Width = 10;
                ed.SetCurrentView(vw);
            }

        }
        [CommandMethod("DeleteElement")]
        public void DeleteElement()
        {
            if (this.ctrl_blockTab == null)
                return;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ObjectId obj;
            if (Selector.ObjectId("OBJ", out obj))
            {
                foreach (Movil m in moviles)
                {
                    if( m.mobile.Equals(obj) )
                    {
                        ed.WriteMessage("{0} Erase!\n",m.bloque.Name);
                        moviles.Remove(m);
                        DBMan.Erase(obj);
                        this.ctrl_blockTab.PrintValues(this.moviles, this.semaforos);
                        return;
                    }
                }
                foreach (Semaforo s in semaforos)
                {
                    if (s.id.Equals(obj))
                    {
                        ed.WriteMessage("{0} Erase!\n", s.block.Name);
                        semaforos.Remove(s);
                        DBMan.Erase(obj);
                        this.ctrl_blockTab.PrintValues(this.moviles, this.semaforos);
                        return;
                    }
                }
                ed.WriteMessage("{0} Not is element of the Traffic Simulator System!\n", obj);
            }
        }
        [CommandMethod("LoadScene")]
        public void LoadScene()
        {
            if (this.ctrl_blockTab == null)
                return;
            Autodesk.AutoCAD.ApplicationServices.Document doc;
            //Agrego el using renombrando la clase aplicación
            //using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
            doc = AcadApp.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("Open ", true, false, false);
            this.mipaleta.Close();
        }
        public void all()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            ed.SelectAll();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTableRecord model = db.CurrentSpaceId.GetObject(OpenMode.ForRead) as BlockTableRecord ;
                foreach (ObjectId id in model)
                {
                    DBObject obj = id.GetObject(OpenMode.ForRead);

                    if (obj is Polyline)
                    {
                        paths.Add(obj as Polyline);
                        DManager d = new DManager();
                        d.AddData(d.AddXRecord(d.AddDictionary(obj.Id, "App"), "Dato"), "ruta", "");
                        d.Extract(d.Get(d.Get(obj.Id, "App"), "Dato"));
                    }
                }
            }
        }
        
        string StringNull(string s)
        {
            return s.Equals("") ? "0" : s;
        }
    }
}
