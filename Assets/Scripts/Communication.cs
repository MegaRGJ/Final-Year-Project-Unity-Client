using System.Collections.Generic;
using System.Threading;
// Includes which are system specific
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System;

public class Communication
{
    private IPEndPoint IPENDPOINT;
    private IPEndPoint LOCALENDPOINT;
    private int BYTE_LIMIT = 512;

    UdpClient UDPCLIENT;

    private List<byte[]> PACKET_LIST = new List<byte[]>();
    private readonly object LOCK = new object();
    

    public void Connect(string ip, int port)
    { 
        IPENDPOINT = new IPEndPoint(IPAddress.Parse(ip), port);
        LOCALENDPOINT = new IPEndPoint(IPAddress.Any, port);

        UDPCLIENT = new UdpClient();
        UDPCLIENT.Connect(IPENDPOINT);
    }

    public void Disconnect()
    {
        UDPCLIENT.Close();
    }

    public void SendData(List<byte[]> list)
    {
        foreach (var byteData in list)
        {
            UDPCLIENT.Send(byteData, byteData.Length);
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
        try
        {
            byte[] data;
            data = new byte[BYTE_LIMIT];
            UDPCLIENT.BeginReceive(new AsyncCallback(HandleReceive), null);
        }
        catch(Exception e)
        {
            var meme = e;
        }
    }

    public void HandleReceive(IAsyncResult data)
    {
        byte[] receivedBytes = UDPCLIENT.EndReceive(data, ref IPENDPOINT);

        lock (LOCK)
        {
            PACKET_LIST.Add(receivedBytes);
            Debug.Log("Packet Added!");
        }
        StartDataReceive();
    }
}


