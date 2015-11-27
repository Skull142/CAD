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


namespace ProyectoCAD_041215
{
    public class Commands
    {
        PaletteSet mipaleta;
        Palette blockTab;
        BlockTab ctrl_blockTab;
        //
        List<Movil> moviles = new List<Movil>();
        List<Semaforo> semaforos = new List<Semaforo>();

        [CommandMethod("GUI")]
        public void loadUI()
        {
            if (blockTab == null)
            {
                this.mipaleta = new PaletteSet("Evaluador de Transito");
                this.ctrl_blockTab = new BlockTab();
                this.blockTab = this.mipaleta.Add("Insertar", this.ctrl_blockTab);
                this.mipaleta.Visible = true;
            }
            else
                this.mipaleta.Visible = !this.mipaleta.Visible;
        }
        [CommandMethod("InsertaVehiculo")]
        public void InsertVehiculo()
        {
            //validar la carga de la interfaz
            if (this.ctrl_blockTab == null)
                return;
            String pth = System.IO.Path.Combine(this.ctrl_blockTab.Directory_Path,
                this.ctrl_blockTab.Blockname);

            if (File.Exists(pth))
            {
                BlockManager blkMan = new BlockManager(pth);
                ObjectId rutaId;
                if (Selector.ObjectId("Selecciona la ruta (Polyline)", "", typeof(Polyline), out rutaId))
                {
                    blkMan.Load();
                    ObjectId id = blkMan.Insert(Point3d.Origin);
                    AttributeManager attMan = new AttributeManager(id);
                    //
                    this.moviles.Add(new Movil(ref rutaId, ref id));
                    attMan.SetAttribute("ID", "Movil" + this.moviles.Count);
                    //
                    this.ctrl_blockTab.PrintValues(this.moviles,this.semaforos);
                }
            }
        }
        [CommandMethod("InsertaSemaforo")]
        public void InsertSemaforo()
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
                if (Selector.Point("Selecciona el lugar a colorcarlo (Point3D)", out pos))
                {
                    blkMan.Load();
                    pos = new Point3d(pos.X, pos.Y, 10f);
                    ObjectId id = blkMan.Insert( pos );
                    this.semaforos.Add( new Semaforo( ref id, 20, 5) );
                }
            }
        }
        [CommandMethod("MoverMoviles")]
        public void MoverMoviles()
        {
            if (this.moviles.Count == 0)
                return;
            foreach (Movil m in this.moviles)
            {
                m.MobilesAround(this.moviles);
                m.Move();
            }
            foreach (Semaforo s in this.semaforos)
            {
                s.Update();
            }
            this.ctrl_blockTab.PrintValues( this.moviles, this.semaforos );
        }
    }
}
