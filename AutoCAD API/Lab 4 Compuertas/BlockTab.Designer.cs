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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listOfBlocks = new System.Windows.Forms.ListBox();
            this.fieldInputA = new System.Windows.Forms.TextBox();
            this.fieldInputB = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Entrada A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Entrada B";
            // 
            // listOfBlocks
            // 
            this.listOfBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listOfBlocks.FormattingEnabled = true;
            this.listOfBlocks.Location = new System.Drawing.Point(17, 129);
            this.listOfBlocks.Name = "listOfBlocks";
            this.listOfBlocks.Size = new System.Drawing.Size(226, 277);
            this.listOfBlocks.TabIndex = 2;
            this.listOfBlocks.DoubleClick += new System.EventHandler(this.listOfBlocks_DoubleClick);
            // 
            // fieldInputA
            // 
            this.fieldInputA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldInputA.Location = new System.Drawing.Point(96, 23);
            this.fieldInputA.MaxLength = 1;
            this.fieldInputA.Name = "fieldInputA";
            this.fieldInputA.Size = new System.Drawing.Size(148, 20);
            this.fieldInputA.TabIndex = 3;
            // 
            // fieldInputB
            // 
            this.fieldInputB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldInputB.Location = new System.Drawing.Point(96, 60);
            this.fieldInputB.MaxLength = 1;
            this.fieldInputB.Name = "fieldInputB";
            this.fieldInputB.Size = new System.Drawing.Size(147, 20);
            this.fieldInputB.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(168, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cargar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BlockTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fieldInputB);
            this.Controls.Add(this.fieldInputA);
            this.Controls.Add(this.listOfBlocks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BlockTab";
            this.Size = new System.Drawing.Size(266, 437);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listOfBlocks;
        private System.Windows.Forms.TextBox fieldInputA;
        private System.Windows.Forms.TextBox fieldInputB;
        private System.Windows.Forms.Button button1;
    }
}
