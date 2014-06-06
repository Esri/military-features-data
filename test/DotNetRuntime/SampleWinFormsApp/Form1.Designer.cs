namespace WindowsFormsApp
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxExport = new System.Windows.Forms.PictureBox();
            this.cbSymbolId = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExport)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxExport
            // 
            this.pictureBoxExport.Location = new System.Drawing.Point(24, 94);
            this.pictureBoxExport.Name = "pictureBoxExport";
            this.pictureBoxExport.Size = new System.Drawing.Size(167, 161);
            this.pictureBoxExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxExport.TabIndex = 0;
            this.pictureBoxExport.TabStop = false;
            // 
            // cbSymbolId
            // 
            this.cbSymbolId.FormattingEnabled = true;
            this.cbSymbolId.Items.AddRange(new object[] {
            "SFGPUCI---AAUSG",
            "SFGAUCI---AAUSG",
            "SHGAUCI---DAUSG",
            "SGGPUCI---MO---",
            "SPGPUCI--------",
            "SUGPUCI--------",
            "SAGPUCI--------",
            "SNGPUCI--------",
            "SSGPUCI--------",
            "SGGPUCI--------",
            "SWGPUCI--------",
            "SMGPUCI--------",
            "SDGPUCI--------",
            "SLGPUCI--------",
            "SJGPUCI--------",
            "SKGPUCI--------",
            "SFGAEVAL-------",
            "SFSAXR---------",
            "SFUASN---------",
            "SFFAGP---------",
            "WAS-PLT---P----",
            "EHOPDGC--------"});
            this.cbSymbolId.Location = new System.Drawing.Point(70, 12);
            this.cbSymbolId.Name = "cbSymbolId";
            this.cbSymbolId.Size = new System.Drawing.Size(121, 21);
            this.cbSymbolId.TabIndex = 11;
            this.cbSymbolId.Text = "SFGPUCI---AAUSG";
            this.cbSymbolId.SelectedIndexChanged += new System.EventHandler(this.cbSymbolId_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "SIDC:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(70, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Show Symbol";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 308);
            this.Controls.Add(this.cbSymbolId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBoxExport);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxExport;
        private System.Windows.Forms.ComboBox cbSymbolId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

