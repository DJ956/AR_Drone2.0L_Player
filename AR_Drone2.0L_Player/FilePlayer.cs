using System;
using System.IO;
using System.Threading;
using AR.Drone.Data;
using AR.Drone.Infrastructure;
using AR.Drone.Media;
using System.Collections.Generic;

namespace AR.Drone.WinApp
{
    public class FilePlayer : WorkerBase
    {
        private readonly Action<NavigationPacket> _navigationPacketAcquired;
        private readonly string _path;
        private readonly Action<VideoPacket> _videoPacketAcquired;

        public FilePlayer(string path, Action<NavigationPacket> navigationPacketAcquired, Action<VideoPacket> videoPacketAcquired)
        {
            _path = path;
            _navigationPacketAcquired = navigationPacketAcquired;
            _videoPacketAcquired = videoPacketAcquired;
        }


        public void LoadVideoFile(ref Queue<NavigationPacket> navigationPackets, ref Queue<VideoPacket> videoPackets)
        {
            using (var stream = new FileStream(_path, FileMode.Open))
            using (var reader = new PacketReader(stream))
            {
                while (stream.Position < stream.Length)
                {
                    var packetType = reader.ReadPacketType();
                    switch(packetType)
                    {
                        case PacketType.Navigation:
                            var navPacket = reader.ReadNavigationPacket();
                            navigationPackets.Enqueue(navPacket);
                            break;
                        case PacketType.Video:
                            var videoPacket = reader.ReadVideoPacket();
                            videoPackets.Enqueue(videoPacket);
                            break;
                    } 
                }
            }

        }

        protected override void Loop(CancellationToken token)
        {
            using (var stream = new FileStream(_path, FileMode.Open))
            using (var reader = new PacketReader(stream))
            {
                while (stream.Position < stream.Length && token.IsCancellationRequested == false)
                {
                    PacketType packetType = reader.ReadPacketType();
                    switch (packetType)
                    {
                        case PacketType.Navigation:
                            NavigationPacket navigationPacket = reader.ReadNavigationPacket();
                            _navigationPacketAcquired(navigationPacket);
                            break;
                        case PacketType.Video:
                            VideoPacket videoPacket = reader.ReadVideoPacket();
                            _videoPacketAcquired(videoPacket);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}