namespace MidiJunction
{
    partial class FormSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.midiChannel = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.inputDevice = new System.Windows.Forms.ComboBox();
            this.outputDevice = new System.Windows.Forms.TextBox();
            this.channel1 = new System.Windows.Forms.TextBox();
            this.channel2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.channel3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.channel4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.channel5 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.channel6 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.breakAfter1 = new System.Windows.Forms.RadioButton();
            this.breakAfter2 = new System.Windows.Forms.RadioButton();
            this.breakAfter4 = new System.Windows.Forms.RadioButton();
            this.breakAfter3 = new System.Windows.Forms.RadioButton();
            this.breakAfter6 = new System.Windows.Forms.RadioButton();
            this.breakAfter5 = new System.Windows.Forms.RadioButton();
            this.breakAfter11 = new System.Windows.Forms.RadioButton();
            this.breakAfter10 = new System.Windows.Forms.RadioButton();
            this.breakAfter9 = new System.Windows.Forms.RadioButton();
            this.breakAfter8 = new System.Windows.Forms.RadioButton();
            this.breakAfter7 = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.channel12 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.channel11 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.channel10 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.channel9 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.channel8 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.channel7 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.breakAfterNone = new System.Windows.Forms.RadioButton();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.midiChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Default MIDI Input Channel";
            // 
            // midiChannel
            // 
            this.midiChannel.Location = new System.Drawing.Point(195, 69);
            this.midiChannel.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.midiChannel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.midiChannel.Name = "midiChannel";
            this.midiChannel.Size = new System.Drawing.Size(70, 22);
            this.midiChannel.TabIndex = 3;
            this.midiChannel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.midiChannel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "MIDI Input Device";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "MIDI Virtual Output Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "1";
            // 
            // inputDevice
            // 
            this.inputDevice.FormattingEnabled = true;
            this.inputDevice.Location = new System.Drawing.Point(195, 97);
            this.inputDevice.Name = "inputDevice";
            this.inputDevice.Size = new System.Drawing.Size(219, 21);
            this.inputDevice.TabIndex = 7;
            // 
            // outputDevice
            // 
            this.outputDevice.Location = new System.Drawing.Point(195, 124);
            this.outputDevice.Name = "outputDevice";
            this.outputDevice.Size = new System.Drawing.Size(219, 22);
            this.outputDevice.TabIndex = 8;
            // 
            // channel1
            // 
            this.channel1.Location = new System.Drawing.Point(105, 201);
            this.channel1.Name = "channel1";
            this.channel1.Size = new System.Drawing.Size(160, 22);
            this.channel1.TabIndex = 9;
            // 
            // channel2
            // 
            this.channel2.Location = new System.Drawing.Point(105, 229);
            this.channel2.Name = "channel2";
            this.channel2.Size = new System.Drawing.Size(160, 22);
            this.channel2.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "2";
            // 
            // channel3
            // 
            this.channel3.Location = new System.Drawing.Point(105, 257);
            this.channel3.Name = "channel3";
            this.channel3.Size = new System.Drawing.Size(160, 22);
            this.channel3.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(50, 260);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "3";
            // 
            // channel4
            // 
            this.channel4.Location = new System.Drawing.Point(105, 285);
            this.channel4.Name = "channel4";
            this.channel4.Size = new System.Drawing.Size(160, 22);
            this.channel4.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "4";
            // 
            // channel5
            // 
            this.channel5.Location = new System.Drawing.Point(105, 313);
            this.channel5.Name = "channel5";
            this.channel5.Size = new System.Drawing.Size(160, 22);
            this.channel5.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(50, 316);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "5";
            // 
            // channel6
            // 
            this.channel6.Location = new System.Drawing.Point(105, 341);
            this.channel6.Name = "channel6";
            this.channel6.Size = new System.Drawing.Size(160, 22);
            this.channel6.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(50, 344);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "6";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(30, 180);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(50, 13);
            this.label17.TabIndex = 32;
            this.label17.Text = "Channel";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(102, 180);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 13);
            this.label18.TabIndex = 33;
            this.label18.Text = "Name";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(266, 180);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 13);
            this.label19.TabIndex = 34;
            this.label19.Text = "Break after";
            // 
            // breakAfter1
            // 
            this.breakAfter1.AutoSize = true;
            this.breakAfter1.Location = new System.Drawing.Point(291, 204);
            this.breakAfter1.Name = "breakAfter1";
            this.breakAfter1.Size = new System.Drawing.Size(14, 13);
            this.breakAfter1.TabIndex = 35;
            // 
            // breakAfter2
            // 
            this.breakAfter2.AutoSize = true;
            this.breakAfter2.Location = new System.Drawing.Point(291, 232);
            this.breakAfter2.Name = "breakAfter2";
            this.breakAfter2.Size = new System.Drawing.Size(14, 13);
            this.breakAfter2.TabIndex = 36;
            this.breakAfter2.TabStop = true;
            // 
            // breakAfter4
            // 
            this.breakAfter4.AutoSize = true;
            this.breakAfter4.Location = new System.Drawing.Point(291, 288);
            this.breakAfter4.Name = "breakAfter4";
            this.breakAfter4.Size = new System.Drawing.Size(14, 13);
            this.breakAfter4.TabIndex = 38;
            this.breakAfter4.TabStop = true;
            // 
            // breakAfter3
            // 
            this.breakAfter3.AutoSize = true;
            this.breakAfter3.Location = new System.Drawing.Point(291, 260);
            this.breakAfter3.Name = "breakAfter3";
            this.breakAfter3.Size = new System.Drawing.Size(14, 13);
            this.breakAfter3.TabIndex = 37;
            // 
            // breakAfter6
            // 
            this.breakAfter6.AutoSize = true;
            this.breakAfter6.Location = new System.Drawing.Point(291, 344);
            this.breakAfter6.Name = "breakAfter6";
            this.breakAfter6.Size = new System.Drawing.Size(14, 13);
            this.breakAfter6.TabIndex = 40;
            this.breakAfter6.TabStop = true;
            // 
            // breakAfter5
            // 
            this.breakAfter5.AutoSize = true;
            this.breakAfter5.Location = new System.Drawing.Point(291, 316);
            this.breakAfter5.Name = "breakAfter5";
            this.breakAfter5.Size = new System.Drawing.Size(14, 13);
            this.breakAfter5.TabIndex = 39;
            this.breakAfter5.TabStop = true;
            // 
            // breakAfter11
            // 
            this.breakAfter11.AutoSize = true;
            this.breakAfter11.Location = new System.Drawing.Point(654, 316);
            this.breakAfter11.Name = "breakAfter11";
            this.breakAfter11.Size = new System.Drawing.Size(14, 13);
            this.breakAfter11.TabIndex = 60;
            this.breakAfter11.TabStop = true;
            // 
            // breakAfter10
            // 
            this.breakAfter10.AutoSize = true;
            this.breakAfter10.Location = new System.Drawing.Point(654, 288);
            this.breakAfter10.Name = "breakAfter10";
            this.breakAfter10.Size = new System.Drawing.Size(14, 13);
            this.breakAfter10.TabIndex = 59;
            this.breakAfter10.TabStop = true;
            // 
            // breakAfter9
            // 
            this.breakAfter9.AutoSize = true;
            this.breakAfter9.Location = new System.Drawing.Point(654, 260);
            this.breakAfter9.Name = "breakAfter9";
            this.breakAfter9.Size = new System.Drawing.Size(14, 13);
            this.breakAfter9.TabIndex = 58;
            this.breakAfter9.TabStop = true;
            // 
            // breakAfter8
            // 
            this.breakAfter8.AutoSize = true;
            this.breakAfter8.Location = new System.Drawing.Point(654, 232);
            this.breakAfter8.Name = "breakAfter8";
            this.breakAfter8.Size = new System.Drawing.Size(14, 13);
            this.breakAfter8.TabIndex = 57;
            this.breakAfter8.TabStop = true;
            // 
            // breakAfter7
            // 
            this.breakAfter7.AutoSize = true;
            this.breakAfter7.Location = new System.Drawing.Point(654, 204);
            this.breakAfter7.Name = "breakAfter7";
            this.breakAfter7.Size = new System.Drawing.Size(14, 13);
            this.breakAfter7.TabIndex = 56;
            this.breakAfter7.TabStop = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(629, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 55;
            this.label11.Text = "Break after";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(465, 180);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 13);
            this.label12.TabIndex = 54;
            this.label12.Text = "Name";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(393, 180);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 13);
            this.label13.TabIndex = 53;
            this.label13.Text = "Channel";
            // 
            // channel12
            // 
            this.channel12.Location = new System.Drawing.Point(468, 341);
            this.channel12.Name = "channel12";
            this.channel12.Size = new System.Drawing.Size(160, 22);
            this.channel12.TabIndex = 52;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(413, 344);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 13);
            this.label14.TabIndex = 51;
            this.label14.Text = "12";
            // 
            // channel11
            // 
            this.channel11.Location = new System.Drawing.Point(468, 313);
            this.channel11.Name = "channel11";
            this.channel11.Size = new System.Drawing.Size(160, 22);
            this.channel11.TabIndex = 50;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(413, 316);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(19, 13);
            this.label15.TabIndex = 49;
            this.label15.Text = "11";
            // 
            // channel10
            // 
            this.channel10.Location = new System.Drawing.Point(468, 285);
            this.channel10.Name = "channel10";
            this.channel10.Size = new System.Drawing.Size(160, 22);
            this.channel10.TabIndex = 48;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(413, 288);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "10";
            // 
            // channel9
            // 
            this.channel9.Location = new System.Drawing.Point(468, 257);
            this.channel9.Name = "channel9";
            this.channel9.Size = new System.Drawing.Size(160, 22);
            this.channel9.TabIndex = 46;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(413, 260);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(13, 13);
            this.label20.TabIndex = 45;
            this.label20.Text = "9";
            // 
            // channel8
            // 
            this.channel8.Location = new System.Drawing.Point(468, 229);
            this.channel8.Name = "channel8";
            this.channel8.Size = new System.Drawing.Size(160, 22);
            this.channel8.TabIndex = 44;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(413, 232);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(13, 13);
            this.label21.TabIndex = 43;
            this.label21.Text = "8";
            // 
            // channel7
            // 
            this.channel7.Location = new System.Drawing.Point(468, 201);
            this.channel7.Name = "channel7";
            this.channel7.Size = new System.Drawing.Size(160, 22);
            this.channel7.TabIndex = 42;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(413, 204);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(13, 13);
            this.label22.TabIndex = 41;
            this.label22.Text = "7";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(593, 377);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(35, 13);
            this.label23.TabIndex = 62;
            this.label23.Text = "None";
            // 
            // breakAfterNone
            // 
            this.breakAfterNone.AutoSize = true;
            this.breakAfterNone.Checked = true;
            this.breakAfterNone.Location = new System.Drawing.Point(654, 377);
            this.breakAfterNone.Name = "breakAfterNone";
            this.breakAfterNone.Size = new System.Drawing.Size(14, 13);
            this.breakAfterNone.TabIndex = 63;
            this.breakAfterNone.TabStop = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(30, 437);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(415, 13);
            this.label24.TabIndex = 64;
            this.label24.Text = "Created by Mats Gefvert using C# and the virtualMIDI SDK from Tobias Erichsen.";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label25.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(30, 459);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(85, 13);
            this.label25.TabIndex = 65;
            this.label25.Text = "Link to website";
            this.label25.Click += new System.EventHandler(this.label25_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label26.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(141, 459);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(124, 13);
            this.label26.TabIndex = 66;
            this.label26.Text = "Link to virtualMIDI SDK";
            this.label26.Click += new System.EventHandler(this.label26_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(520, 449);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(88, 30);
            this.okButton.TabIndex = 67;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(614, 449);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(88, 30);
            this.cancelButton.TabIndex = 68;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(420, 127);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(138, 13);
            this.label27.TabIndex = 69;
            this.label27.Text = "(requires program restart)";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(420, 100);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(104, 13);
            this.label28.TabIndex = 70;
            this.label28.Text = "(partial name is ok)";
            // 
            // FormSettings
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(767, 510);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.breakAfterNone);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.breakAfter11);
            this.Controls.Add(this.breakAfter10);
            this.Controls.Add(this.breakAfter9);
            this.Controls.Add(this.breakAfter8);
            this.Controls.Add(this.breakAfter7);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.channel12);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.channel11);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.channel10);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.channel9);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.channel8);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.channel7);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.breakAfter6);
            this.Controls.Add(this.breakAfter5);
            this.Controls.Add(this.breakAfter4);
            this.Controls.Add(this.breakAfter3);
            this.Controls.Add(this.breakAfter2);
            this.Controls.Add(this.breakAfter1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.channel6);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.channel5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.channel4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.channel3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.channel2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.channel1);
            this.Controls.Add(this.outputDevice);
            this.Controls.Add(this.inputDevice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.midiChannel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MIDI Junction Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.Shown += new System.EventHandler(this.FormSettings_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.midiChannel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown midiChannel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox inputDevice;
        private System.Windows.Forms.TextBox outputDevice;
        private System.Windows.Forms.TextBox channel1;
        private System.Windows.Forms.TextBox channel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox channel3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox channel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox channel5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox channel6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RadioButton breakAfter1;
        private System.Windows.Forms.RadioButton breakAfter2;
        private System.Windows.Forms.RadioButton breakAfter4;
        private System.Windows.Forms.RadioButton breakAfter3;
        private System.Windows.Forms.RadioButton breakAfter6;
        private System.Windows.Forms.RadioButton breakAfter5;
        private System.Windows.Forms.RadioButton breakAfter11;
        private System.Windows.Forms.RadioButton breakAfter10;
        private System.Windows.Forms.RadioButton breakAfter9;
        private System.Windows.Forms.RadioButton breakAfter8;
        private System.Windows.Forms.RadioButton breakAfter7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox channel12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox channel11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox channel10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox channel9;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox channel8;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox channel7;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.RadioButton breakAfterNone;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
    }
}