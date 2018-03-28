using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
// Includes which are system specific
using System.Net;
using System.Net.Sockets;

public class Communication : MonoBehaviour
{
    private Socket CLIENT_SOCKET;
    private EndPoint IPENDPOINT;
    private Thread RECEIVE_THREAD;
    private int BYTE_LIMIT = 512;

    private int POSITION_ID = 0;
    private int CONNECT_ID = 1;
    private int DISCONNECT_ID = 2;

    public void Connect(string ip, int port)
    {
        CLIENT_SOCKET = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        CLIENT_SOCKET.SendTimeout = 1;

        IPENDPOINT = new IPEndPoint(IPAddress.Parse(ip), port);
    }

    public void Disconnect()
    {

    }

    public void SendData(List<byte[]> list)
    {
        foreach (var byteData in list)
        {
            CLIENT_SOCKET.SendTo(byteData, IPENDPOINT);
        }
    }

    public List<string> GetData()
    {

        RECEIVE_THREAD = new Thread(() =>
        {
            byte[] data;

            while (true)
            {
                data = new byte[BYTE_LIMIT];

                CLIENT_SOCKET.ReceiveFrom(data, ref IPENDPOINT);

                
                packetBuffer.AddPacket(packet);
            }
        });
        return new List<string>();
    }

    public byte[] CreateConnectData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(CONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));
        
        return byteList.ToArray();
    }

    public byte[] CreateDisconnecetData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(DISCONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));

        return byteList.ToArray();
    }

    public byte[] CreatePositionData(float x, float y, float z, string alphabet)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(POSITION_ID)); //4 bytes
        byteList.AddRange(BitConverter.GetBytes(x));
        byteList.AddRange(BitConverter.GetBytes(y));
        byteList.AddRange(BitConverter.GetBytes(z));
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(alphabet));

        return byteList.ToArray();

    }
}

