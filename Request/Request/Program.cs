using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Request
{
    class Program
    {
        static void Main(string[] args)
        {
            var UdpServer = new UdpClient();
            UdpServer.EnableBroadcast = true;

            var r = GetHostIPAddress();// bilgisayarın aldığı IP leri var r değişkennine atıyor.
            var ClientIP = new IPEndPoint(0, 8888);
            string hostIp = r.ToString(); // hostIP adlı değişkene IP atıyor.

            string[] ipCheck = hostIp.Split('.'); // IP yi . olan kısmına bölüyor.

            string kompleIp = string.Format("{0}.{1}.{2}.", ipCheck[0],
                ipCheck[1], ipCheck[2]);// IP nin bölünmüş halini değişkene atıyor.s

            while(true)
            {
                string mesaj = Console.ReadLine(); // console dan gelen mesaj
                byte[] gonderilecekVeri = Encoding.Default.GetBytes(mesaj);// mesajı yazıyor.

                for (int i = 1; i != 254; i++)// sonu 1 ile 255 arasında olması için kullanıyor.
                {
                    Console.WriteLine("Denenen adresler: " + kompleIp + i.ToString());// mesaj göndermek için denenen IP adreslerini yazıyor.
                    UdpServer.Send(gonderilecekVeri, gonderilecekVeri.Length,
                    new IPEndPoint(IPAddress.Parse(kompleIp + i.ToString()), 8888));
                }
            } 
        }

        public static IPAddress GetHostIPAddress()// IP yı aldırdığımız asıl metot .
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses("");
            List<IPAddress> adresler = new List<IPAddress>();
            List<IPAddress> ters_adresler = new List<IPAddress>();

            for(int r = 0; r != hostAddresses.Length; r++)
                adresler.Add(hostAddresses[r]);

            for (int s = adresler.Count - 1; s != -1; s--)
                ters_adresler.Add(adresler[s]);

            foreach (IPAddress adere in ters_adresler)
            {
                if (adere.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(adere) &&
                    !adere.ToString().StartsWith("169.254.") &&// bu IP yi olmayan (bu IP net yokken makinanın aldığı IPdir.
                    !adere.ToString().Contains(":"))// IPv6 almasını istemiyoruz : işareti her Ipv6 da olduğu için içinde bu olan ıp yi almıyoruz.

                    return adere;// ıpyi dönderiyoruz.
            }

            return IPAddress.None; // yoksa hiç birIP döndermiyoruz.
        }
    }
}
