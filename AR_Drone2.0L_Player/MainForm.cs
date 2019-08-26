using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using AR.Drone.Client;
using AR.Drone.Client.Command;
using AR.Drone.Client.Configuration;
using AR.Drone.Data;
using AR.Drone.Data.Navigation;
using AR.Drone.Data.Navigation.Native;
using AR.Drone.Media;
using AR.Drone.Avionics;
using AR.Drone.Avionics.Objectives;
using AR.Drone.Avionics.Objectives.IntentObtainers;
using AR_Drone2._0L_Player.ControllerCommand;
using AR_Drone2._0L_Player;

namespace AR.Drone.WinApp
{
    public partial class MainForm : Form
    {
        private const string ARDroneTrackFileExt = ".ardrone";
        private const string ARDroneTrackFilesFilter = "AR.Drone track files (*.ardrone)|*.ardrone";

        private readonly OpenH264Lib.Decoder decoder;

        private readonly DroneClient _droneClient;
        private readonly List<PlayerForm> _playerForms;
        private Settings _settings;
        private NavigationData _navigationData;
        private NavigationPacket _navigationPacket;
        private PacketRecorder _packetRecorderWorker;
        private FileStream _recorderStream;
        private Autopilot _autopilot;

        private GameController gameController;
        private ControllerSetting cntSetting;
        private ControllerValues values;
        private Timer controllerTimer;

        public MainForm()
        {
            InitializeComponent();

            _droneClient = new DroneClient("192.168.1.1");
            _droneClient.NavigationPacketAcquired += OnNavigationPacketAcquired;
            _droneClient.VideoPacketAcquired += OnVideoPacketAcquired;
            _droneClient.NavigationDataAcquired += data => _navigationData = data;

            decoder = new OpenH264Lib.Decoder("openh264-1.7.0-win64.dll");

            tmrStateUpdate.Enabled = true;

            controllerTimer = new Timer();
            controllerTimer.Interval = 100;
            controllerTimer.Tick += ControllerTimerTick;

            values = new ControllerValues(-2, -1, -1, -1, -1);
            _playerForms = new List<PlayerForm>();

            cntSetting = ControllerSetting.Load();

            try
            {
                gameController = new GameController(OnTakeOff, OnLand,
                        OnHover, OnUp, OnDown, OnLeft, OnRight, OnForward, OnBack, OnTurnLeft, OnTurnRight,
                        OnFlatTrim);
            }
            catch (ControllerNofFoundException ex){
                MessageBox.Show(ex.Message, "ControllerNotFoundException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach(var codec in Enum.GetNames(typeof(VideoCodecType)))
            {
                if(codec == "NULL") { continue; }
                comboBoxVideoCodec.Items.Add(codec);
            }
            comboBoxVideoCodec.SelectedIndex = 7;
        }

        /// <summary>
        /// コントローラーから取得したアナログ値をドローン用の値に再設定する
        /// </summary>
        /// <param name="x">コントローラーのアナログ値</param>
        /// <param name="in_min">コントローラーの最小値</param>
        /// <param name="in_max">コントローラーの最大値</param>
        /// <param name="out_min">ドローン最小値</param>
        /// <param name="out_max">ドローン最大値</param>
        /// <returns></returns>
        private float Map(float x, float in_min, float in_max, float out_min, float out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        /// <summary>
        /// バッテリー残量をチェックする
        /// </summary>
        private void CheckBatteryStatus()
        {
            var value = progressBarBattery.Value;
            if(value < 25) { progressBarBattery.ForeColor = Color.Red; }
            if(value > 25 && value < 50) { progressBarBattery.ForeColor = Color.Yellow; }
            if(value > 50) { progressBarBattery.ForeColor = Color.Green; }
        }

        protected override void OnLoad(EventArgs e)
        {
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            base.OnLoad(e);
            Text += Environment.Is64BitProcess ? " [64-bit]" : " [32-bit]";
            Text += " - " + version.FileVersion;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_autopilot != null)
            {
                _autopilot.UnbindFromClient();
                _autopilot.Stop();
            }

            StopRecording();

            _droneClient.Dispose();
            decoder.Dispose();
            if(gameController != null) { gameController.Dispose(); }

            base.OnClosed(e);
        }

        /// <summary>
        /// コントローラーの入力を取得し、入力があればコマンドを実行する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControllerTimerTick(object sender, EventArgs e)
        {
            if(gameController == null) { return; }

            if (gameController.TryUpdateGamePad(ref values))
            {
                foreach(var cmd in gameController.Commands)
                {
                    if (cmd.IsMatch(values)) { cmd.Execute(values); }
                }
            }
            values.Clear();
        }

        /// <summary>
        /// ドローンからのデータを受け取る
        /// </summary>
        /// <param name="packet"></param>
        private void OnNavigationPacketAcquired(NavigationPacket packet)
        {
            if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);

            _navigationPacket = packet;
        }

        /// <summary>
        /// ドローンからの映像データを受け取る
        /// </summary>
        /// <param name="packet"></param>
        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            if (_packetRecorderWorker != null && _packetRecorderWorker.IsAlive)
                _packetRecorderWorker.EnqueuePacket(packet);

            pbVideo.Image = decoder.Decode(packet.Data, packet.Data.Length);
        }


        /// <summary>
        /// 離陸コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnTakeOff(ControllerValues values) { _droneClient.Takeoff(); }

        /// <summary>
        /// 着陸コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnLand(ControllerValues values) { _droneClient.Land(); }

        /// <summary>
        /// ホバリングコマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnHover(ControllerValues values) { _droneClient.Hover(); }

        /// <summary>
        /// 上昇コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnUp(ControllerValues values) { _droneClient.Progress(FlightMode.Progressive, gaz: cntSetting.UpMax); }
        
        /// <summary>
        /// 降下コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnDown(ControllerValues values) { _droneClient.Progress(FlightMode.Progressive, gaz: cntSetting.DownMax); }

        /// <summary>
        /// 左に移動 反応しない
        /// </summary>
        /// <param name="values"></param>
        private void OnLeft(ControllerValues values)
        {
            var roll = Map(values.X, LeftCommand.MIN, LeftCommand.MAX, 0, cntSetting.LeftMax);
            _droneClient.Progress(FlightMode.Progressive, roll: roll);
        }

        /// <summary>
        /// 右に移動 反応しない
        /// </summary>
        /// <param name="values"></param>
        private void OnRight(ControllerValues values)
        {
            var roll = Map(values.X, RightCommand.MIN, RightCommand.MAX, 0, cntSetting.RightMax);
            _droneClient.Progress(FlightMode.Progressive, roll: roll);
        }

        /// <summary>
        /// 前進コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnForward(ControllerValues values)
        {
            var pitch = Map(values.Y, ForwardCommand.MIN, ForwardCommand.MAX, 0, cntSetting.ForwardMax);
            _droneClient.Progress(FlightMode.Progressive, pitch: pitch);
        }

        /// <summary>
        /// 後退コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnBack(ControllerValues values)
        {
            var pitch = Map(values.Y, BackCommand.MIN, BackCommand.MAX, 0, cntSetting.BackMax);
            _droneClient.Progress(FlightMode.Progressive, pitch: pitch);
        }

        /// <summary>
        /// 左旋回コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnTurnLeft(ControllerValues values)
        {
            var yaw = Map(values.LeftZ, TurnLeftCommand.MIN, TurnLeftCommand.MAX, 0, cntSetting.TurnLeftMax);
            _droneClient.Progress(FlightMode.Progressive, yaw: yaw);
        }

        /// <summary>
        /// 右旋回コマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnTurnRight(ControllerValues values)
        {
            var yaw = Map(values.LeftZ, TurnRightCommand.MIN, TurnRightCommand.MAX, 0, cntSetting.TurnRightMax);
            _droneClient.Progress(FlightMode.Progressive, yaw: yaw);
        }

        /// <summary>
        /// 姿勢を平行にするコマンド
        /// </summary>
        /// <param name="values"></param>
        private void OnFlatTrim(ControllerValues values) { _droneClient.FlatTrim(); }


        /// <summary>
        /// ドローン接続コマンド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            _droneClient.Start();
            controllerTimer.Enabled = true;
            btnStart.BackColor = Color.LimeGreen;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnStop.BackColor = SystemColors.Control;
        }

        /// <summary>
        /// ドローン接続終了コマンド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            _droneClient.Stop();
            controllerTimer.Enabled = false;
            btnStop.BackColor = Color.Red;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnStart.BackColor = SystemColors.Control;
        }

        /// <summary>
        /// ドローンデータをツリービューに表示させる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            tvInfo.BeginUpdate();

            TreeNode node = tvInfo.Nodes.GetOrCreate("ClientActive");
            node.Text = string.Format("Client Active: {0}", _droneClient.IsActive);

            node = tvInfo.Nodes.GetOrCreate("Navigation Data");
            if (_navigationData != null) DumpBranch(node.Nodes, _navigationData);

            node = tvInfo.Nodes.GetOrCreate("Configuration");
            if (_settings != null) DumpBranch(node.Nodes, _settings);

            TreeNode vativeNode = tvInfo.Nodes.GetOrCreate("Native");

            NavdataBag navdataBag;
            if (_navigationPacket.Data != null && NavdataBagParser.TryParse(ref _navigationPacket, out navdataBag))
            {
                var ctrl_state = (CTRL_STATES) (navdataBag.demo.ctrl_state >> 0x10);
                node = vativeNode.Nodes.GetOrCreate("ctrl_state");
                node.Text = string.Format("Ctrl State: {0}", ctrl_state);

                var flying_state = (FLYING_STATES) (navdataBag.demo.ctrl_state & 0xffff);
                node = vativeNode.Nodes.GetOrCreate("flying_state");
                node.Text = string.Format("Ctrl State: {0}", flying_state);
                
                DumpBranch(vativeNode.Nodes, navdataBag);
                //Label
                var navData = NavdataConverter.ToNavigationData(navdataBag);
                statusLabelBattery.Text = "バッテリー:" + navData.Battery.Percentage;
                progressBarBattery.Value = (int)navData.Battery.Percentage;

                statusLabelWifiQuality.Text = "Link:" + navData.Wifi.LinkQuality * 100;
                progressBarLink.Value = (int)(navData.Wifi.LinkQuality * 100);
                CheckBatteryStatus();
            }
            tvInfo.EndUpdate();

            if (_autopilot != null && !_autopilot.Active && btnAutopilot.ForeColor != Color.Black)
                btnAutopilot.ForeColor = Color.Black;
        }

        private void DumpBranch(TreeNodeCollection nodes, object o)
        {
            Type type = o.GetType();
         
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                TreeNode node = nodes.GetOrCreate(fieldInfo.Name);
                object value = fieldInfo.GetValue(o);
                DumpValue(fieldInfo.FieldType, node, value);
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                TreeNode node = nodes.GetOrCreate(propertyInfo.Name);
                object value = propertyInfo.GetValue(o, null);

                DumpValue(propertyInfo.PropertyType, node, value);
            }
        }

        private void DumpValue(Type type, TreeNode node, object value)
        {
            if (value == null)
                node.Text = node.Name + ": null";
            else
            {
                if (type.Namespace.StartsWith("System") || type.IsEnum)
                    node.Text = node.Name + ": " + value;
                else
                    DumpBranch(node.Nodes, value);
            }
        }

        private void btnFlatTrim_Click(object sender, EventArgs e) { _droneClient.FlatTrim(); }

        private void button2_Click(object sender, EventArgs e) { _droneClient.Takeoff(); }

        private void button3_Click(object sender, EventArgs e) { _droneClient.Land(); }

        private void btnEmergency_Click(object sender, EventArgs e) { _droneClient.Emergency(); }

        private void btnReset_Click(object sender, EventArgs e) { _droneClient.ResetEmergency(); }

        private void btnSwitchCam_Click(object sender, EventArgs e)
        {
            var configuration = new Settings();
            configuration.Video.Channel = VideoChannelType.Next;
            _droneClient.Send(configuration);
        }

        private void btnHover_Click(object sender, EventArgs e) { _droneClient.Hover(); }

        private void btnUp_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, gaz: 0.25f); }

        private void btnDown_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, gaz: -0.25f); }

        private void btnTurnLeft_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, yaw: 0.25f); }

        private void btnTurnRight_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, yaw: -0.25f); }

        private void btnLeft_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, roll: -0.05f); }

        private void btnRight_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, roll: 0.05f); }

        private void btnForward_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, pitch: -0.05f); }

        private void btnBack_Click(object sender, EventArgs e) { _droneClient.Progress(FlightMode.Progressive, pitch: 0.05f); }

        /// <summary>
        /// ドローン設定を読み込む
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadConfig_Click(object sender, EventArgs e)
        {
            Task<Settings> configurationTask = _droneClient.GetConfigurationTask();
            configurationTask.ContinueWith(delegate(Task<Settings> task)
                {
                    if (task.Exception != null)
                    {
                        Trace.TraceWarning("Get configuration task is faulted with exception: {0}", task.Exception.InnerException.Message);
                        return;
                    }

                    _settings = task.Result;
                });
            configurationTask.Start();
        }

        /// <summary>
        /// ドローン設定を再送信する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendConfig_Click(object sender, EventArgs e)
        {
            var sendConfigTask = new Task(() =>
                {
                    if (_settings == null) _settings = new Settings();
                    Settings settings = _settings;

                    if (string.IsNullOrEmpty(settings.Custom.SessionId) ||
                        settings.Custom.SessionId == "00000000")
                    {
                        // set new session, application and profile
                        _droneClient.AckControlAndWaitForConfirmation(); // wait for the control confirmation

                        settings.Custom.SessionId = Settings.NewId();
                        _droneClient.Send(settings);

                        _droneClient.AckControlAndWaitForConfirmation();

                        settings.Custom.ProfileId = Settings.NewId();
                        _droneClient.Send(settings);

                        _droneClient.AckControlAndWaitForConfirmation();

                        settings.Custom.ApplicationId = Settings.NewId();
                        _droneClient.Send(settings);

                        _droneClient.AckControlAndWaitForConfirmation();
                    }

                    settings.General.NavdataDemo = false;
                    settings.General.NavdataOptions = NavdataOptions.All;

                    settings.Video.BitrateCtrlMode = VideoBitrateControlMode.Dynamic;
                    settings.Video.Bitrate = 5000;
                    settings.Video.MaxBitrate = 9000;

                    var name = comboBoxVideoCodec.SelectedItem.ToString();
                    var videoCodec = (VideoCodecType)Enum.Parse(typeof(VideoCodecType), name);

                    settings.Video.Codec = videoCodec;
                    //settings.Leds.LedAnimation = new LedAnimation(LedAnimationType.BlinkGreenRed, 2.0f, 2);
                    //settings.Control.FlightAnimation = new FlightAnimation(FlightAnimationType.Wave);

                    // record video to usb
                    //settings.Video.OnUsb = true;
                    // usage of MP4_360P_H264_720P codec is a requirement for video recording to usb
                    //settings.Video.Codec = VideoCodecType.MP4_360P_H264_720P;
                    // start
                    //settings.Userbox.Command = new UserboxCommand(UserboxCommandType.Start);
                    // stop
                    //settings.Userbox.Command = new UserboxCommand(UserboxCommandType.Stop);


                    //send all changes in one pice
                    _droneClient.Send(settings);
                });
            sendConfigTask.Start();
        }

        /// <summary>
        /// 映像録画を停止させる
        /// </summary>
        private void StopRecording()
        {
            if (_packetRecorderWorker != null)
            {
                _packetRecorderWorker.Stop();
                _packetRecorderWorker.Join();
                _packetRecorderWorker = null;
            }
            if (_recorderStream != null)
            {
                _recorderStream.Dispose();
                _recorderStream = null;
            }
        }

        /// <summary>
        /// 映像データ録画開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            string path = string.Format("flight_{0:yyyy_MM_dd_HH_mm}" + ARDroneTrackFileExt, DateTime.Now);
            
            using (var dialog = new SaveFileDialog {DefaultExt = ARDroneTrackFileExt, Filter = ARDroneTrackFilesFilter, FileName = path})
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    StopRecording();

                    _recorderStream = new FileStream(dialog.FileName, FileMode.OpenOrCreate);
                    _packetRecorderWorker = new PacketRecorder(_recorderStream);
                    _packetRecorderWorker.Start();

                    btnStartRecording.BackColor = Color.Red;
                    btnStartRecording.Enabled = false;
                    btnStopRecording.Enabled = true;
                    btnStopRecording.BackColor = SystemColors.Control;
                }
            }
        }
        /// <summary>
        /// 録画停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            StopRecording();
            btnStopRecording.BackColor = Color.Red;
            btnStopRecording.Enabled = false;
            btnStartRecording.Enabled = true;
            btnStartRecording.BackColor = SystemColors.Control;
        }

        /// <summary>
        /// 録画データのリプレイ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplay_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog {DefaultExt = ARDroneTrackFileExt, Filter = ARDroneTrackFilesFilter})
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    StopRecording();

                    var playerForm = new PlayerForm(decoder) { FileName = dialog.FileName };
                    playerForm.Closed += (o, args) => _playerForms.Remove(o as PlayerForm);
                    _playerForms.Add(playerForm);
                    playerForm.Show(this);
                }
            }
        }

        // Make sure '_autopilot' variable is initialized with an object
        private void CreateAutopilot()
        {
            if (_autopilot != null) return;

            _autopilot = new Autopilot(_droneClient);
            _autopilot.OnOutOfObjectives += Autopilot_OnOutOfObjectives;
            _autopilot.BindToClient();
            _autopilot.Start();
        }

        // Event that occurs when no objectives are waiting in the autopilot queue
        private void Autopilot_OnOutOfObjectives()
        {
            _autopilot.Active = false;
        }

        // Create a simple mission for autopilot
        private void CreateAutopilotMission()
        {
            _autopilot.ClearObjectives();

            // Do two 36 degrees turns left and right if the drone is already flying
            if (_droneClient.NavigationData.State.HasFlag(NavigationState.Flying))
            {
                const float turn = (float)(Math.PI / 5);
                float heading = _droneClient.NavigationData.Yaw;

                _autopilot.EnqueueObjective(Objective.Create(2000, new Heading(heading + turn, aCanBeObtained: true)));
                _autopilot.EnqueueObjective(Objective.Create(2000, new Heading(heading - turn, aCanBeObtained: true)));
                _autopilot.EnqueueObjective(Objective.Create(2000, new Heading(heading, aCanBeObtained: true)));
            }
            else // Just take off if the drone is on the ground
            {
                _autopilot.EnqueueObjective(new FlatTrim(1000));
                _autopilot.EnqueueObjective(new Takeoff(3500));
            }

            // One could use hover, but the method below, allows to gain/lose/maintain desired altitude
            _autopilot.EnqueueObjective(
                Objective.Create(3000,
                    new VelocityX(0.0f),
                    new VelocityY(0.0f),
                    new Altitude(1.0f)
                )
            );

            _autopilot.EnqueueObjective(new Land(5000));
        }

        // Activate/deactive autopilot
        private void btnAutopilot_Click(object sender, EventArgs e)
        {
            if (!_droneClient.IsActive) return;

            CreateAutopilot();
            if (_autopilot.Active) _autopilot.Active = false;
            else
            {
                CreateAutopilotMission();
                _autopilot.Active = true;
                btnAutopilot.ForeColor = Color.Red;
            }
        }

        private void DroneMenuItem_Click(object sender, EventArgs e)
        {
            var settingForm = new SettingForm();
            settingForm.ShowDialog();
            cntSetting = ControllerSetting.Load();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) { this.Close(); }

        private void VersionMenuItem_Click(object sender, EventArgs e)
        {
            var versionForm = new VersionForm();
            versionForm.ShowDialog();
        }
    }
}