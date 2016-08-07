namespace MidiJunction.Forms
{
  partial class FormAbout
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
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
      this.pictureBox1.Image = global::MidiJunction.Properties.Resources.logo_small;
      this.pictureBox1.Location = new System.Drawing.Point(372, 36);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(59, 93);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(37, 36);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(153, 30);
      this.label1.TabIndex = 1;
      this.label1.Text = "MIDI Junction";
      // 
      // label2
      // 
      this.label2.BackColor = System.Drawing.Color.Transparent;
      this.label2.Location = new System.Drawing.Point(39, 83);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(273, 46);
      this.label2.TabIndex = 2;
      this.label2.Text = "Written by Mats Gefvert, primarily for the purpose of making audio software perfo" +
    "rm well in a live setting -- in particular, my church.";
      // 
      // label3
      // 
      this.label3.BackColor = System.Drawing.Color.Transparent;
      this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
      this.label3.Location = new System.Drawing.Point(39, 138);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(273, 36);
      this.label3.TabIndex = 3;
      this.label3.Text = "This program uses the virtualMIDI SDK provided by Tobias Erichsen.";
      this.label3.Click += new System.EventHandler(this.label3_Click);
      // 
      // label4
      // 
      this.label4.BackColor = System.Drawing.Color.Transparent;
      this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
      this.label4.Location = new System.Drawing.Point(39, 183);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(273, 19);
      this.label4.TabIndex = 4;
      this.label4.Text = "It is hosted on GitHub.";
      this.label4.Click += new System.EventHandler(this.label4_Click);
      // 
      // button1
      // 
      this.button1.BackColor = System.Drawing.Color.DimGray;
      this.button1.Location = new System.Drawing.Point(145, 243);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(223, 38);
      this.button1.TabIndex = 5;
      this.button1.Text = "Thank you, this program is awesome!";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // FormAbout
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::MidiJunction.Properties.Resources.BackgroundImage;
      this.ClientSize = new System.Drawing.Size(489, 316);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.pictureBox1);
      this.ForeColor = System.Drawing.Color.White;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormAbout";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "About MIDI Junction";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button button1;
  }
}