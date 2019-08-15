using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AR.Drone.Data;

namespace AR.Drone.WinApp
{
    public partial class PlayerForm : Form
    {
        private string _fileName;
        private FilePlayer _filePlayer;
        private Bitmap _frameBitmap;
        private decimal _frameNumber;

        public PlayerForm()
        {
            InitializeComponent();
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


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            StartPlaying();
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
    }
}