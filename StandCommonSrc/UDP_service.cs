/**********************************************************************
    NomeFile : StandFacile/Udp_Service.cs
    Data	 : 24.04.2026
    Autore   : Mauro Artuso

    gestione delle notifiche UDP tra le postazioni:
    UDP a differenza di TCP non richiede la connessione tra i nodi,
    per contro non garantisce la consegna dei messaggi
 ***********************************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static StandCommonFiles.ComDef;
using static StandCommonFiles.CommonCl;

using static StandCommonFiles.LogServer;
using StandFacile;

namespace StandCommonFiles
{
    /// <summary>classe per la gestione delle notifiche UDP</summary>
    public class UdpBroadcastService
    {
        /// <summary>porta server UDP</summary>
        public readonly int UDP_SRV_PORT = 55024;
        /// <summary>porta client UDP</summary>
        public readonly int UDP_CLT_PORT = 55025;

        private readonly UdpBroadcastServer _server;
        private readonly UdpBroadcastClient _client;

        /// <summary>riferimento a FrmMain</summary>
        public static UdpBroadcastService rUdpService;

        static Random _iRandDelay = new Random();

        /// <summary>Costruttore</summary>
        public UdpBroadcastService()
        {
            rUdpService = this;

            _server = new UdpBroadcastServer(UDP_SRV_PORT);
            _client = new UdpBroadcastClient(UDP_CLT_PORT);

            _server.MessageReceived += OnServerMessage;
            _client.MessageReceived += OnClientMessage;

            Start_UDP();
        }

        /// <summary>starts UDP server and client</summary>
        public async void Start_UDP()
        {
            _server.Start();
            _client.Start();

            //await _client.SendAsync("Ciao !");
            await SendFromClient("Ciao !");
        }

        /// <summary>stops UDP server and client</summary>
        public void Stop_UDP()
        {
            _server.Stop();
            _client.Stop();
        }

        private void OnServerMessage(IPEndPoint ep, string msg)
        {
            LogToFile(String.Format($"[UDP-SERVER] {ep}, {msg}"), true);

#if STAND_MONITOR
            // gli eventi corretti per il refresh sono: ticket emesso dalla cassa principale, aggiornamento dati cassa secondaria,
            // ordine evaso da StandOrdini
            if (msg.Contains(UDP_EVENTS.PRI_CASHDESK_TICKET_EVENT) || msg.Contains(UDP_EVENTS.SEC_CASH_DATA_UPDATE_EVENT) ||
                msg.Contains(UDP_EVENTS.ORDER_COMPLETED_EVENT))
            {
                StandFacile.FrmMain.SetShortUpdateTimeout(_iRandDelay.Next(4, 10)); // *250ms, 1-2.5s
            }

#elif STAND_CUCINA
            if (msg.Contains(UDP_EVENTS.PRI_CASHDESK_TICKET_EVENT) || msg.Contains(UDP_EVENTS.SEC_CASHDESK_TICKET_EVENT))
            {
                FrmMain.rFrmMain.SetShortUpdateTimeout(2);
            }

#elif STANDFACILE
            // DataManager.AggiornaDisponibilità(); // chiamata diretta genera eccezione cross thread, 
            // bisogna usare il timer di FrmMain per aggiornare i dati di disponibilità e vendita
            if (msg.Contains(UDP_EVENTS.SEC_CASHDESK_TICKET_EVENT) && (glb.SF_Data.iNumCassa == CASSA_PRINCIPALE))
            {
                FrmMain.SetShortDispUpdateTimeout(2);
            }
            else if (msg.Contains(UDP_EVENTS.PRICELIST_MODIFIED_EVENT) && (glb.SF_Data.iNumCassa != CASSA_PRINCIPALE))
            {
                WarningManager(WRN_PRLM);
            }
#endif            
        }

        private void OnClientMessage(IPEndPoint ep, string msg)
        {
            Console.WriteLine($"[UDP-CLIENT] {ep}, {msg}");
        }

        /// <summary>sends a message from the UDP client</summary>
        public Task SendFromClient(string message)
        {
            return _client.SendBroadcastAsync(UDP_SRV_PORT, message);
        }
    }

    /// <summary>classe server per ricezione di pacchetti UDP</summary>
    class UdpBroadcastServer
    {
        private readonly int _port;
        private UdpClient _udp;
        private CancellationTokenSource _cts;

        public event Action<IPEndPoint, string> MessageReceived;

        public bool IsRunning => _udp != null;

        public UdpBroadcastServer(int port)
        {
            _port = port;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            _udp = new UdpClient();
            _udp.EnableBroadcast = true;
            _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udp.Client.Bind(new IPEndPoint(IPAddress.Any, _port));

            _cts = new CancellationTokenSource();

            Task.Run(() => ReceiveLoop(_cts.Token));
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            _cts.Cancel();
            _udp.Close();
            _udp = null;
        }

        private async Task ReceiveLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await _udp.ReceiveAsync();
                    string text = Encoding.UTF8.GetString(result.Buffer);

                    MessageReceived?.Invoke(result.RemoteEndPoint, text);
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignora: chiusura socket
            }
        }

        public async Task SendBroadcastAsync(int clientPortparam, string message)
        {
            if (!IsRunning)
                return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            await _udp.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, clientPortparam));
        }
    }


    /// <summary>classe client per l'invio di pacchetti UDP</summary>
    public class UdpBroadcastClient
    {
        private readonly int _port;

        private UdpClient _udp;
        private CancellationTokenSource _cts;

        /// <summary>messaggio ricevuto dal server</summary>
        public event Action<IPEndPoint, string> MessageReceived;

        /// <summary>classe client per l'invio di pacchetti UDP</summary>
        bool IsRunning => _udp != null;

        /// <summary>configura il nodo UDP</summary>
        public UdpBroadcastClient(int serverPort)
        {
            _port = serverPort;
        }

        /// <summary>avvia il nodo UDP</summary>
        public void Start()
        {
            if (IsRunning)
                return;

            _udp = new UdpClient();
            _udp.EnableBroadcast = true;
            _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udp.Client.Bind(new IPEndPoint(IPAddress.Any, _port));

            _cts = new CancellationTokenSource();
            Task.Run(() => ReceiveLoop(_cts.Token));
        }

        /// <summary>arresta il nodo UDP</summary>
        public void Stop()
        {
            if (!IsRunning)
                return;

            _cts.Cancel();
            _udp.Close();
            _udp = null;
        }

        private async Task ReceiveLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await _udp.ReceiveAsync();
                    string text = Encoding.UTF8.GetString(result.Buffer);

                    MessageReceived?.Invoke(result.RemoteEndPoint, text);
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignora
            }
        }

        /// <summary>invio del messaggio UDP</summary>
        public async Task SendBroadcastAsync(int serverPortparam, string message)
        {
            if (!IsRunning)
                return;

            byte[] data = Encoding.UTF8.GetBytes(message);
            await _udp.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, serverPortparam));
        }
    }

}
