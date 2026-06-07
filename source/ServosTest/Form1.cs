using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ArduinoUploader;
using ArduinoUploader.Hardware;
using System.IO.Ports;
using System.Collections.Specialized;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.DirectoryServices.ActiveDirectory;
using static System.Net.Mime.MediaTypeNames;

namespace ServosTest
{
    public partial class Form1 : Form
    {
        internal class NoFocusTrackBar : System.Windows.Forms.TrackBar
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public extern static int SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

            private static int MakeParam(int loWord, int hiWord)
            {
                return (hiWord << 16) | (loWord & 0xffff);
            }

            protected override void OnGotFocus(EventArgs e)
            {
                base.OnGotFocus(e);
                SendMessage(Handle, 0x0128, MakeParam(1, 0x1), 0);
            }
        }

        int[] PWMPins = { 3, 5, 6, 9, 10, 11 };
        bool[] isComboBoxesInitialization = { false, false, false, false };
        bool[] isComboBoxesEnteredFirst = { true, true, true, true };
        Color Start = Color.FromArgb(0, 255, 0, 0);
        Color Center = Color.FromArgb(0, 0, 255, 0);
        Color End = Color.FromArgb(0, 255, 0, 0);
        String[] ports;
        SerialPort port;
        bool isConnected = false;
        int[] serials = { 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 74880, 115200, 230400, 250000, 500000, 1000000, 2000000 };
        String[] ArduinoBoards = { "Leonardo", "Mega1284", "Mega2560", "Micro", "NanoR2", "NanoR3", "UnoR3" };
        bool[] arduinoConnectionComboBoxes = { false, false, false };
        bool[] isArduinoComboBoxesEnteredFirst = { true, true, true };
        bool isFirstSerialConnection = true;
        bool isStructureChanged = false;
        
        string SettingsFileLocation = Directory.GetCurrentDirectory() + "\\Settings.txt";
        string HistoryFileLocation = Directory.GetCurrentDirectory() + "\\History Log.txt";
        StreamWriter SettingsFileWrite;
        StreamWriter HistoryFileWrite;
        StreamReader SettingsFileRead;

        int[] trackBarsValues = { 180, 0, 180, 0, 180, 0, 180, 0 };

        NoFocusTrackBar trackBar1 = new NoFocusTrackBar();
        NoFocusTrackBar trackBar2 = new NoFocusTrackBar();
        NoFocusTrackBar trackBar3 = new NoFocusTrackBar();
        NoFocusTrackBar trackBar4 = new NoFocusTrackBar();

        string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        string pathToTemp = Path.GetTempPath();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            WriteToHistoryLogFile("Application started by user: " + username);

            if (File.Exists(SettingsFileLocation))
            {
                SettingsFileRead = File.OpenText(SettingsFileLocation);
                string[] lines = File.ReadAllLines(SettingsFileLocation);
                for(int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        trackBarsValues[i] = int.Parse(lines[i].Split('=')[1]);
                    } catch
                    {
                        MessageBox.Show("[Error 17]:\nThe structure of settings file has been intentionally changed.", "Settings File Structure Messed Up", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FileInfo myFile = new FileInfo(SettingsFileLocation);
                        myFile.Attributes &= ~FileAttributes.Hidden;
                        myFile.Attributes &= ~FileAttributes.ReadOnly;
                        isStructureChanged = true;
                        break;
                    }
                }
                SettingsFileRead.Close();
                if (isStructureChanged)
                {
                    File.Delete(SettingsFileLocation);
                }
            }

            trackBar1.Orientation = Orientation.Vertical;
            trackBar1.Maximum = trackBarsValues[0];
            trackBar1.Minimum = trackBarsValues[1];
            trackBar1.Value = (trackBar1.Maximum + trackBar1.Minimum) /2;
            trackBar1.Size = new Size(label7.Size.Width, label6.Location.Y + label6.Size.Height - label5.Location.Y);
            trackBar1.Location = new Point(((2 * comboBox1.Location.X + comboBox1.Size.Width) / 2) - trackBar1.Width / 2, label5.Location.Y);
            trackBar1.TickStyle = TickStyle.None;
            trackBar1.LargeChange = 1;
            Controls.Add(trackBar1);
            trackBar1.ValueChanged += trackBar1_ValueChanged;

            trackBar2.Orientation = Orientation.Vertical;
            trackBar2.Maximum = trackBarsValues[2];
            trackBar2.Minimum = trackBarsValues[3];
            trackBar2.Value = (trackBar2.Maximum + trackBar2.Minimum) / 2;
            trackBar2.Size = new Size(label14.Size.Width, label8.Location.Y + label8.Size.Height - label9.Location.Y);
            trackBar2.Location = new Point(((2 * comboBox2.Location.X + comboBox2.Size.Width) / 2) - trackBar2.Width/2, label9.Location.Y);
            trackBar2.TickStyle = TickStyle.None;
            trackBar2.LargeChange = 1;
            Controls.Add(trackBar2);
            trackBar2.ValueChanged += trackBar2_ValueChanged;

            trackBar3.Orientation = Orientation.Vertical;
            trackBar3.Maximum = trackBarsValues[4];
            trackBar3.Minimum = trackBarsValues[5];
            trackBar3.Value = (trackBar3.Maximum + trackBar3.Minimum) / 2;
            trackBar3.Size = new Size(label15.Size.Width, label10.Location.Y + label10.Size.Height - label11.Location.Y);
            trackBar3.Location = new Point(((2 * comboBox3.Location.X + comboBox3.Size.Width) / 2) - trackBar3.Width / 2, label11.Location.Y);
            trackBar3.TickStyle = TickStyle.None;
            trackBar3.LargeChange = 1;
            Controls.Add(trackBar3);
            trackBar3.ValueChanged += trackBar3_ValueChanged;

            trackBar4.Orientation = Orientation.Vertical;
            trackBar4.Maximum = trackBarsValues[6];
            trackBar4.Minimum = trackBarsValues[7];
            trackBar4.Value = (trackBar4.Maximum + trackBar4.Minimum) / 2;
            trackBar4.Size = new Size(label16.Size.Width, label12.Location.Y + label12.Size.Height - label13.Location.Y);
            trackBar4.Location = new Point(((2 * comboBox4.Location.X + comboBox4.Size.Width) / 2) - trackBar4.Width / 2, label13.Location.Y);
            trackBar4.TickStyle = TickStyle.None;
            trackBar4.LargeChange = 1;
            Controls.Add(trackBar4);
            trackBar4.ValueChanged += trackBar4_ValueChanged;

            SoftReset();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (isComboBoxesInitialization[0])
            {
                isComboBoxesInitialization[0] = false;
            }
            else if (!isComboBoxesEnteredFirst[0] || (isComboBoxesEnteredFirst[0] && comboBox1.SelectedIndex != 0))
            {
                comboBox1.Enabled = false;
                comboBox2.Items.Remove(comboBox1.SelectedItem.ToString());
                comboBox3.Items.Remove(comboBox1.SelectedItem.ToString());
                comboBox4.Items.Remove(comboBox1.SelectedItem.ToString());
                toolStripMenuItem2.Enabled = false;
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (isComboBoxesEnteredFirst[0] && comboBox1.SelectedIndex == 0)
            {
                isComboBoxesEnteredFirst[0] = false;
                comboBox1.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isComboBoxesInitialization[1])
            {
                isComboBoxesInitialization[1] = false;
            }
            else if (!isComboBoxesEnteredFirst[1] || (isComboBoxesEnteredFirst[1] && comboBox2.SelectedIndex != 0))
            {
                comboBox2.Enabled = false;
                comboBox1.Items.Remove(comboBox2.SelectedItem.ToString());
                comboBox3.Items.Remove(comboBox2.SelectedItem.ToString());
                comboBox4.Items.Remove(comboBox2.SelectedItem.ToString());
                toolStripMenuItem2.Enabled = false;
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (isComboBoxesEnteredFirst[1] && comboBox2.SelectedIndex == 0)
            {
                isComboBoxesEnteredFirst[1] = false;
                comboBox2.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isComboBoxesInitialization[2])
            {
                isComboBoxesInitialization[2] = false;
            }
            else if (!isComboBoxesEnteredFirst[2] || (isComboBoxesEnteredFirst[2] && comboBox3.SelectedIndex != 0))
            {
                comboBox3.Enabled = false;
                comboBox1.Items.Remove(comboBox3.SelectedItem.ToString());
                comboBox2.Items.Remove(comboBox3.SelectedItem.ToString());
                comboBox4.Items.Remove(comboBox3.SelectedItem.ToString());
                toolStripMenuItem2.Enabled = false;
            }
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            if (isComboBoxesEnteredFirst[2] && comboBox3.SelectedIndex == 0)
            {
                isComboBoxesEnteredFirst[2] = false;
                comboBox3.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isComboBoxesInitialization[3])
            {
                isComboBoxesInitialization[3] = false;
            }
            else if (!isComboBoxesEnteredFirst[3] || (isComboBoxesEnteredFirst[3] && comboBox4.SelectedIndex != 0))
            {
                comboBox4.Enabled = false;
                comboBox1.Items.Remove(comboBox4.SelectedItem.ToString());
                comboBox2.Items.Remove(comboBox4.SelectedItem.ToString());
                comboBox3.Items.Remove(comboBox4.SelectedItem.ToString());
                toolStripMenuItem2.Enabled = false;
            }
        }

        private void comboBox4_Leave(object sender, EventArgs e)
        {
            if (isComboBoxesEnteredFirst[3] && comboBox4.SelectedIndex == 0)
            {
                isComboBoxesEnteredFirst[3] = false;
                comboBox4.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label7.Text = trackBar1.Value.ToString();
            label7.ForeColor = GradientPick((double)(trackBar1.Value - trackBar1.Minimum) / (double)(trackBar1.Maximum - trackBar1.Minimum), Start, Center, End);
            SendData();
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label14.Text = trackBar2.Value.ToString();
            label14.ForeColor = GradientPick((double)(trackBar2.Value - trackBar2.Minimum) / (double)(trackBar2.Maximum - trackBar2.Minimum), Start, Center, End);
            SendData();
        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label15.Text = trackBar3.Value.ToString();
            label15.ForeColor = GradientPick((double)(trackBar3.Value - trackBar3.Minimum) / (double)(trackBar3.Maximum - trackBar3.Minimum), Start, Center, End);
            SendData();
        }
        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            label16.Text = trackBar4.Value.ToString();
            label16.ForeColor = GradientPick((double)(trackBar4.Value - trackBar4.Minimum) / (double)(trackBar4.Maximum - trackBar4.Minimum), Start, Center, End);
            SendData();
        }

        public int LinearInterp(int start, int end, double percentage) => start + (int)Math.Round(percentage * (end - start));
        public Color ColorInterp(Color start, Color end, double percentage) =>
            Color.FromArgb(LinearInterp(start.A, end.A, percentage),
                           LinearInterp(start.R, end.R, percentage),
                           LinearInterp(start.G, end.G, percentage),
                           LinearInterp(start.B, end.B, percentage));
        public Color GradientPick(double percentage, Color Start, Color Center, Color End)
        {
            if (percentage < 0.5)
                return ColorInterp(Start, Center, percentage / 0.5);
            else if (percentage == 0.5)
                return Center;
            else
                return ColorInterp(Center, End, (percentage - 0.5) / 0.5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value < trackBar1.Maximum)
            {
                trackBar1.Value += trackBar1.SmallChange;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (trackBar2.Value < trackBar2.Maximum)
            {
                trackBar2.Value += trackBar2.SmallChange;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (trackBar3.Value < trackBar3.Maximum)
            {
                trackBar3.Value += trackBar3.SmallChange;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (trackBar4.Value < trackBar4.Maximum)
            {
                trackBar4.Value += trackBar4.SmallChange;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value > trackBar1.Minimum)
            {
                trackBar1.Value -= trackBar1.SmallChange;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (trackBar2.Value > trackBar2.Minimum)
            {
                trackBar2.Value -= trackBar2.SmallChange;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (trackBar3.Value > trackBar3.Minimum)
            {
                trackBar3.Value -= trackBar3.SmallChange;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (trackBar4.Value > trackBar4.Minimum)
            {
                trackBar4.Value -= trackBar4.SmallChange;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // First Level Validation
            if (comboBox5.GetItemText(comboBox5.SelectedItem) == "Choose a board")
            {
                MessageBox.Show("[Error 01]:\nChoose a board from the list", "Missed board option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox6.GetItemText(comboBox6.SelectedItem) == "Choose a port" && comboBox6.Items.Count > 1)
            {
                MessageBox.Show("[Error 02]:\nChoose a port from the list", "Missed port option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox6.GetItemText(comboBox6.SelectedItem) == "Choose a port" && comboBox6.Items.Count == 1)
            {
                MessageBox.Show("[Error 03]:\nConnect a board to some port, then choose that port", "No connected board", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if ((comboBox6.GetItemText(comboBox6.SelectedItem) == "" || comboBox6.GetItemText(comboBox6.SelectedItem) == " " || comboBox6.GetItemText(comboBox6.SelectedItem) == null) && comboBox6.Enabled == true)
            {
                MessageBox.Show("[Error 04]:\nConnect a board to some port, then choose that port", "No connected board", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox7.GetItemText(comboBox7.SelectedItem) == "Choose a serial")
            {
                MessageBox.Show("[Error 05]:\nChosse a serial from the list", "Missed serial option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if ((comboBox7.GetItemText(comboBox7.SelectedItem) == "" || comboBox7.GetItemText(comboBox7.SelectedItem) == " " || comboBox7.GetItemText(comboBox7.SelectedItem) == null) && comboBox7.Enabled == true)
            {
                MessageBox.Show("[Error 06]:\nChoose a baud rate form the serial list", "Baud rate missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Second Level Validation
            if (comboBox1.Enabled == true && comboBox1.GetItemText(comboBox1.SelectedItem) == "Choose Pin")
            {
                MessageBox.Show("[Error 07]:\nChoose a pin for base servo", "Missed base servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox1.Enabled == true && (comboBox1.GetItemText(comboBox1.SelectedItem) == "" || comboBox1.GetItemText(comboBox1.SelectedItem) == " " || comboBox1.GetItemText(comboBox1.SelectedItem) == null))
            {
                MessageBox.Show("[Error 08]:\nChoose a pin for base servo", "Missed base servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox2.Enabled == true && comboBox2.GetItemText(comboBox2.SelectedItem) == "Choose Pin")
            {
                MessageBox.Show("[Error 09]:\nChoose a pin for elbow servo", "Missed elbow servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBox2.Enabled == true && (comboBox2.GetItemText(comboBox2.SelectedItem) == "" || comboBox2.GetItemText(comboBox2.SelectedItem) == " " || comboBox2.GetItemText(comboBox2.SelectedItem) == null))
            {
                MessageBox.Show("[Error 10]:\nChoose a pin for elbow servo", "Missed elbow servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox3.Enabled == true && comboBox3.GetItemText(comboBox3.SelectedItem) == "Choose Pin")
            {
                MessageBox.Show("[Error 11]:\nChoose a pin for shoulder servo", "Missed shoulder servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBox3.Enabled == true && (comboBox3.GetItemText(comboBox3.SelectedItem) == "" || comboBox3.GetItemText(comboBox3.SelectedItem) == " " || comboBox3.GetItemText(comboBox3.SelectedItem) == null))
            {
                MessageBox.Show("[Error 12]:\nChoose a pin for shoulder servo", "Missed shoulder servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (comboBox4.Enabled == true && comboBox4.GetItemText(comboBox4.SelectedItem) == "Choose Pin")
            {
                MessageBox.Show("[Error 13]:\nChoose a pin for gripper servo", "Missed gripper servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else if (comboBox4.Enabled == true && (comboBox4.GetItemText(comboBox4.SelectedItem) == "" || comboBox4.GetItemText(comboBox4.SelectedItem) == " " || comboBox4.GetItemText(comboBox4.SelectedItem) == null))
            {
                MessageBox.Show("[Error 14]:\nChoose a pin for gripper servo", "Missed gripper servo pin option", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isAvailable = false;
            ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                if (port == comboBox6.Text)
                {
                    isAvailable = true;
                    break;
                }
            }
            if (!isAvailable)
            {
                MessageBox.Show("[Error 15]:\nYour board is probably disconnected, reconnect it and choose a port from the list", "Unfound board", MessageBoxButtons.OK, MessageBoxIcon.Error);
                resetComboBox6();
                return;
            }

            int Board = 0;
            if (comboBox5.SelectedItem.ToString().Contains("Leonardo"))
            {
                Board = 0;
            } else if (comboBox5.SelectedItem.ToString().Contains("Mega1284"))
            {
                Board = 1;
            } else if (comboBox5.SelectedItem.ToString().Contains("Mega2560"))
            {
                Board = 2;
            } else if (comboBox5.SelectedItem.ToString().Contains("Micro"))
            {
                Board = 3;
            } else if (comboBox5.SelectedItem.ToString().Contains("NanoR2"))
            {
                Board = 4;
            } else if (comboBox5.SelectedItem.ToString().Contains("NanoR3"))
            {
                Board = 5;
            } else if (comboBox5.SelectedItem.ToString().Contains("UnoR3"))
            {
                Board = 6;
            }

            string fileName;
            switch (comboBox7.Text)
            {
                case ("300"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test300_ino_standard);
                    break;
                case ("1200"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test1200_ino_standard);
                    break;
                case ("2400"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test2400_ino_standard);
                    break;
                case ("4800"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test4800_ino_standard);
                    break;
                case ("9600"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test9600_ino_standard);
                    break;
                case ("19200"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test19200_ino_standard);
                    break;
                case ("38400"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test38400_ino_standard);
                    break;
                case ("57600"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test57600_ino_standard);
                    break;
                case ("74880"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test74880_ino_standard);
                    break;
                case ("115200"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test115200_ino_standard);
                    break;
                case ("230400"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test230400_ino_standard);
                    break;
                case ("250000"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test250000_ino_standard);
                    break;
                case ("500000"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test500000_ino_standard);
                    break;
                case ("1000000"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test1000000_ino_standard);
                    break;
                case ("2000000"):
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test2000000_ino_standard);
                    break;
                default:
                    File.WriteAllBytes(pathToTemp + "test.hex", Properties.Resources.Test9600_ino_standard);
                    break;
            }
            fileName = pathToTemp + "test.hex";
            var uploader = new ArduinoSketchUploader(
                new ArduinoSketchUploaderOptions()
                {
                    FileName = fileName,
                    PortName = comboBox6.Text,
                    ArduinoModel = (ArduinoModel)Board
                });
            uploader.UploadSketch();


            port = new SerialPort(comboBox6.Text, int.Parse(comboBox7.Text), Parity.None, 8, StopBits.One);
            port.Open();
            button9.Enabled = false;
            isConnected = true;
            panel1.BackColor = Color.Green;
            panel1.Visible = true;
            button10.Enabled = true;

            Thread.Sleep(3000);
            SendPins();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arduinoConnectionComboBoxes[0])
            {
                arduinoConnectionComboBoxes[0] = false;
            }
            else if (!isArduinoComboBoxesEnteredFirst[0] || (isArduinoComboBoxesEnteredFirst[0] && comboBox5.SelectedIndex != 0))
            {
                comboBox5.Enabled = false;
                toolStripMenuItem2.Enabled = false;
            }
        }

        private void comboBox5_Leave(object sender, EventArgs e)
        {
            if (isArduinoComboBoxesEnteredFirst[0] && comboBox5.SelectedIndex == 0)
            {
                isArduinoComboBoxesEnteredFirst[0] = false;
                comboBox5.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arduinoConnectionComboBoxes[1])
            {
                arduinoConnectionComboBoxes[1] = false;
            }
            else if (!isArduinoComboBoxesEnteredFirst[1] || (isArduinoComboBoxesEnteredFirst[1] && comboBox6.SelectedIndex != 0))
            {
                comboBox6.Enabled = false;
                toolStripMenuItem2.Enabled = false;
            }
        }
        private void comboBox6_Click(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        private void comboBox6_Enter(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        private void comboBox6_Leave(object sender, EventArgs e)
        {
            if (isArduinoComboBoxesEnteredFirst[1] && comboBox6.SelectedIndex == 0)
            {
                isArduinoComboBoxesEnteredFirst[1] = false;
                comboBox6.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arduinoConnectionComboBoxes[2])
            {
                arduinoConnectionComboBoxes[2] = false;
            }
            else if (!isArduinoComboBoxesEnteredFirst[2] || (isArduinoComboBoxesEnteredFirst[2] && comboBox7.SelectedIndex != 0))
            {
                comboBox7.Enabled = false;
                toolStripMenuItem2.Enabled = false;
            }
        }

        private void comboBox7_Leave(object sender, EventArgs e)
        {
            if (isArduinoComboBoxesEnteredFirst[2] && comboBox7.SelectedIndex == 0)
            {
                isArduinoComboBoxesEnteredFirst[2] = false;
                comboBox7.Items.RemoveAt(0);
            }
            label1.Select();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            trackBar1.Minimum = trackBar1.Value;
            label6.Text = trackBar1.Minimum.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            trackBar2.Minimum = trackBar2.Value;
            label8.Text = trackBar2.Minimum.ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            trackBar3.Minimum = trackBar3.Value;
            label10.Text = trackBar3.Minimum.ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            trackBar4.Minimum = trackBar4.Value;
            label12.Text = trackBar4.Minimum.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            trackBar1.Maximum = trackBar1.Value;
            label5.Text = trackBar1.Maximum.ToString();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            trackBar2.Maximum = trackBar2.Value;
            label9.Text = trackBar2.Maximum.ToString();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            trackBar3.Maximum = trackBar3.Value;
            label11.Text = trackBar3.Maximum.ToString();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            trackBar4.Maximum = trackBar4.Value;
            label13.Text = trackBar4.Maximum.ToString();
        }

        private void resetComboBox6()
        {
            comboBox6.Enabled = true;
            comboBox6.Items.Clear();
            comboBox6.Items.Insert(0, "Choose a port");
            arduinoConnectionComboBoxes[0] = true;
            comboBox6.SelectedIndex = 0;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            port.Close();
            button9.Enabled = true;
            isConnected = false;
            panel1.BackColor = Color.Red;
            panel1.Visible = true;
            button10.Enabled = false;
            isFirstSerialConnection = true;
        }

        private void SendData()
        {
            if (isConnected)
            {
                try
                {
                    port.Write(trackBar1.Value.ToString() + ","
                        + trackBar2.Value.ToString() + ","
                        + trackBar3.Value.ToString() + ","
                        + trackBar4.Value.ToString() + "x");
                } catch
                {
                    MessageBox.Show("[Error 16]:\nYour board is disconnected, reconnect it and choose a port from the list", "Disconnected board", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    resetComboBox6();
                    Disconnect();
                }
                Thread.Sleep(100);
            }
        }

        private void SendPins()
        {
            if (isConnected && isFirstSerialConnection)
            {
                port.Write(comboBox1.Text.Split(' ')[1] + ","
                    + comboBox2.Text.Split(' ')[1] + ","
                    + comboBox3.Text.Split(' ')[1] + ","
                    + comboBox4.Text.Split(' ')[1] + "x");
                isFirstSerialConnection = false;
                Thread.Sleep(500);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isConnected)
            {
                port.Close();
            }

            if (File.Exists(SettingsFileLocation))
            {
                FileInfo myFile = new FileInfo(SettingsFileLocation);
                myFile.Attributes &= ~FileAttributes.Hidden;
                myFile.Attributes &= ~FileAttributes.ReadOnly;
            }
            SettingsFileWrite = new StreamWriter(SettingsFileLocation);
            SaveSettings();
            SettingsFileWrite.Close();
            File.SetAttributes(SettingsFileLocation, FileAttributes.Hidden | FileAttributes.ReadOnly);

            if (File.Exists(Path.GetTempPath() + "test.hex"))
            {
                File.Delete(Path.GetTempPath() + "test.hex");
            }

            WriteToHistoryLogFile("Application ended by user: " + username);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "Pin 3";
            comboBox2.SelectedItem = "Pin 5";
            comboBox3.SelectedItem = "Pin 6";
            comboBox4.SelectedItem = "Pin 9";
            comboBox5.SelectedItem = "UnoR3";
            UpdatePorts();
            if (comboBox6.Items.Count > 1)
            {
                comboBox6.SelectedIndex = 1;
            }
            else if (comboBox6.Items.Count == 1 && comboBox6.GetItemText(comboBox6.Items[0]) != "Choose a serial")
            {
                comboBox6.SelectedIndex = 0;
            }
            comboBox7.SelectedItem = "9600";
        }

        private void UpdatePorts()
        {
            ports = SerialPort.GetPortNames();
            int i = 1;
            while (comboBox6.Items.Count > 1)
            {
                if (comboBox6.Items[i] != null)
                {
                    comboBox6.Items.RemoveAt(i);
                }
                i++;
            }
            if (comboBox6.Items.Count == 1 && (comboBox6.Items[0].ToString() != "Choose a port"))
            {
                if (comboBox6.Items[0] != null)
                {
                    comboBox6.Items.RemoveAt(0);
                }
            }
            foreach (string port in ports)
            {
                if (!comboBox6.Items.Contains(port))
                {
                    comboBox6.Items.Add(port);
                }
            }
        }

        private void SoftReset()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();

            comboBox1.Items.Add("Choose Pin");
            comboBox2.Items.Add("Choose Pin");
            comboBox3.Items.Add("Choose Pin");
            comboBox4.Items.Add("Choose Pin");

            for (int i = 0; i < 6; i++)
            {
                comboBox1.Items.Add("Pin " + PWMPins[i]);
                comboBox2.Items.Add("Pin " + PWMPins[i]);
                comboBox3.Items.Add("Pin " + PWMPins[i]);
                comboBox4.Items.Add("Pin " + PWMPins[i]);
            }

            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;

            isComboBoxesInitialization[0] = true;
            isComboBoxesInitialization[1] = true;
            isComboBoxesInitialization[2] = true;
            isComboBoxesInitialization[3] = true;

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            label5.Text = trackBar1.Maximum.ToString();
            label6.Text = trackBar1.Minimum.ToString();
            label9.Text = trackBar2.Maximum.ToString();
            label8.Text = trackBar2.Minimum.ToString();
            label11.Text = trackBar3.Maximum.ToString();
            label10.Text = trackBar3.Minimum.ToString();
            label13.Text = trackBar4.Maximum.ToString();
            label12.Text = trackBar4.Minimum.ToString();

            label7.Text = trackBar1.Value.ToString();
            label14.Text = trackBar2.Value.ToString();
            label15.Text = trackBar3.Value.ToString();
            label16.Text = trackBar4.Value.ToString();

            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();

            button10.Enabled = false;
            button9.Enabled = true;

            panel1.Visible = false;

            comboBox5.Items.Add("Choose a board");
            foreach (string board in ArduinoBoards)
            {
                if (!comboBox5.Items.Contains(board))
                {
                    comboBox5.Items.Add(board);
                }
            }

            comboBox6.Items.Add("Choose a port");
            UpdatePorts();

            comboBox7.Items.Add("Choose a serial");
            foreach (int serial in serials)
            {
                if (!comboBox7.Items.Contains(serial.ToString()))
                {
                    comboBox7.Items.Add(serial.ToString());
                }
            }
            arduinoConnectionComboBoxes[0] = true;
            comboBox5.SelectedIndex = 0;
            arduinoConnectionComboBoxes[1] = true;
            comboBox6.SelectedIndex = 0;
            arduinoConnectionComboBoxes[2] = true;
            comboBox7.SelectedIndex = 0;

            comboBox5.Enabled = true;
            comboBox6.Enabled = true;
            comboBox7.Enabled = true;

            if (isConnected)
            {
                port.Close();
                isConnected = false;
            }
            isFirstSerialConnection = true;
            trackBar1.Value = (trackBar1.Maximum + trackBar1.Minimum) / 2;
            trackBar2.Value = (trackBar2.Maximum + trackBar2.Minimum) / 2;
            trackBar3.Value = (trackBar3.Maximum + trackBar3.Minimum) / 2;
            trackBar4.Value = (trackBar4.Maximum + trackBar4.Minimum) / 2;

            label7.ForeColor = Color.Black;
            label14.ForeColor = Color.Black;
            label15.ForeColor = Color.Black;
            label16.ForeColor = Color.Black;

            toolStripMenuItem2.Enabled = true;

            label1.Select();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SoftReset();
        }

        private void Disconnect()
        {
            port.Close();
            button9.Enabled = true;
            isConnected = false;
            panel1.BackColor = Color.Red;
            panel1.Visible = true;
            button10.Enabled = false;
            isFirstSerialConnection = true;
        }

        private void SaveSettings()
        {
            SettingsFileWrite.WriteLine("BaseServoMaxAngle=" + label5.Text);
            SettingsFileWrite.WriteLine("BaseServoMinAngle=" + label6.Text);
            SettingsFileWrite.WriteLine("ElbowServoMaxAngle=" + label9.Text);
            SettingsFileWrite.WriteLine("ElbowServoMinAngle=" + label8.Text);
            SettingsFileWrite.WriteLine("ShoulderServoMaxAngle=" + label11.Text);
            SettingsFileWrite.WriteLine("ShoulderServoMinAngle=" + label10.Text);
            SettingsFileWrite.WriteLine("GripperServoMaxAngle=" + label13.Text);
            SettingsFileWrite.Write("GripperServoMinAngle=" + label12.Text);
        }

        private void TrackBarHardReset()
        {
            for (int i = 0; i < trackBarsValues.Length; i++)
            {
                if(i%2 == 0)
                {
                    trackBarsValues[i] = 180;
                } else
                {
                    trackBarsValues[i] = 0;
                }
            }
            trackBar1.Maximum = trackBarsValues[0];
            trackBar1.Minimum = trackBarsValues[1];
            trackBar1.Value = (trackBar1.Maximum + trackBar1.Minimum) / 2;
            trackBar2.Maximum = trackBarsValues[2];
            trackBar2.Minimum = trackBarsValues[3];
            trackBar2.Value = (trackBar2.Maximum + trackBar2.Minimum) / 2;
            trackBar3.Maximum = trackBarsValues[4];
            trackBar3.Minimum = trackBarsValues[5];
            trackBar3.Value = (trackBar3.Maximum + trackBar3.Minimum) / 2;
            trackBar4.Maximum = trackBarsValues[6];
            trackBar4.Minimum = trackBarsValues[7];
            trackBar4.Value = (trackBar4.Maximum + trackBar4.Minimum) / 2;
        }

        private void HardReset()
        {
            if (File.Exists(SettingsFileLocation))
            {
                FileInfo myFile = new FileInfo(SettingsFileLocation);
                myFile.Attributes &= ~FileAttributes.Hidden;
                myFile.Attributes &= ~FileAttributes.ReadOnly;
                File.Delete(SettingsFileLocation);
            }
            TrackBarHardReset();
            SoftReset();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            HardReset();
        }

        private void WriteToHistoryLogFile(string line)
        {
            if (File.Exists(HistoryFileLocation))
            {
                FileInfo myFile = new FileInfo(HistoryFileLocation);
                myFile.Attributes &= ~FileAttributes.Hidden;
                myFile.Attributes &= ~FileAttributes.ReadOnly;
                HistoryFileWrite = File.AppendText(HistoryFileLocation);
            }
            else
            {
                HistoryFileWrite = new StreamWriter(HistoryFileLocation);
            }
            line = DateTime.Now.ToString() + " - " + line ;
            HistoryFileWrite.WriteLine(line);
            HistoryFileWrite.Close();
            File.SetAttributes(HistoryFileLocation, FileAttributes.Hidden | FileAttributes.ReadOnly);
        }

        private void schematicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.ShowDialog();

            string[] message =
            {
                "In order to use this application in a proper way, follow the instructions below: ",
                "",
                "* After starting the application, you will have two options either to click 'default' from the toolbar menu, or to fill in the required information manually.",
                "* By clicking on the 'default' option, the following will be done automatically according to the schematic:",
                "\t- The control pins of the base servo, elbow servo, shoulder servo, and",
                "\tgripper servo will be connected to the digital pins 3, 5, 6, and 9",
                "\trespectively of the Arduino board.",
                "\t- Arduino UNO Rev3 will be selected from the board's options menu.",
                "\t- If there is a connected board available, the port to which it is",
                "\tconnected will be selected.",
                "\t\t-- In case of more than one connected board available, the",
                "\t\tfirst port will be selected.",
                "\t- Baud rate 9600 bps will be selected from the baud rate options",
                "\tmenu.",
                "* The 'Max', and 'Min' buttons are used to set the range of each servo so that no hardware (servo and/or material) is damaged.",
                "* The range of each servo is saved in a text file in the same directory so you do not have to bother setting the limits of each servo every time you start the application. Please do not mess with the structure of that file, if you do, you have to do a 'Hard Reset' of the application.",
                "* If you want to reset the range of all servo motors, you have to choose 'Hard Reset' from 'Reset' option in the toolbar.",
                "* Always make a small adjustment in the angle of any servo to avoid any damage to the servo motor."
        };
            string totalMsg = "";
            foreach(string s in message)
            {
                totalMsg += s + "\n";
            }
            //MessageBox.Show(totalMsg, "Help", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.ShowDialog();

            string[] message =
            {
                "Servos Test Application",
                "Version: 1.0.0.8 (19-Sep-22)",
                "",
                "This application is used in the initial stages with any robotic arm with 4 servo motors in order to avoid hardware damage, get the limits of each servo motor, and also find the offset of each one."
            };
            string totalMsg = "";
            foreach (string s in message)
            {
                totalMsg += s + "\n";
            }
            //MessageBox.Show(totalMsg, "About", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
    }
}