using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace lastchance
{
    public class Program
    {
        static public List<int> Ports_list = new List<int>()
        {
        20,21,22,23,25,53,80,110,135,137,138,139,143,149,194,443,593,992,993,994,995,8000,8080,3128,3389,6588,1080,5900,8888,27015
        };
        static public List<string> IP_list = new List<string>()
        {
        "212.193.157.190","212.193.149.139"
        };
        static public void Add_port(int Port)
        {
            Ports_list.Add(Port);

        }
        static void Main(string[] args)
        {
            

            string Type_scan;
            Console.WriteLine("Сканировать 30 самых популярных (введите 1) портов или все? (введите 2)");

            Type_scan = Console.ReadLine();
            if (Type_scan == "1")
            {
                foreach (string _IP in IP_list)
                {
                    foreach (int _Port in Ports_list)
                    {
                        Scanport(_Port, _IP);
                    }
                }
            }

            else
            if (Type_scan == "2")
            {
                Ports_list.Clear();
                for (int i = 1; i< 65537;i++)
                {
                    Ports_list.Add(i);
                    //кринж ну ладно
                }
                foreach (string _IP in IP_list)
                {
                    foreach (int _Port in Ports_list)
                    {
                        Scanport(_Port, _IP);

                    }
                }

            }
            else
            {
                Console.WriteLine("Неверный ввод");
                //todo
            }

            

            Console.ReadLine();
        }
        static public void Scanport(int Port, string Ip_addr)
        {
            Task.Run(() =>
            {

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    s.Connect(Ip_addr, Port);
                    string rez = s.Connected.ToString();
                    if (rez == "True")
                    {
                        Console.WriteLine("Для IP = " + Ip_addr + " Порт " + Port + " открыт");

                    }
                }
                catch
                {
                    //Console.WriteLine("Для IP = " + Ip_addr + " Порт " + Port + " закрыт");
                }
               
            });

            //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //try
            //{
            //    string rez = s.Connected.ToString();
            //    if (rez == "True")
            //    {
            //        Console.WriteLine("Порт " + Port + " открыт");
            //    }
            //}
            //catch
            //{
            //    Console.WriteLine("Порт " + Port + " закрыт");
            //}




            //return Port;
        }
    }
}
