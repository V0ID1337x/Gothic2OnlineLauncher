using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
/// <summary>
/// Przykładowe użycie jakbym zapomniał czy dał komuś libke.
/// Blok try został dodany tylko dlatego że klasa MasterList może zwracać wyjątki np. jak strona nie działa.
/// Jeśli chcesz tylko pobrać dane z serwerów należy zrobić array z obiektem Server zawierający adresy serwerów.
/*
             try
            {

                var ml = new MasterList("http://185.5.97.181:8000/master/public_servers/");
                var nl = new NetworkList();
                if (nl.InitSocket())
                {
                    nl.Start();
                    foreach (var value in ml.Servers)
                    {
                        Console.WriteLine("IP: {0} Port: {1}", value.ip, value.port);
                        nl.ServerData(value);
                    }
                    Thread.Sleep(3000);
                    nl.Stop();
                    foreach (var el in nl.ServerList)
                        Console.WriteLine(el.Value);
                }
            }
            catch(Exception e)
            {

            }
 */
/// </summary>
namespace ServerManager
{
    /// <summary>
    /// Adres serwera
    /// </summary>
    class ServerAddress
    {
        public string ip { get; set; }
        public UInt16 port { get; set; }

        public ServerAddress(string Ip, UInt16 Port)
        {
            ip = Ip; port = Port;
        }
        public ServerAddress() { }
        public override string ToString() { return $"{ip}:{port}"; }
    }
    /// <summary>
    /// Klasa umożliwiająca pobranie danych z lobby o podanym adresie URL
    /// </summary>
    class MasterList
    {
        /// <summary>
        /// Lista pobranych serwerów z listy internet
        /// </summary>
        public ServerAddress[] Servers;
        /// <summary>
        /// Czy lista serwerów została pobrana pomyślnie?
        /// </summary>
        public bool ListGot = false;
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="url">Adres www serwera lobby</param>
        public MasterList(string url)
        {
            using (var wc = new WebClient())
            {
                var resultJson = string.Empty;
                try {
                    resultJson = wc.DownloadString(url);
                    try {
                        Servers = new JavaScriptSerializer().Deserialize<ServerAddress[]>(resultJson);
                        ListGot = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Nie można skonwertować listy serwerów ze stringu Json!");
                        Console.WriteLine("Wyjątek: {0}", e);
                        throw e; 
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Nie można pobrać serwerów z listy! Wyjątek: {0}", e);
                    throw e;
                }
            }
        }
    }
    /// <summary>
    /// Wersja gry
    /// </summary>
    class Version
    {
        public int major { get; set; }
        public int minor { get; set; }
        public int patch { get; set; }
        public int build { get; set; }

        public override string ToString()
        {
            return $"{major}.{minor}.{patch}.{build}";
        }
    }
    /// <summary>
    /// Dane serwera
    /// </summary>
    class Server
    {
        public ServerAddress address = new ServerAddress("127.0.0.1", 28970);
        public Version version = new Version() { major = 0, minor = 0, patch = 0, build = 0 };
        public uint players = 0;
        public uint playersMax = 0;
        public string hostname = "n/a";
        public int ping = -1;
        public string world = "NEWWORLD\\NEWWORLD.ZEN";
        public string description = "n/a";

        public override string ToString() { return $"{hostname} {address.ToString()} {players}/{playersMax} Ping:{ping} World:{world} Version:{version.major}.{version.minor}.{version.patch}.{version.build}\n{description}"; }
    }
    /// <summary>
    /// Klasa używana do pobierania wysyłania zapytań i odbierania danych z serwerów
    /// </summary>
    class NetworkList
    {
        /// <summary>
        /// Klasa przechowywująca zapytania do serwerów
        /// </summary>
        private class Request
        {
            public ServerAddress address { get; set; }
            public int tickTime { get; set; }
        }
        private Socket socket;
        private Thread thread;
        private static readonly object @lock = new object();
        private Dictionary<string, Request> requests = new Dictionary<string, Request>();
        public Dictionary<string, Server> ServerList = new Dictionary<string, Server>();
        public Dictionary<string, Server> FailedList = new Dictionary<string, Server>();
        /// <summary>
        /// Czy wątek pracuje?
        /// </summary>
        private bool isRunning;

        public NetworkList()
        {
            thread = new Thread(new ThreadStart(Listener));
        }
        /// <summary>
        /// Tworzy nowy socket
        /// </summary>
        /// <returns>Jeśli nie wystąpił żaden wyjątek zwracamy true, w przeciwnym razie false</returns>
        public bool InitSocket()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Blocking = false;
                socket.EnableBroadcast = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Nie można utworzyć socketa! Wyjątek: {0}", e);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Rozpoczynamy proces oczekiwania na odpowiedzi serwerów
        /// </summary>
        public void Start()
        {
            // Czyścimy obie listy w przypadku gdy pobieranie danych wywołane jest kolejny raz
            ServerList.Clear();
            requests.Clear();
            FailedList.Clear();
            isRunning = true;
            thread.Start();
        }
        /// <summary>
        /// Wywołać gdy nastąpi koniec czasu przeznaczonego na odebranie danych z serwerów
        /// </summary>
        public void Stop()
        {
            lock (@lock)
            {
                isRunning = false;

                //Uzupełniamy listę nieudanych requestów
                var failed = requests.Where(n => !ServerList.ContainsKey(n.Value.address.ToString()));
                foreach(var el in failed)
                    FailedList[el.Value.address.ToString()] = new Server() { address = el.Value.address };
            }
        }
        /// <summary>
        /// Zapytanie o informacje o serwerze
        /// </summary>
        /// <param name="address"></param>
        private void Info(in ServerAddress address)
        {
            Send(address, "GOi");
        }
        /// <summary>
        /// Nieobsługiwany
        /// </summary>
        /// <param name="address"></param>
        private void Ping(in ServerAddress address)
        {
            Send(address, "GOp");
        }
        /// <summary>
        /// Zapytanie o szczegóły serwera (np. opis)
        /// </summary>
        /// <param name="address"></param>
        private void Details(in ServerAddress address)
        {
            Send(address, "GOd");
        }
        /// <summary>
        /// Od razu pobieramy kompletne dane serwera
        /// </summary>
        /// <param name="address"></param>
        public void ServerData(in ServerAddress address)
        {
            Info(address);
            Details(address);
        }
        public void ServerData(in string address)
        {
            var addr = address.Split(':');
            ServerData(new ServerAddress(addr[0], UInt16.Parse(addr[1])));
        }
        /// <summary>
        /// Bezpieczna funkcja do wysyłania zapytań do serwerów
        /// </summary>
        /// <param name="address">Adres ip i port serwera</param>
        /// <param name="packet">zapytanie</param>
        private void Send(ServerAddress address, in string packet)
        {
            lock (@lock)
            {
                IPAddress broadcast = IPAddress.Parse(address.ip);
                byte[] sendbuf = Encoding.ASCII.GetBytes(packet);
                IPEndPoint ep = new IPEndPoint(broadcast, address.port);
                socket.SendTo(sendbuf, ep);
                //Environment.TickCount działa dobrze tylko przez 25dni(?)
                requests[address.ToString()] = new Request() { address = address, tickTime = (Environment.TickCount & Int32.MaxValue) };
            }
        }
        /// <summary>
        /// Funkcja wątku służąca do "zbierania" odpowiedzi od serwerów
        /// </summary>
        private void Listener()
        {
            try
            {
                var buffer = new byte[512];
                var sender = new IPEndPoint(IPAddress.Any, 0);
                var senderRemote = sender as EndPoint;
                int len;

                while (isRunning)
                {
                    len = -1;
                    try
                    {
                        len = socket.ReceiveFrom(buffer, SocketFlags.None, ref senderRemote);
                        if (len <= 0)
                        {
                            Thread.Sleep(2);
                            continue;
                        }
                   
                        //Przetworzony zostaje tylko pakiet który został wcześniej wywołany przez request
                        if(requests.ContainsKey(senderRemote.ToString()))
                        {
                            var value = requests[senderRemote.ToString()];
                            ProcessPackets(value.address, buffer, len, (Environment.TickCount & Int32.MaxValue) - value.tickTime);
                        }
                        else
                        {
                            Thread.Sleep(2);
                            continue;
                        }
                    }
                    catch //To się wywołuje gdy ReceiveFrom nie odebrał danych w trybie non-blocking
                    {
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Problem z odbieraniem pakietów! Wyjątek: {e}");
            }
            finally
            {
                isRunning = false;
            }
        }
        /// <summary>
        /// Dodałem tę funkcję tylko dlatego że listener okazał się być za duży
        /// </summary>
        /// <param name="serverAddress">adres serwera</param>
        /// <param name="packet">pakiet</param>
        /// <param name="requestTime">ping</param>
        private void ProcessPackets(in ServerAddress serverAddress, in byte[] packet, in int packetLen, in int requestTime)
        {
            if (Encoding.ASCII.GetString(packet, 0, 2).StartsWith("GO"))
            {
                switch((char)packet[2])
                {
                    //Pakietu 'p' nie parsuje bo nigdy nie bedzie uzywany
                    case 'i':
                        {
                            if(packet[3] == 0x1) //PACKET_VERSION
                            {
                                if (!ServerList.ContainsKey(serverAddress.ToString()))
                                {
                                    ServerList[serverAddress.ToString()] = new Server()
                                    {
                                        address = serverAddress,
                                        version = new Version()
                                        {
                                            major = packet[4],
                                            minor = packet[5],
                                            patch = packet[6]
                                        },
                                        players = packet[7],
                                        playersMax = packet[8],
                                        ping = requestTime,
                                        hostname = Encoding.ASCII.GetString(packet, 9, packetLen - 9)
                                    };
                                }
                                else
                                {
                                    var el = ServerList[serverAddress.ToString()];
                                    el.address = serverAddress;
                                    el.version = new Version()
                                    {
                                        major = packet[4],
                                        minor = packet[5],
                                        patch = packet[6]
                                    };
                                    el.players = packet[7];
                                    el.playersMax = packet[8];
                                    el.ping = requestTime;
                                    el.hostname = Encoding.ASCII.GetString(packet, 9, packetLen - 9);
                                }
                            }
                        }
                        break;
                    case 'd':
                        {
                            if(packet[3] == 0x1) //PACKET_VERSION
                            {
                                int worldLen = packet[4];

                                if(!ServerList.ContainsKey(serverAddress.ToString()))
                                {
                                    ServerList[serverAddress.ToString()] = new Server()
                                    {
                                        address = serverAddress,
                                        world = Encoding.ASCII.GetString(packet, 5, worldLen),
                                        description = Encoding.ASCII.GetString(packet, 5 + worldLen, packetLen - 5 - worldLen)
                                    };
                                }
                                else
                                {
                                    var el = ServerList[serverAddress.ToString()];
                                    el.address = serverAddress;
                                    el.world = Encoding.ASCII.GetString(packet, 5, worldLen);
                                    el.description = Encoding.ASCII.GetString(packet, 5 + worldLen, packetLen - 5 - worldLen);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}
