using System;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Device.Location;
using Hardware.Info;
//using NodaTime;
using System.Globalization;
using System.Diagnostics;
using System.Management;

namespace ConsoleApp1
{
    class Program
    {
        public static string userName, tempo, veloci, bearing, alti, preci, batt;
        //public static double latitu, longitu;
        public static string latitu, longitu;
        static void Main(string[] args)
        {
            trova();



            void trova()
            {
                GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

                // Do not suppress prompt, and wait 1000 milliseconds to start.


                do
                {
                    watcher.TryStart(true, TimeSpan.FromMilliseconds(1500));

                    Console.WriteLine(watcher.Status.ToString());
                } while (watcher.Status.ToString().Equals("NoData"));

                GeoCoordinate coord = watcher.Position.Location;
                int i = 0;

                userName = "henlociao";
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                tempo = Timestamp.ToString();
                Console.WriteLine("TIMESTAMP: " + tempo);
                //latitu = "13.3312409";
                latitu = coord.Latitude.ToString();
                latitu = latitu.Replace(',', '.');

                // longitu = "46.9420094";
                longitu = coord.Longitude.ToString();
                longitu = longitu.Replace(',', '.');

                veloci = "0.0";
                bearing = "0.0";

                alti = coord.Altitude.ToString();
                alti = alti.Replace(',', '.');

                double vertical, horizontal, media;
                vertical = coord.VerticalAccuracy;
                horizontal = coord.HorizontalAccuracy;
                media = (vertical + horizontal) / 2;

                preci = media.ToString();


                if (coord.IsUnknown != true)
                {
                    Console.WriteLine("\nLat: {0}, Long: {1}",
                        coord.Latitude,
                        coord.Longitude);
                    magicClient();
                }
                else
                {
                    Console.WriteLine("\nUnknown latitude and longitude. :C");

                }

                void magicClient()
                {
                    WebClient client = new WebClient();

                    /// trova percentuale batteria
                    ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");

                    foreach (ManagementObject mo in mos.Get())
                    {
                        batt = mo["EstimatedChargeRemaining"].ToString();
                    }

                    // "?id=" + userName + "&timestamp=1627595940&lat=" + latitu + "&lon=" + longitu + "&speed=" + veloci + "&bearing=" + bearing + "&altitude=" + alti + "&accuracy=" + + "&batt=" + batt + "
                    string datiVari = "?id=" + "deadbook_02" + "&timestamp=" + tempo + "&lat=" + latitu + "&lon=" + longitu + "&speed=" + veloci + "&bearing=" + bearing + "&altitude=" + alti + "&accuracy=2000.0&batt=" + batt;
                    //funziona alle 21.18 "?id=henlociao&timestamp=1627595440&lat=230.0519427&lon=17.088826&speed=0.0&bearing=90.0&altitude=68.0927084024509&accuracy=2000.0&batt=93.0"
                    string URI = "http://demo3.traccar.org:5055" + datiVari;

                    client.UploadString(URI, datiVari);
                    Console.WriteLine("\nURI Parziale" + datiVari + "\n\nAggiornato.");
                }
            }
        }
    }
}
