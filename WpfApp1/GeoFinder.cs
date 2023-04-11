using System;
using System.Net;
using System.Device.Location;
using System.Management;
using WpfApp1;


class GeoFinder
{
    private string userName, time, velocity, bearing, alti, precision, batt;
    private string latitude, longitude;

    public void trova()
    {
        GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
        // Do not suppress prompt, and wait 1000 milliseconds to start.
        do
        {
            watcher.TryStart(true, TimeSpan.FromMilliseconds(1500));

            Console.WriteLine(watcher.Status.ToString());
        } while (watcher.Status.ToString().Equals("NoData"));


        GeoCoordinate coord = watcher.Position.Location;

        userName = "henlociao";
        var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        time = Timestamp.ToString();
        Console.WriteLine("TIMESTAMP: " + time);
        //latitude = "13.3312409";
        latitude = coord.Latitude.ToString();
        latitude = latitude.Replace(',', '.');

        // longitude = "46.9420094";
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


        /*if (coord.IsUnknown != true)
        {
            Console.WriteLine("\nLat: {0}, Long: {1}",
                coord.Latitude,
                coord.Longitude);
            magicClient();
        }
        else
        {
            Console.WriteLine("\nUnknown latitude and longitude. :C");

        } */
    }

    public string magicClient()
    {
        WebClient client = new WebClient();

        /// trova percentuale batteria
        ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_Battery");

        foreach (ManagementObject mo in mos.Get())

        {
            batt = mo["EstimatedChargeRemaining"].ToString();
        }

        // "?id=" + userName + "&timestamp=1627595940&lat=" + latitude + "&lon=" + longitude + "&speed=" + velocity + "&bearing=" + bearing + "&altitude=" + alti + "&accuracy=" + + "&batt=" + batt + "
        string datiVari = "/?id=" + "deadboo00000k_02" + "&timestamp=" + time + "&lat=" + latitude + "&lon=" + longitude + "&speed=" + velocity + "&bearing=" + bearing + "&altitude=" + alti + "&accuracy=2000.0";
        //funziona alle 21.18 "?id=henlociao&timestamp=1627595440&lat=230.0519427&lon=17.088826&speed=0.0&bearing=90.0&altitude=68.0927084024509&accuracy=2000.0&batt=93.0"
        string URI = "http://demo3.traccar.org:5055" + datiVari;


        client.UploadString(URI, datiVari);
        Console.WriteLine("\nURI Parziale" + datiVari + "\n\nAggiornato.");
        return URI;
    }

    public string getLatitude()
    {
        return latitude;
    }

    public string getLongitude()
    {
        return longitude;
    }


}