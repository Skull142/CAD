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
        public Boolean InputA
        {
            get
            {
                string content = this.fieldInputA.Text;
                if (content == "1")
                    return true;
                else
                    return false;
            }
        }
        public Boolean InputB
        {
            get
            {
                string content = this.fieldInputB.Text;
                if (content == "1")
                    return true;
                else
                    return false;
            }
        }

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
                foreach(FileInfo f in new DirectoryInfo(this.Directory_Path).GetFiles())
                {
                    if (f.Extension.ToUpper().Contains("DWG"))
                        this.listOfBlocks.Items.Add(f.Name);
                }
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
            doc.SendStringToExecute("INSERTCOMPUERTA ", true, false, false);
        }
    }
}
