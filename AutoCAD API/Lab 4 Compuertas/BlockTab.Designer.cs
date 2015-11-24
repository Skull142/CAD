namespace AutoCADAPI.Lab4
{
    partial class BlockTab
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlockTab));
            this.label1 = new System.Windows.Forms.Label();
            this.lVehiculos = new System.Windows.Forms.Label();
            this.listOfBlocks = new System.Windows.Forms.ListBox();
            this.cVehiculos = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lOculto = new System.Windows.Forms.Label();
            this.lVelocidad = new System.Windows.Forms.Label();
            this.bVelocidades = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lVehiculos
            // 
            resources.ApplyResources(this.lVehiculos, "lVehiculos");
            this.lVehiculos.Name = "lVehiculos";
            // 
            // listOfBlocks
            // 
            resources.ApplyResources(this.listOfBlocks, "listOfBlocks");
            this.listOfBlocks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listOfBlocks.Name = "listOfBlocks";
            this.listOfBlocks.Sorted = true;
            this.listOfBlocks.DoubleClick += new System.EventHandler(this.listOfBlocks_DoubleClick);
            // 
            // cVehiculos
            // 
            resources.ApplyResources(this.cVehiculos, "cVehiculos");
            this.cVehiculos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cVehiculos.Name = "cVehiculos";
            this.cVehiculos.UseVisualStyleBackColor = true;
            this.cVehiculos.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lOculto
            // 
            resources.ApplyResources(this.lOculto, "lOculto");
            this.lOculto.Name = "lOculto";
            // 
            // lVelocidad
            // 
            resources.ApplyResources(this.lVelocidad, "lVelocidad");
            this.lVelocidad.Name = "lVelocidad";
            // 
            // bVelocidades
            // 
            resources.ApplyResources(this.bVelocidades, "bVelocidades");
            this.bVelocidades.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bVelocidades.FormattingEnabled = true;
            this.bVelocidades.Name = "bVelocidades";
            this.bVelocidades.SelectedIndexChanged += new System.EventHandler(this.bVelocidades_SelectedIndexChanged);
            // 
            // BlockTab
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.bVelocidades);
            this.Controls.Add(this.lVelocidad);
            this.Controls.Add(this.lOculto);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cVehiculos);
            this.Controls.Add(this.listOfBlocks);
            this.Controls.Add(this.lVehiculos);
            this.Controls.Add(this.label1);
            this.Name = "BlockTab";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lVehiculos;
        private System.Windows.Forms.ListBox listOfBlocks;
        private System.Windows.Forms.Button cVehiculos;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lOculto;
        private System.Windows.Forms.Label lVelocidad;
        private System.Windows.Forms.ListBox bVelocidades;
    }
}
