using System;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure.Tcp
{
    public class TcpCommunication
    {
        private readonly string _ipAddress;
        private readonly int _port;
        private string _lastData;

        public TcpCommunication(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public void StartTcpPolling()
        {
            TcpSendAndReceive("GET_DATA");
        }

        public void SendCommand(string command)
        {
            TcpSendAndReceive(command);
        }

        public void TcpSendAndReceive(string command)
        {
            try
            {
                using TcpClient client = new TcpClient(_ipAddress, _port);
                NetworkStream stream = client.GetStream();

                byte[] dataToSend = Encoding.ASCII.GetBytes(command);
                Console.WriteLine($"Sent: {command}");

                // Veriyi göndermek için akışı kullan
                stream.Write(dataToSend, 0, dataToSend.Length);

                // Yanıtı almak için akışı kullan
                byte[] dataToReceive = new byte[256];
                int bytes = stream.Read(dataToReceive, 0, dataToReceive.Length);
                _lastData = Encoding.ASCII.GetString(dataToReceive, 0, bytes);
                Console.WriteLine($"Received: {_lastData}");
            }
            catch (SocketException socketEx)
            {
                // TCP bağlantı hatası
                Console.WriteLine($"TCP Socket Exception: {socketEx.Message}");
            }
            catch (Exception ex)
            {
                // Genel hata yakalama
                Console.WriteLine($"TCP Communication Exception: {ex.Message}");
            }
        }

        public string GetLastData()
        {
            return _lastData;
        }
    }
}
