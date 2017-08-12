using System;

namespace MidiJunction.Forms
{
    partial class FormMain
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.focusLabel = new System.Windows.Forms.Label();
            this.labelCurrentChannel = new System.Windows.Forms.Label();
            this.midiLeft = new System.Windows.Forms.Button();
            this.midiRight = new System.Windows.Forms.Button();
            this.labelVolume = new System.Windows.Forms.Label();
            this.midiBus1 = new MidiJunction.Controls.MidiBus();
            this.midiInputBus = new MidiJunction.Controls.MidiBus();
            this.flowOutputDevicesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.menuHamburger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuShowPiano = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPerformances = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpOverview = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpKeys = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.labelChord = new System.Windows.Forms.Label();
            this.flowButtonPanel.SuspendLayout();
            this.flowOutputDevicesPanel.SuspendLayout();
            this.menuHamburger.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 25);
            this.button1.TabIndex = 1;
            this.button1.TabStop = false;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.ChannelButtonClick);
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChannelButtonMouseClick);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(70, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 25);
            this.button2.TabIndex = 2;
            this.button2.TabStop = false;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.ChannelButtonClick);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.TimerTick);
            // 
            // flowButtonPanel
            // 
            this.flowButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowButtonPanel.BackColor = System.Drawing.Color.Transparent;
            this.flowButtonPanel.Controls.Add(this.button1);
            this.flowButtonPanel.Controls.Add(this.button2);
            this.flowButtonPanel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowButtonPanel.Location = new System.Drawing.Point(311, 9);
            this.flowButtonPanel.Name = "flowButtonPanel";
            this.flowButtonPanel.Size = new System.Drawing.Size(601, 59);
            this.flowButtonPanel.TabIndex = 22;
            // 
            // focusLabel
            // 
            this.focusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.focusLabel.BackColor = System.Drawing.Color.Chartreuse;
            this.focusLabel.Location = new System.Drawing.Point(14, 9);
            this.focusLabel.Name = "focusLabel";
            this.focusLabel.Size = new System.Drawing.Size(19, 72);
            this.focusLabel.TabIndex = 28;
            this.focusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCurrentChannel
            // 
            this.labelCurrentChannel.BackColor = System.Drawing.Color.Transparent;
            this.labelCurrentChannel.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentChannel.ForeColor = System.Drawing.Color.White;
            this.labelCurrentChannel.Location = new System.Drawing.Point(183, 9);
            this.labelCurrentChannel.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrentChannel.Name = "labelCurrentChannel";
            this.labelCurrentChannel.Size = new System.Drawing.Size(55, 47);
            this.labelCurrentChannel.TabIndex = 29;
            this.labelCurrentChannel.Text = "1";
            this.labelCurrentChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // midiLeft
            // 
            this.midiLeft.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.midiLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.midiLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midiLeft.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.midiLeft.ForeColor = System.Drawing.Color.White;
            this.midiLeft.Location = new System.Drawing.Point(190, 61);
            this.midiLeft.Name = "midiLeft";
            this.midiLeft.Size = new System.Drawing.Size(19, 22);
            this.midiLeft.TabIndex = 30;
            this.midiLeft.TabStop = false;
            this.midiLeft.Text = "t";
            this.midiLeft.UseVisualStyleBackColor = true;
            this.midiLeft.Click += new System.EventHandler(this.ChannelCurrentButtonClick);
            // 
            // midiRight
            // 
            this.midiRight.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.midiRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.midiRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.midiRight.Font = new System.Drawing.Font("Wingdings 3", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.midiRight.ForeColor = System.Drawing.Color.White;
            this.midiRight.Location = new System.Drawing.Point(210, 61);
            this.midiRight.Name = "midiRight";
            this.midiRight.Size = new System.Drawing.Size(19, 22);
            this.midiRight.TabIndex = 31;
            this.midiRight.TabStop = false;
            this.midiRight.Text = "u";
            this.midiRight.UseVisualStyleBackColor = true;
            this.midiRight.Click += new System.EventHandler(this.ChannelCurrentButtonClick);
            // 
            // labelVolume
            // 
            this.labelVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVolume.BackColor = System.Drawing.Color.Transparent;
            this.labelVolume.Font = new System.Drawing.Font("Segoe UI Symbol", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVolume.ForeColor = System.Drawing.Color.White;
            this.labelVolume.Location = new System.Drawing.Point(1219, 40);
            this.labelVolume.Name = "labelVolume";
            this.labelVolume.Size = new System.Drawing.Size(80, 28);
            this.labelVolume.TabIndex = 32;
            this.labelVolume.Text = "100/100";
            this.labelVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // midiBus1
            // 
            this.midiBus1.ActiveChannel = null;
            this.midiBus1.BackColor = System.Drawing.Color.Transparent;
            this.midiBus1.ForeColor = System.Drawing.Color.White;
            this.midiBus1.Location = new System.Drawing.Point(3, 3);
            this.midiBus1.Name = "midiBus1";
            this.midiBus1.Size = new System.Drawing.Size(197, 15);
            this.midiBus1.TabIndex = 43;
            this.midiBus1.Title = "MIDI Bus A";
            this.midiBus1.TitleColor = System.Drawing.Color.Transparent;
            this.midiBus1.Vertical = false;
            // 
            // midiInputBus
            // 
            this.midiInputBus.ActiveChannel = null;
            this.midiInputBus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.midiInputBus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.midiInputBus.BackColor = System.Drawing.Color.Transparent;
            this.midiInputBus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.midiInputBus.ForeColor = System.Drawing.Color.White;
            this.midiInputBus.Location = new System.Drawing.Point(39, 9);
            this.midiInputBus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.midiInputBus.Name = "midiInputBus";
            this.midiInputBus.Size = new System.Drawing.Size(129, 72);
            this.midiInputBus.TabIndex = 45;
            this.midiInputBus.Title = "MIDI Input";
            this.midiInputBus.TitleColor = System.Drawing.Color.Empty;
            this.midiInputBus.Vertical = true;
            // 
            // flowOutputDevicesPanel
            // 
            this.flowOutputDevicesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowOutputDevicesPanel.BackColor = System.Drawing.Color.Transparent;
            this.flowOutputDevicesPanel.Controls.Add(this.midiBus1);
            this.flowOutputDevicesPanel.Location = new System.Drawing.Point(980, 9);
            this.flowOutputDevicesPanel.Name = "flowOutputDevicesPanel";
            this.flowOutputDevicesPanel.Size = new System.Drawing.Size(200, 72);
            this.flowOutputDevicesPanel.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Wingdings", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(921, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 74);
            this.label4.TabIndex = 27;
            this.label4.Text = "è";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Wingdings", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(253, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 74);
            this.label3.TabIndex = 20;
            this.label3.Text = "è";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Image = global::MidiJunction.Properties.Resources.hamburger_icon;
            this.button3.Location = new System.Drawing.Point(1271, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(29, 33);
            this.button3.TabIndex = 47;
            this.button3.TabStop = false;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.menuHamburger_Click);
            // 
            // menuHamburger
            // 
            this.menuHamburger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShowPiano,
            this.menuTools,
            this.menuPerformances,
            this.toolStripSeparator1,
            this.menuSettings,
            this.toolStripSeparator2,
            this.menuHelp,
            this.menuClose});
            this.menuHamburger.Name = "menuHamburger";
            this.menuHamburger.Size = new System.Drawing.Size(220, 148);
            // 
            // menuShowPiano
            // 
            this.menuShowPiano.Name = "menuShowPiano";
            this.menuShowPiano.Size = new System.Drawing.Size(219, 22);
            this.menuShowPiano.Text = "Show &piano...";
            this.menuShowPiano.Click += new System.EventHandler(this.menuShowPiano_Click);
            // 
            // menuTools
            // 
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(219, 22);
            this.menuTools.Text = "Show &message trace...";
            this.menuTools.Click += new System.EventHandler(this.menuTools_Click);
            // 
            // menuPerformances
            // 
            this.menuPerformances.Name = "menuPerformances";
            this.menuPerformances.Size = new System.Drawing.Size(219, 22);
            this.menuPerformances.Text = "Show performance library...";
            this.menuPerformances.Click += new System.EventHandler(this.menuPerformances_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(219, 22);
            this.menuSettings.Text = "&Settings...";
            this.menuSettings.Click += new System.EventHandler(this.menuSettings_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpOverview,
            this.menuHelpKeys,
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(219, 22);
            this.menuHelp.Text = "Help";
            // 
            // menuHelpOverview
            // 
            this.menuHelpOverview.Name = "menuHelpOverview";
            this.menuHelpOverview.Size = new System.Drawing.Size(192, 22);
            this.menuHelpOverview.Text = "Show &overview";
            this.menuHelpOverview.Click += new System.EventHandler(this.menuHelpOverview_Click);
            // 
            // menuHelpKeys
            // 
            this.menuHelpKeys.Name = "menuHelpKeys";
            this.menuHelpKeys.Size = new System.Drawing.Size(192, 22);
            this.menuHelpKeys.Text = "Keys";
            this.menuHelpKeys.Click += new System.EventHandler(this.menuHelpKeys_Click);
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(192, 22);
            this.menuHelpAbout.Text = "&About MIDI Junction...";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(219, 22);
            this.menuClose.Text = "E&xit";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(1219, 68);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(80, 10);
            this.progressBar1.TabIndex = 33;
            this.progressBar1.Value = 30;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(314, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(621, 16);
            this.label1.TabIndex = 48;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelChord
            // 
            this.labelChord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelChord.BackColor = System.Drawing.Color.Transparent;
            this.labelChord.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChord.ForeColor = System.Drawing.Color.White;
            this.labelChord.Location = new System.Drawing.Point(1186, 7);
            this.labelChord.Name = "labelChord";
            this.labelChord.Size = new System.Drawing.Size(79, 30);
            this.labelChord.TabIndex = 49;
            this.labelChord.Text = "Cmaj7";
            this.labelChord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.BackgroundImage = global::MidiJunction.Properties.Resources.BackgroundImage;
            this.ClientSize = new System.Drawing.Size(1307, 90);
            this.Controls.Add(this.labelChord);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.labelVolume);
            this.Controls.Add(this.flowOutputDevicesPanel);
            this.Controls.Add(this.midiInputBus);
            this.Controls.Add(this.midiRight);
            this.Controls.Add(this.midiLeft);
            this.Controls.Add(this.labelCurrentChannel);
            this.Controls.Add(this.focusLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.flowButtonPanel);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(0, 90);
            this.Name = "FormMain";
            this.Text = "MIDI Junction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.flowButtonPanel.ResumeLayout(false);
            this.flowButtonPanel.PerformLayout();
            this.flowOutputDevicesPanel.ResumeLayout(false);
            this.menuHamburger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flowButtonPanel;
        private System.Windows.Forms.Label focusLabel;
        private System.Windows.Forms.Label labelCurrentChannel;
        private System.Windows.Forms.Button midiLeft;
        private System.Windows.Forms.Button midiRight;
        private System.Windows.Forms.Label labelVolume;
        private Controls.MidiBus midiBus1;
        private Controls.MidiBus midiInputBus;
        private System.Windows.Forms.FlowLayoutPanel flowOutputDevicesPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip menuHamburger;
        private System.Windows.Forms.ToolStripMenuItem menuShowPiano;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpOverview;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuHelpKeys;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelChord;
        private System.Windows.Forms.ToolStripMenuItem menuPerformances;
    }
}

