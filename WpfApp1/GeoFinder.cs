using System;
using System.Net;
using System.Device.Location;
using System.Management;
using System.IO;
using System.Windows.Forms;
using System.Windows;

namespace Client_for_Traccar
{
    public class GeoFinder
    {
        private static string userName, time, velocity, bearing, alti, precision, batt;
        private static string latitude, longitude;

        //finds the position by using the GPS
        public static void findPosition()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            //check for gps permission and state
            if (!(watcher.Status.ToString() == GeoPositionStatus.Disabled.ToString() || watcher.Permission.ToString() == GeoPositionPermission.Denied.ToString()))
            {
                do
                {
                    watcher.TryStart(true, TimeSpan.FromMilliseconds(1500));

                    Console.WriteLine(watcher.Status.ToString());
                } while (watcher.Status.ToString().Equals("NoData"));


                GeoCoordinate coord = watcher.Position.Location;

                userName = "henlociao";
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                time = Timestamp.ToString();

                latitude = coord.Latitude.ToString();
                latitude = latitude.Replace(',', '.');
                longitude = coord.Longitude.ToString();
                longitude = longitude.Replace(',', '.');

                velocity = "0.0";
                bearing = "0.0";

                alti = coord.Altitude.ToString();
                alti = alti.Replace(',', '.');

                double vertical, horizontal, media;
                vertical = coord.VerticalAccuracy;
                horizontal = coord.HorizontalAccuracy;
                media = (vertical + horizontal) / 2;

                precision = media.ToString();
            }
            else
            {
                //System.Windows.MessageBox.Show("Unable to access GPS device. Please, make sure to enable it and to give permission to this application.", "GPS error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.MessageBox.Show(watcher.Status.ToString());
            }
        }

        //reads the address which hosts the Traccar server, from file
        private static string readLink()
        {
            string linkToServer = "";
            string path = @"..\..\srcs\serverLink.txt";
            linkToServer = File.ReadAllText(path);

            return linkToServer;
        }

        //composes the string and send it to the server
        public static string magicClient()
        {
            WebClient client = new WebClient();

            //finds battery percentage, if available
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");

            foreach (ManagementObject mo in mos.Get())
            {
                batt = mo["EstimatedChargeRemaining"].ToString();
            }

            string entireString = "/?id=" + "deadboo00000k_02" + "&timestamp=" + time
                + "&lat=" + latitude + "&lon=" + longitude + "&speed="
                + velocity + "&bearing=" + bearing + "&altitude=" + alti + "&accuracy=2000.0";
            string URI = readLink() + entireString;

            client.UploadString(URI, entireString);
            return URI;
        }

        public static string getLatitude()
        {
            return latitude;
        }

        public static string getLongitude()
        {
            return longitude;
        }
    }
}