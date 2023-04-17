using System;
using System.Net;
using System.Device.Location;
using System.Management;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using System.Net.NetworkInformation;
using System.Windows.Media;

namespace Client_for_Traccar
{
    public class GeoFinder
    {
        //userName,
        private static string userName, time, velocity, bearing, alti, precision, batt;
        private static string latitude, longitude;

        //finds the position by using the GPS
        public static Boolean variousChecks()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Avviare il monitoraggio della posizione
            bool isPositionOK = watcher.TryStart(true, TimeSpan.FromSeconds(1));
            bool isNetworkOK = NetworkInterface.GetIsNetworkAvailable();

            if (isNetworkOK && isNetworkOK)
            {
                return true;
            }

            return false;
        }

        public static void findPosition()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            //check for gps permission and state
            //if (!(watcher.Status.ToString() == GeoPositionStatus.Disabled.ToString() || watcher.Status.ToString() == GeoPositionStatus.NoData.ToString()))
            //{
            //System.Windows.MessageBox.Show(watcher.Status.ToString());
            do
            {
                watcher.TryStart(true, TimeSpan.FromMilliseconds(1500));
            } while (watcher.Status.ToString().Equals("NoData"));

            GeoCoordinate coord = watcher.Position.Location;

            userName = Properties.Settings.Default.defaultDeviceName;
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

        //reads the address which hosts the Traccar server, from file
        private static string readLink()
        {
            return Properties.Settings.Default.serverLink;
        }

        //composes the string and send it to the server
        public static void magicClient(MainWindow mainWindow2)
        {
            if (variousChecks())
            {
                WebClient client = new WebClient();

                //finds battery percentage, if available
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");

                foreach (ManagementObject mo in mos.Get())
                {
                    batt = mo["EstimatedChargeRemaining"].ToString();
                }

                string entireString = "/?id=" + userName + "&timestamp=" + time
                    + "&lat=" + latitude + "&lon=" + longitude + "&speed="
                    + velocity + "&bearing=" + bearing + "&altitude=" + alti + "&accuracy=2000.0";
                string URI = readLink() + entireString;

                //System.Windows.MessageBox.Show(URI);            

                try
                {
                    client.UploadString(URI, entireString);
                }
                catch (Exception ex)
                {
                    // Codice che gestisce l'eccezione
                    System.Windows.MessageBox.Show("Something went wrong: make sure to use a correct name for device!");
                    ThreadForGPS.pauseStyleSetter(mainWindow2);
                    ThreadForGPS.killThread();

                    String s = ex.Message;
                }
            }
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