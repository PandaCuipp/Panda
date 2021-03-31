using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyScoketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Client!");

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            try
            {
                int c = 0;
                while (!client.Connected && c < 10)
                {
                    Console.WriteLine($"尝试连接...");
                    try
                    {

                        client.Connect(IPAddress.Parse("127.0.0.1"), 9001);
                    }
                    catch (SocketException ex)
                    {

                    }
                    finally
                    {
                        Thread.Sleep(3000);
                        c++;
                    }

                }

                if (!client.Connected)
                {
                    Console.WriteLine("连接失败");
                }

                Console.WriteLine("连接成功");

                new Thread(a => ReceiveHandle(client)).Start();

                var str = "";
                while (str != "over")
                {
                    str = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        byte[] buff = new byte[1024];
                        buff = Encoding.UTF8.GetBytes(str);
                        client.Send(buff);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                client.Close();
            }

            Console.ReadKey();
        }

        static void ReceiveHandle(object s)
        {
            var client = (Socket)s;

            string reader = "";
            while (reader != "over")
            {
                byte[] buff = new byte[1024];

                var r = client.Receive(buff);
                reader = Encoding.UTF8.GetString(buff, 0, r);
                Console.WriteLine($"{reader}");

            }
            client.Close();
        }

    }
}
