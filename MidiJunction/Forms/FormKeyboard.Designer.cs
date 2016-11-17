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
            this.Piano = new Piano();
            this.SuspendLayout();
            // 
            // Piano
            // 
            this.Piano.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
            this.Piano.Location = new System.Drawing.Point(12, 13);
            this.Piano.Name = "Piano";
            this.Piano.Size = new System.Drawing.Size(1024, 111);
            this.Piano.TabIndex = 0;
            // 
            // FormKeyboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 136);
            this.Controls.Add(this.Piano);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormKeyboard";
            this.Text = "Virtual keyboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormKeyboard_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        public Piano Piano;
    }
}