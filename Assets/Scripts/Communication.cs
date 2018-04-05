using System.Collections.Generic;
using System.Threading;
// Includes which are system specific
using System.Net;
using System.Net.Sockets;

public class Communication
{
    private Socket CLIENT_SOCKET;
    private EndPoint IPENDPOINT;
    private EndPoint LOCALENDPOINT;
    private Thread RECEIVE_THREAD;
    private int BYTE_LIMIT = 512;

    private List<byte[]> PACKET_LIST = new List<byte[]>();
    private readonly object LOCK = new object();
    

    public void Connect(string ip, int port)
    {
        CLIENT_SOCKET = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        CLIENT_SOCKET.SendTimeout = 1;
        
        IPENDPOINT = new IPEndPoint(IPAddress.Parse(ip), port);
        LOCALENDPOINT = new IPEndPoint(IPAddress.Any, port);
    }

    public void Disconnect()
    {
        RECEIVE_THREAD.Abort();
        CLIENT_SOCKET.Close();
    }

    public void SendData(List<byte[]> list)
    {
        foreach (var byteData in list)
        {
            CLIENT_SOCKET.SendTo(byteData, IPENDPOINT);
        }
    }

    public List<byte[]> GetData()
    {
        List<byte[]> packetList;

        lock (LOCK)
        {
            packetList = PACKET_LIST;
            PACKET_LIST = new List<byte[]>();
        }

        return packetList;
    }

    public void StartDataReceive()
    {

        RECEIVE_THREAD = new Thread(() =>
        {
            byte[] data;

            while (true)
            {
                data = new byte[BYTE_LIMIT];

                CLIENT_SOCKET.ReceiveFrom(data, ref IPENDPOINT); // or Receive

                //lock (LOCK)
                //{
                    PACKET_LIST.Add(data);
             //   }
            }
        });
        RECEIVE_THREAD.Start();
    }
}

