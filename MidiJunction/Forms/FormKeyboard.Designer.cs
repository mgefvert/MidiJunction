using System;
using MidiJunction.Controls;

namespace MidiJunction.Forms
{
    partial class FormKeyboard
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
            this.components = new System.ComponentModel.Container();
            this.Piano = new MidiJunction.Controls.Piano();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Piano
            // 
            this.Piano.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Piano.Location = new System.Drawing.Point(12, 13);
            this.Piano.Name = "Piano";
            this.Piano.Size = new System.Drawing.Size(896, 100);
            this.Piano.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormKeyboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 125);
            this.Controls.Add(this.Piano);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormKeyboard";
            this.Text = "Virtual keyboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormKeyboard_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.FormKeyboard_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormKeyboard_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormKeyboard_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        public Piano Piano;
        private System.Windows.Forms.Timer timer1;
    }
}