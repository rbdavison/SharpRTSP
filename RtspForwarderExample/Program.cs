using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace RtspForwarderExample
{
    public class Program
    {
        private static RtspServer rtspServer;

        public static void Main()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("RtspForwarderExample", LogLevel.Warning)
                    .AddFilter("Rtsp", LogLevel.Warning)
                    .AddConsole();
            });

            // Setup the Server
            int port = 8554;
            string username = null;// = "user";      // or use NUL if there is no username
            string password = null;// = "password";  // or use NUL if there is no password
            /////////////////////////////////////////
            // Step 1 - Start the RTSP Server
            /////////////////////////////////////////
            rtspServer = new RtspServer(port, username, password, loggerFactory);
            try
            {
                rtspServer.StartListen();
            }
            catch
            {
                Console.WriteLine("Error: Could not start server");
                return;
            }

            Console.WriteLine("RTSP URL is rtsp://" + username + ":" + password + "@" + "hostname:" + port);


            // Setup the Client
            RTSPClient client = new(loggerFactory.CreateLogger<RTSPClient>());
            client.ReceivedSpsPps += Client_ReceivedSpsPps;
            client.ReceivedNALs += Client_ReceivedNALs;

            // Connect to RTSP Server
            Console.WriteLine("Connecting");
            // The URL of the source RTSP stream
            string url = "rtsp://192.168.0.100/axis-media/media.amp?videocodec=h264";
            client.Connect(url, RTSPClient.RTP_TRANSPORT.TCP, RTSPClient.MEDIA_REQUEST.VIDEO_ONLY);

            while (true &&
                   !client.StreamingFinished())
            {
                Console.WriteLine("Q = Quit.");
                ConsoleKey key = Console.ReadKey().Key;

                if ( key == ConsoleKey.Q)
                {
                    break;
                }
            }

            client.Stop();
            rtspServer.StopListen();
        }

        private static void Client_ReceivedNALs(uint timestamp_ms, List<byte[]> nal_units)
        {
            rtspServer.FeedInRawNAL(timestamp_ms, nal_units);
        }

        private static void Client_ReceivedSpsPps(byte[] sps, byte[] pps)
        {
            rtspServer.FeedInRawSPSandPPS(sps, pps);
        }

    }
}