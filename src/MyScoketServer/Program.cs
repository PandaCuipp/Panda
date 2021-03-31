using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyScoketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Server!");

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //
                server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9001));

                server.Listen(10);
                while (true)
                {
                    var client = server.Accept();
                    new Thread(a => ClientSocketHandle(client)).Start();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                server.Close();
            }
        }

        static void ClientSocketHandle(object s)
        {
            var client = (Socket)s;

            string reader = "";
            while (reader != "over")
            {
                byte[] buff = new byte[1024];

                var r = client.Receive(buff);
                reader = Encoding.UTF8.GetString(buff, 0, r);
                Console.WriteLine($"{r}:{reader}");

                client.Send(Encoding.UTF8.GetBytes("收到！"));
            }
            client.Close();
        }
    }
}
