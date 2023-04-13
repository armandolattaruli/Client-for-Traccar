using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_for_Traccar
{
    public static class SharedData
    {
        public static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public static void createTask()
        {
            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {

                    Client_for_Traccar.GeoFinder.findPosition();
                    GeoFinder.magicClient();


                    // Attendi per 5 secondi prima di eseguire nuovamente il codice
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }, _cancellationTokenSource.Token);
        }

        public static void cancelTask()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
