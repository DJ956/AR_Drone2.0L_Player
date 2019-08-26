using AR_Drone2._0L_Player.ControllerCommand;
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

        private ControllerSetting cntSetting;

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

            cntSetting = ControllerSetting.Load();
            trackBar_Up.Value = (int)(cntSetting.UpMax * X);
            trackBar_Down.Value = (int)(cntSetting.DownMax * X);
            trackBar_Left.Value = (int)(cntSetting.LeftMax * X);
            trackBar_Right.Value = (int)(cntSetting.RightMax * X);
            trackBar_TurnLeft.Value = (int)(cntSetting.TurnLeftMax * X);
            trackBar_TurnRight.Value = (int)(cntSetting.TurnRightMax * X);
            trackBar_Forward.Value = (int)(cntSetting.ForwardMax * X);
            trackBar_Back.Value = (int)(cntSetting.BackMax * X);
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

            cntSetting.UpMax = upMax;
            cntSetting.DownMax = downMax;
            cntSetting.LeftMax = leftMax;
            cntSetting.RightMax = rightMax;
            cntSetting.ForwardMax = forwardMax;
            cntSetting.BackMax = backMax;
            cntSetting.TurnLeftMax = turnLeftMax;
            cntSetting.TurnRightMax = turnRightMax;

            ControllerSetting.Save(cntSetting);

            this.Close();
        }
    }
}
