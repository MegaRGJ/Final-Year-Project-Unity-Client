using System.Collections;
using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour 
{
    private int POSITION_ID = 0;
    private int CONNECT_ID = 1;
    private int DISCONNECT_ID = 2;

    private Communication COMMS;
    private Thread NETWORKSERVER_THREAD;
    public string IP_ADDRESS = "127.0.0.1";
    public int PORT = 4739;
    public int MS_INTERVAL = 16; // 1000/ 60 = 16.6666666667

    public GameObject PLAYER_CHARACTER;
    private Transform PLAYER_TRANSFORM;

    void Start ()
    {
        PLAYER_TRANSFORM = PLAYER_CHARACTER.GetComponent<Transform>();
        COMMS = new Communication();
        StartConnection();

        //UpdateServerWithClientData();
    }
	
	void Update ()
    {
        UpdateServerWithClientData();
    }

    private void StartConnection()
    {
        COMMS.Connect(IP_ADDRESS, PORT);
        //Send Connect Packet
    }

    List<byte[]> packetList = new List<byte[]>();
    double LastSend = 0;

    private void UpdateServerWithClientData()
    {
        if (LastSend + MS_INTERVAL <= GetMSTime())
        {
            LastSend = GetMSTime();

            //Get required data
            float x = this.PLAYER_TRANSFORM.position.x;
            float y = this.PLAYER_TRANSFORM.position.y;
            float z = this.PLAYER_TRANSFORM.position.z;
            float r = this.PLAYER_TRANSFORM.rotation.y;

            //Create any byte packets that needs sending.
            packetList.Add(CreatePositionData(x, y, z, r));

            //Send Packets
            COMMS.SendData(packetList);
            packetList.Clear();
        }
    }



    private double GetMSTime()
    {
        return (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
    }

    byte[] CreateConnectData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(CONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));

        return byteList.ToArray();
    }

    byte[] CreateDisconnecetData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(DISCONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));

        return byteList.ToArray();
    }

    byte[] CreatePositionData(float x, float y, float z, float r)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(POSITION_ID)); //4 bytes
        byteList.AddRange(BitConverter.GetBytes(x));
        byteList.AddRange(BitConverter.GetBytes(y));
        byteList.AddRange(BitConverter.GetBytes(z));
        byteList.AddRange(BitConverter.GetBytes(r));
        //byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(alphabet));

        return byteList.ToArray();

    }
}
