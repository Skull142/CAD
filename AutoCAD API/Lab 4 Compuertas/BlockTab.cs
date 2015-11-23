using System;
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


        public BlockTab()
        {
            InitializeComponent();
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
                if (this.listOfBlocks.Items.Count > 0)
                    HidesState(true);
                else
                    HidesState(false);
            }
            else
                this.Directory_Path = String.Empty;
        }

        private void listOfBlocks_DoubleClick(object sender, EventArgs e)
        {
            Autodesk.AutoCAD.ApplicationServices.Document doc;
            //Agrego el using renombrando la clase aplicación
            //using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
            doc = AcadApp.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("InsertaVehiculo ", true, false, false);
        }

        public void PrintVelocity(List<Movil> mobiles)
        { 
            if(mobiles.Count == 0)
            {
                VelocityState(false);
                return;
            }
            VelocityState(true);
            this.bVelocidades.Items.Clear();
            foreach(Movil m in mobiles)
            {
                this.bVelocidades.Items.Add(m.Data);
            }

        }
        void HidesState(bool state)
        {
            this.lVehiculos.Visible = state;
            this.lOculto.Visible = state;
            this.listOfBlocks.Enabled = state;
            this.listOfBlocks.Visible = state;
        }
        void VelocityState(bool state)
        {
            this.lVelocidad.Visible = state;
            this.bVelocidades.Visible = state;
            this.bVelocidades.Enabled = state;
        }
        
    }
}
