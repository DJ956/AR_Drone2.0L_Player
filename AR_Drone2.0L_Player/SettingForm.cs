using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AR_Drone2._0L_Player
{
    public partial class SettingForm : Form
    {
        private static readonly int X = 100;

        private List<Label> labels;

        public SettingForm()
        {
            InitializeComponent();
            
            labels = new List<Label>();
            labels.Add(label_Up);
            labels.Add(label_Down);
            labels.Add(label_Left);
            labels.Add(label_Right);
            labels.Add(label_Forward);
            labels.Add(label_Back);
            labels.Add(label_TurnLeft);
            labels.Add(label_TurnRight);

            LoadSetting();
        }

        private void LoadSetting()
        {
            trackBar_Up.Value = (int)(Properties.Settings.Default.UpMax * X);
            trackBar_Down.Value = (int)(Properties.Settings.Default.DownMax * X);
            trackBar_Left.Value = (int)(Properties.Settings.Default.LeftMax * X);
            trackBar_Right.Value = (int)(Properties.Settings.Default.RightMax * X);
            trackBar_TurnLeft.Value = (int)(Properties.Settings.Default.TurnLeftMax * X);
            trackBar_TurnRight.Value = (int)(Properties.Settings.Default.TurnRightMax * X);
            trackBar_Forward.Value = (int)(Properties.Settings.Default.ForwardMax * X);
            trackBar_Back.Value = (int)(Properties.Settings.Default.BackMax * X);
        }

        private void trackBarValueChange(object sender, EventArgs e)
        {
            const char SPLIT = '_';
            var trackBar = (TrackBar)sender;
            var type = trackBar.Name.Split(SPLIT)[1];
            foreach(var label in labels)
            {
                if(label.Name.Split(SPLIT)[1] == type)
                {
                    label.Text = type + ":" + (trackBar.Value);
                }
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            var upMax = (float)trackBar_Up.Value / X;
            var downMax = (float)trackBar_Down.Value / X;
            var leftMax = (float)trackBar_Left.Value / X;
            var rightMax = (float)trackBar_Right.Value / X;
            var forwardMax = (float)trackBar_Forward.Value / X;
            var backMax = (float)trackBar_Back.Value / X;
            var turnLeftMax = (float)trackBar_TurnLeft.Value / X;
            var turnRightMax = (float)trackBar_TurnRight.Value / X;

            Properties.Settings.Default.UpMax = upMax;
            Properties.Settings.Default.DownMax = downMax;
            Properties.Settings.Default.LeftMax = leftMax;
            Properties.Settings.Default.RightMax = rightMax;
            Properties.Settings.Default.ForwardMax = forwardMax;
            Properties.Settings.Default.BackMax = backMax;
            Properties.Settings.Default.TurnLeftMax = turnLeftMax;
            Properties.Settings.Default.TurnRightMax = turnRightMax;
            
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
