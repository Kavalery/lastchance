using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lastchance
{
    [Serializable]
    public class Port
    {
        public int PortNumber { get; set; }
        public bool Res { get; set; }
    }
    [Serializable]
    public class Host
    {
        public string HostName { get; set; }
        public List<Port> Ports_list { get; set; }
    }
    
    public class HostsL
    {
        public List<Host> Hosts_list = new List<Host>();
    }
    public class Program
    {
        //static  Host _host = new Host();

        //static  Hosts hosts = new Hosts();
        static public HostsL hostsL = new HostsL();

        static public List<int> Ports_list = new List<int>() { 20, 21, 22, 23, 25, 53, 80, 110, 135, 137, 138, 139, 143, 149, 194, 443, 593, 992, 993, 994, 995, 8000, 8080, 3128, 3389, 6588, 1080, 5900, 8888, 27015 };

        static public List<string> IP_list = new List<string>() { "212.193.157.190", "212.193.149.139" };

        static public HostsL DeSerial(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(HostsL));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return (HostsL)xmlSerializer.Deserialize(fs);
            }
        }

        static public void PrintInfo(HostsL hostsL)
        {
            foreach(Host host in hostsL.Hosts_list)
            {
                Console.WriteLine(" "+host.HostName);
                foreach (Port port in host.Ports_list)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("   "+port.PortNumber);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        static public void ViewInfo()
        {
           string[] str=  Directory.GetFiles("_info/");
            foreach (string str2 in str)
            {
               HostsL hosts = DeSerial(str2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(str2);
                Console.ForegroundColor = ConsoleColor.White;
                PrintInfo(hosts);
            }
                
            Console.ReadLine();
        }

        static public void Add_port(int Port)
        {
            Ports_list.Add(Port);
        }
        static public void AddRangeToHosts()
        {
            foreach (string Host in IP_list)
            {
                Host host = new Host();
                host.HostName = Host;
                List<Port> ports = new List<Port>();
                host.Ports_list = ports;              
                hostsL.Hosts_list.Add(host);
            }
        }
        static public void Serial(HostsL _hosts)
        {
            string fileName= DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")+".xml";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(HostsL));
            using (FileStream fs = new FileStream("_info/"+fileName, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, _hosts);
            }
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            AddRangeToHosts();
            string Type_scan;
        tryagain:
            Console.WriteLine("Сканировать 30 самых популярных (введите 1) портов или все? (введите 2)\nЧтобы просмотреть отчет о портах нажмите 3");

            Type_scan = Console.ReadLine();
            if (Type_scan == "1")
            {
                foreach (Host _1host in hostsL.Hosts_list)
                {
                    Parallel.ForEach(Ports_list, i =>
                    {
                        Port _port = new Port();
                        bool _res = Scanport(i, _1host.HostName);
                        if (_res == true)
                        {
                            _port.PortNumber = i;
                            _port.Res = true;
                            _1host.Ports_list.Add(_port);
                        }
                    });
                }
                Serial(hostsL);
            }
            else
            if (Type_scan == "2")
            {
                Ports_list.Clear();
                for (int i = 1; i < 65537; i++)
                {
                    Ports_list.Add(i);
                    //кринж ну ладно
                }
                foreach (Host _1host in hostsL.Hosts_list)
                {
                    Parallel.ForEach(Ports_list, i =>
                    {
                        Port _port = new Port();
                        bool _res = Scanport(i, _1host.HostName);
                        if (_res == true)
                        {
                            _port.PortNumber = i;
                            _port.Res = true;
                            _1host.Ports_list.Add(_port);
                        }
                    });
                }
                Serial(hostsL);
            }
            else if(Type_scan == "3")
            {
                ViewInfo();
            }
            else
            {
                Console.Clear();
                goto tryagain;
            }
            Console.WriteLine("Finally!");
            Console.ReadLine();
        }
        static public bool Scanport(int Port, string Ip_addr)
        {
            string rez = string.Empty;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(Ip_addr, Port);
                rez = s.Connected.ToString();
                if (rez == "True")
                {
                    Console.WriteLine("Для IP = " + Ip_addr + " Порт " + Port + " открыт");
                }
            }
            catch
            {
            }
            if (rez == "True")
                return true;
            else
                return false;
        }

    }
}
