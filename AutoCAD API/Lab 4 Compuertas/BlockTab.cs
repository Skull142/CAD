﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCADAPI.Lab4
{
    public partial class BlockTab : UserControl
    {
        public String Blockname
        {
            get
            {
                if (this.listOfBlocks.SelectedIndex != -1)
                    return this.listOfBlocks.SelectedItem.ToString();
                else
                    return String.Empty;
            }
        }

        public String Directory_Path;
        public string itemList;
        public int sceneCounter = 0;


        public BlockTab()
        {
            InitializeComponent();
            this.Directory_Path = "";
            this.itemList = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dia = new FolderBrowserDialog();
            dia.RootFolder = Environment.SpecialFolder.Desktop;
            if (dia.ShowDialog() == DialogResult.OK)
            {
                this.Directory_Path = dia.SelectedPath;
                this.listOfBlocks.Items.Clear();
                foreach (FileInfo f in new DirectoryInfo(this.Directory_Path).GetFiles())
                {
                    if (f.Extension.ToUpper().Contains("DWG"))
                        this.listOfBlocks.Items.Add(f.Name);
                }
                if (this.listOfBlocks.Items.Count > 0 && !this.Directory_Path.Equals("NULL"))
                    HidesState(true);
                else
                    HidesState(false);
            }
            else
                this.Directory_Path = String.Empty;
        }

        private void listOfBlocks_DoubleClick(object sender, EventArgs e)
        {
            //this.MethodToExecute("InsertaVehiculo");
        }

        public void PrintValues(List<Movil> mobiles, List<Semaforo> semaforos, List<Polyline> paths)
        {
            if (mobiles.Count == 0)
                VelocityState(false);
            else
                VelocityState(true);
            //
            this.bVelocidades.Items.Clear();
            foreach (Movil m in mobiles)
                this.bVelocidades.Items.Add(m.Data);
            //
            this.bSemaforos.Items.Clear();
            if (semaforos.Count == 0)
                TrafficlightState(false);
            else
                TrafficlightState(true);
            foreach (Semaforo s in semaforos)
            {
                this.bSemaforos.Items.Add(s.Data);
            }
            if(mobiles.Count > 0 || semaforos.Count > 0)
                UpdateState(true);
            else
                UpdateState(false);
            this.lContent.Text = string.Format("Vehicles: {0}\nTraffic Lights: {1}\nPaths: {2}", mobiles.Count, semaforos.Count, paths.Count);

        }
        //ETIQUETAS PARA EL CONTENIDO DEL BLOQUE DE SELECCIOM
        void HidesState(bool state)
        {
            this.lVehiculos.Visible = state;
            this.listOfBlocks.Enabled = state;
            this.listOfBlocks.Visible = state;
            this.bInsertVehicle.Visible = state;
            this.bInsertVehicle.Enabled = state;
            this.bTlights.Visible = state;
            this.bTlights.Enabled = state;
        }
        //ETIQUETAS DEL BLOQUE DE VELOCIDADES
        void VelocityState(bool state)
        {
            this.lVelocidad.Visible = state;
            this.bVelocidades.Visible = state;
            this.bVelocidades.Enabled = state;

        }
        void TrafficlightState(bool state)
        {
            this.lTLights.Visible = state;
            this.bSemaforos.Visible = state;
            this.bSemaforos.Enabled = state;
        }

        //ETIQUETAS DE CONTENIDO Y UPDATE
        void UpdateState(bool state)
        {
            this.bUpdate.Enabled = state;
            this.bUpdate.Visible = state;
            this.bDelete.Enabled = state;
            this.bDelete.Visible = state;
            this.lContent.Visible = state;
        }


        private void bUpdate_Click(object sender, EventArgs e)
        {
            this.MethodToExecute("UpdateScene");
        }


        public void MethodToExecute(string name)
        {
            Autodesk.AutoCAD.ApplicationServices.Document doc;
            //Agrego el using renombrando la clase aplicación
            //using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
            doc = AcadApp.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute(name + " ", true, false, false);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.MethodToExecute("InsertTrafficLight");
        }

        private void bInsertVehicle_Click(object sender, EventArgs e)
        {
            this.MethodToExecute("InsertVehicle");
        }

        private void tbCaution_TextChanged(object sender, EventArgs e)
        {
            this.MethodToExecute("ChangeExternParameters");
        }

        private void tbStopGo_TextChanged(object sender, EventArgs e)
        {
            this.MethodToExecute("ChangeExternParameters");
        }

        private void tbZpos_TextChanged(object sender, EventArgs e)
        {
            this.MethodToExecute("ChangeExternParameters");
        }

        private void tbMin_TextChanged(object sender, EventArgs e)
        {
            this.MethodToExecute("ChangeExternParameters");
        }

        private void tbMax_TextChanged(object sender, EventArgs e)
        {
            this.MethodToExecute("ChangeExternParameters");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            this.MethodToExecute("DeleteElement");
        }

        private void bLoadScene_Click(object sender, EventArgs e)
        {
            this.MethodToExecute("LoadScene");
        }

        private void cbLoopTravel_CheckedChanged(object sender, EventArgs e)
        {
            this.MethodToExecute("ChangeExternParameters");
        }

        private void bVelocidades__DoubleClick(object sender, EventArgs e)
        {
            this.itemList = this.bVelocidades.SelectedItem.ToString().Split(':')[0];
            this.MethodToExecute("FocusVehicle");
        }

        private void bSemaforos__DoubleClick(object sender, EventArgs e)
        {
            this.itemList = this.bSemaforos.SelectedItem.ToString().Split(':')[0];
            this.MethodToExecute("FocusTrafficLight");
        }
    }
}
