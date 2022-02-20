using ConsoleApp1.LajtitService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(System.Configuration.ConfigurationManager.AppSettings["R"]);
            //Console.ReadLine();
            //return;
            while (true)
                SetCommand(args[0], args[1]);
        }
        public  static void SetCommand(string ip, string registerId)
        {
            System.Threading.Thread.Sleep(3000);

            int cashRegisterId = 1;

            if (!String.IsNullOrEmpty(registerId))
                cashRegisterId = Int32.Parse(registerId);

            LajtitService.AutoComplete ac = new LajtitService.AutoComplete();

            string msg = null;
            bool isError = CheckStatus( ip, out msg);

            LajtitService.ReceiptCommand[] cmds = ac.GetOrderReceiptCommands(cashRegisterId, ip, isError, msg);



            if (isError)
                return;

            foreach (LajtitService.ReceiptCommand cmd in cmds)
            {
               SendCommand(ip, ac, cmd);

            }
        }

        private static void SendCommand(string ip, AutoComplete ac, ReceiptCommand cmd)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                string xml = cmd.Xml;

                Console.WriteLine(xml);
                try

                {
                    Encoding cp1252 = Encoding.GetEncoding(1252);

                    Byte[] bytesSent = cp1252.GetBytes(xml);

                    s.Connect(ip, 6001);
                    s.Send(bytesSent, bytesSent.Length, 0);
                    Byte[] bytesReceived = new Byte[256];
                    int bytes = 0;
                    string page = "";

                    while (s.Available > 0)
                    {
                        bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                        page = page + cp1252.GetString(bytesReceived, 0, bytes);
                    }
                    ac.SetOrderReceiptCommandResult(cmd.Id, 2, null);

                }
                catch (Exception ex)
                {
                    ac.SetOrderReceiptCommandResult(cmd.Id, -2, ex.Message);
                }
            }
        }
        private static bool  CheckStatus(string ip, out string msg)
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                string xml = String.Format("{0}, {1}, <pakiet><dle_pl/></pakiet>", DateTime.Now, 
                   System.Configuration.ConfigurationManager.AppSettings["R"]);

                Console.WriteLine(xml);
                try

                {
                    Encoding cp1252 = Encoding.GetEncoding(1252);

                    Byte[] bytesSent = cp1252.GetBytes(xml);

                    s.Connect(ip, 6001);
                    s.Send(bytesSent, bytesSent.Length, 0);
                    Byte[] bytesReceived = new Byte[256];
                    int bytes = 0;
                    string page = "";

                    while (s.Available > 0)
                    {
                        bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                        page = page + cp1252.GetString(bytesReceived, 0, bytes);
                    }
                    msg= page == "" ? null : page;
                    return false;

                }
                catch (Exception ex)
                {
                    msg  = ex.Message;
                    return true;
                }
            }
        }
    }
}
