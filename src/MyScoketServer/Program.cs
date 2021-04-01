using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyScoketServer
{
    class Program
    {
        static Encoding encoding = Encoding.Default;

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
                int r = 0;
                try
                {
                    r = client.Receive(buff);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        Console.WriteLine($"某一client关闭！");
                    }
                    else
                    {
                        Console.WriteLine(ex);
                    }

                    break;
                }

                reader = encoding.GetString(buff, 0, r);
                Console.WriteLine($"{r}:{reader}");

                client.Send(encoding.GetBytes("收到！"));
            }
            client.Close();
        }
    }
}
