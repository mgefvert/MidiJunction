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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelChord = new System.Windows.Forms.Label();
            this.labelDevices = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.labelChord, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDevices, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1090, 709);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labelChord
            // 
            this.labelChord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.labelChord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelChord.Font = new System.Drawing.Font("Segoe UI", 128F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChord.ForeColor = System.Drawing.Color.Lime;
            this.labelChord.Location = new System.Drawing.Point(545, 0);
            this.labelChord.Margin = new System.Windows.Forms.Padding(0);
            this.labelChord.Name = "labelChord";
            this.labelChord.Size = new System.Drawing.Size(545, 709);
            this.labelChord.TabIndex = 3;
            this.labelChord.Text = "label1";
            this.labelChord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDevices
            // 
            this.labelDevices.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.labelDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDevices.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDevices.ForeColor = System.Drawing.Color.White;
            this.labelDevices.Location = new System.Drawing.Point(0, 0);
            this.labelDevices.Margin = new System.Windows.Forms.Padding(0);
            this.labelDevices.Name = "labelDevices";
            this.labelDevices.Size = new System.Drawing.Size(545, 709);
            this.labelDevices.TabIndex = 2;
            this.labelDevices.Text = "label2";
            this.labelDevices.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormSecondaryScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(1090, 709);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSecondaryScreen";
            this.Text = "FormSecondaryScreen";
            this.Load += new System.EventHandler(this.FormSecondaryScreen_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.Label labelChord;
        public System.Windows.Forms.Label labelDevices;
    }
}