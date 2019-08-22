using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AR.Drone.Data;
using OpenH264Lib;

namespace AR.Drone.WinApp
{
    public partial class PlayerForm : Form
    {
        private string _fileName;
        private FilePlayer _filePlayer;
        private Bitmap _frameBitmap;
        private decimal _frameNumber;
        private Decoder decoder;

        public PlayerForm(Decoder decoder)
        {
            InitializeComponent();
            this.decoder = decoder;
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
            var root = textBoxFolderPath.Text;
            if (root != "" && Directory.Exists(root) && img != null)
            {
                Parallel.Invoke(() =>
                {
                    var time = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                    time += ".png";
                    var path = Path.Combine(root, time);
                    img.Save(path, ImageFormat.Png);
                });
            }

            pbVideo.Image = img;
            //Thread.Sleep(20);
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

        private void ButtonFolderDialog_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK) { textBoxFolderPath.Text = dialog.SelectedPath; }
        }
    }
}