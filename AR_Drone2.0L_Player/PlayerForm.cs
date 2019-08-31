using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AR.Drone.Data;
using OpenH264Lib;
using AR_Drone2._0L_Player.Codec;
using System.Collections.Generic;
using System.Linq;

namespace AR.Drone.WinApp
{
    public partial class PlayerForm : Form
    {
        private string _fileName;
        private FilePlayer _filePlayer;
        private Decoder decoder;
        private AviWriter aviWriter;

        private int packetCount;

        private Queue<VideoPacket> videoPackets;
        private Queue<NavigationPacket> navigationPackets;

        public PlayerForm(Decoder decoder)
        {
            InitializeComponent();
            this.decoder = decoder;
            videoPackets = new Queue<VideoPacket>();
            navigationPackets = new Queue<NavigationPacket>();
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                Text = Path.GetFileName(_fileName);
            }
        }

        private void UnhandledException(object sender, Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Unhandled Exception (Ctrl+C)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void StartPlaying()
        {
            StopPlaying();
            if (_filePlayer == null) _filePlayer = new FilePlayer(_fileName, OnNavigationPacketAcquired, OnVideoPacketAcquired);
            _filePlayer.UnhandledException += UnhandledException;
            _filePlayer.Start();
        }

        private void OnNavigationPacketAcquired(NavigationPacket obj)
        {
            // do nothing
        }

        private void OnVideoPacketAcquired(VideoPacket packet)
        {
            var img = decoder.Decode(packet.Data, packet.Data.Length);
            pbVideo.Image = img;
            Thread.Sleep(20);
        }

        private void StopPlaying()
        {
            if (_filePlayer != null)
            {
                _filePlayer.Stop();
                _filePlayer.Join();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            navigationPackets.Clear();
            Close();
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            StopPlaying();
            StartPlaying();
        }

        protected override void OnClosed(EventArgs e)
        {
            StopPlaying();
            base.OnClosed(e);
        }

        private void ButtonSaveDialog_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "avi | *.avi";
            dialog.FileName = Path.GetFileName(_fileName);
            if (dialog.ShowDialog() == DialogResult.OK) { textBoxSavePath.Text = dialog.FileName; }
        }

        private async void ButtonSave_Click(object sender, EventArgs e)
        {
            if(textBoxSavePath.Text == "") { buttonSaveDialog.PerformClick(); }
            if (_filePlayer == null) _filePlayer = new FilePlayer(_fileName, OnNavigationPacketAcquired, OnVideoPacketAcquired);
            _filePlayer.UnhandledException += UnhandledException;

            _filePlayer.LoadVideoFile(ref navigationPackets, ref videoPackets);
            packetCount = videoPackets.Count;

            var progress = new Progress<int>(UpdateProgress);
            await Task.Run(()=> EncodeToAvi(videoPackets, progress));

            MessageBox.Show($"保存完了しました : {textBoxSavePath.Text}",
                "Aviファイル作成完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EncodeToAvi(Queue<VideoPacket> packets, IProgress<int> progress)
        {
            var packet = videoPackets.Peek();
            var width = packet.Width;
            var height = packet.Height;

            using (var stream = new FileStream(textBoxSavePath.Text, FileMode.Create))
            {
                aviWriter = new AviWriter(stream, "H264", width, height, 20);
                var i = 0;
                while (videoPackets.Any())
                {
                    packet = videoPackets.Dequeue();
                    aviWriter.AddImage(packet.Data, false);
                    i++;
                    progress.Report(i);
                }

                aviWriter.Close();
            }

            videoPackets.Clear();
        }

        private void UpdateProgress(int i)
        {
            double value = (double)i / packetCount * 100;
            labelProgress.Text = $"エンコード:{(int)value}%";
            progressBar.Value = (int)value;
        }
    }
}