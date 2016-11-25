using Rtsp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RtspClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //String url = "rtsp://192.168.1.128/ch1.h264";    // IPS
            //String url = "rtsp://192.168.1.125/onvif-media/media.amp?profile=quality_h264"; // Axis
            //String url = "rtsp://192.168.1.124/rtsp_tunnel?h26x=4&line=1&inst=1"; // Bosch

            //String url = "rtsp://192.168.1.121:8554/h264";  // Raspberry Pi RPOS using Live555
            //String url = "rtsp://192.168.1.121:8554/h264m";  // Raspberry Pi RPOS using Live555 in Multicast mode

            //String url = "rtsp://127.0.0.1:8554/h264ESVideoTest"; // Live555 Cygwin
            String url = "rtsp://192.168.1.160:8554/h264ESVideoTest"; // Live555 Cygwin
            //String url = "rtsp://127.0.0.1:8554/h264ESVideoTest"; // Live555 Cygwin
            //String url = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov";


            // Create a RTSP Client
            RTSPClient c = new RTSPClient(url, RTSPClient.RTP_TRANSPORT.TCP);

            // Wait for user to terminate programme
            Console.WriteLine("Press ENTER to exit");
            String dummy = Console.ReadLine();

            c.Stop();

        }
    }


   

}
