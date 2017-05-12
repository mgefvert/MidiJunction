namespace MidiJunction.Forms
{
    partial class FormSecondaryScreen
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelChord = new System.Windows.Forms.Label();
            this.labelDevices = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelDevices);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(30);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.splitContainer1.Panel2.Controls.Add(this.labelChord);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(30);
            this.splitContainer1.Size = new System.Drawing.Size(1090, 709);
            this.splitContainer1.SplitterDistance = 545;
            this.splitContainer1.TabIndex = 0;
            // 
            // labelChord
            // 
            this.labelChord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.labelChord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelChord.Font = new System.Drawing.Font("Segoe UI", 96F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChord.ForeColor = System.Drawing.Color.Lime;
            this.labelChord.Location = new System.Drawing.Point(30, 30);
            this.labelChord.Margin = new System.Windows.Forms.Padding(0);
            this.labelChord.Name = "labelChord";
            this.labelChord.Size = new System.Drawing.Size(481, 649);
            this.labelChord.TabIndex = 0;
            this.labelChord.Text = "label1";
            this.labelChord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDevices
            // 
            this.labelDevices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.labelDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDevices.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDevices.ForeColor = System.Drawing.Color.White;
            this.labelDevices.Location = new System.Drawing.Point(30, 30);
            this.labelDevices.Margin = new System.Windows.Forms.Padding(0);
            this.labelDevices.Name = "labelDevices";
            this.labelDevices.Size = new System.Drawing.Size(485, 649);
            this.labelDevices.TabIndex = 1;
            this.labelDevices.Text = "label2";
            this.labelDevices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSecondaryScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(1090, 709);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSecondaryScreen";
            this.Text = "FormSecondaryScreen";
            this.Load += new System.EventHandler(this.FormSecondaryScreen_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Label labelDevices;
        public System.Windows.Forms.Label labelChord;
    }
}