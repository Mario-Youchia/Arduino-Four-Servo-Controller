namespace ServosTest
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(0, 440);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(453, 39);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(358, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-545, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(413, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "In order to use this application in a proper way, follow the instructions below:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(422, 30);
            this.label3.TabIndex = 5;
            this.label3.Text = "* After starting the application, you will have two options either to click\r\n\'def" +
    "ault\' from the toolbar menu, or to fill in the required information manually.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(411, 30);
            this.label4.TabIndex = 5;
            this.label4.Text = "* By clicking on the \'default\' option, the following will be done automatically\r\n" +
    "according to the schematic:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(370, 45);
            this.label5.TabIndex = 5;
            this.label5.Text = "- The control pins of the base servo, elbow servo, shoulder servo, and\r\ngripper s" +
    "ervo will be connected to the digital pins 3, 5, 6, and 9\r\nrespectively of the A" +
    "rduino board.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(369, 15);
            this.label8.TabIndex = 5;
            this.label8.Text = "- Arduino UNO Rev3 will be selected from the board\'s options menu.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(40, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(332, 30);
            this.label9.TabIndex = 5;
            this.label9.Text = "- If there is a connected board available, the port to which it is\r\nconnected wil" +
    "l be selected.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(60, 208);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(316, 30);
            this.label11.TabIndex = 5;
            this.label11.Text = "-- In case of more than one connected board available, the\r\nfirst port will be se" +
    "lected.";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(40, 241);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(342, 30);
            this.label13.TabIndex = 5;
            this.label13.Text = "- Baud rate 9600 bps will be selected from the baud rate options\r\nmenu.";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 273);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(410, 30);
            this.label15.TabIndex = 5;
            this.label15.Text = "* The \'Max\', and \'Min\' buttons are used to set the range of each servo so that\r\nn" +
    "o hardware (servo and/or material) is damaged.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 305);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(417, 60);
            this.label6.TabIndex = 5;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 368);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(379, 30);
            this.label7.TabIndex = 5;
            this.label7.Text = "* If you want to reset the range of all servo motors, you have to choose\r\n\'Hard R" +
    "eset\' from \'Reset\' option in the toolbar.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 400);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(387, 30);
            this.label10.TabIndex = 5;
            this.label10.Text = "* Always make a small adjustment in the angle of any servo to avoid any\r\ndamage t" +
    "o the servo motor.";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(452, 478);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label8;
        private Label label9;
        private Label label11;
        private Label label13;
        private Label label15;
        private Label label6;
        private Label label7;
        private Label label10;
    }
}