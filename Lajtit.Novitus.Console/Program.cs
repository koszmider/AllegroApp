using System;
using System.Net.Sockets;
using System.Text;

namespace Lajtit.Novitus.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            SetCommand(args[0]);
        }
        public static object SetCommand(string ip)
        {
            LajtIt.Dal.Order order = LajtIt.Dal.DbHelper.Orders.GetOrder(70000);

            string xml = @"<pakiet ><wydruk_niefiskalny><linia typ='linia'  numer_czcionki='1' >linia 1</linia><linia typ='qr_kod' pogrubienie='tak' wysrodkowany='tak' numer_czcionki='1' atrybut_czcionki='duza'>NOVITUS</linia></wydruk_niefiskalny></pakiet>";
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                Byte[] bytesSent = Encoding.ASCII.GetBytes(xml);

                s.Connect(ip, 6001);
                // Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);
                Byte[] bytesReceived = new Byte[256];
                // Receive the server home page content.
                int bytes = 0;
                string page = "";

                // The following will block until the page is transmitted.
                while (s.Available > 0)
                {
                    bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                // while (bytes == bytesReceived.Length);

                return page;
            }
            return null;
        }
    }
}
