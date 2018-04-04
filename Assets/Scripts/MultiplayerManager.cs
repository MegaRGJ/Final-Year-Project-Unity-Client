using System.Collections;
using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour 
{
    private Communication COMMS;
    private Serialisation SERIAL;

    private Thread NETWORKSERVER_THREAD;
    public string IP_ADDRESS = "127.0.0.1";
    public int PORT = 4739;
    public int MS_INTERVAL = 16; // 1000/ 60 = 16.6666666667
    public string USERNAME = "Mega";

    public GameObject PLAYER_CHARACTER;
    public bool IS_CONNECTED = false;
    private Transform PLAYER_TRANSFORM;
    private List<byte[]> PACKET_LIST = new List<byte[]>();
    private double LAST_SEND = 0;
    private int PLAYER_ID;
    
    void Start ()
    {
        PLAYER_TRANSFORM = PLAYER_CHARACTER.GetComponent<Transform>();
        COMMS = new Communication();
        SERIAL = new Serialisation();

        StartConnection();
    }
	
	void Update ()
    {
        if (LAST_SEND + MS_INTERVAL <= GetMSTime())
        {
            LAST_SEND = GetMSTime();

            UpdateServerWithClientData();

            UpdateClientWithServerData(); // Check in here if packets are missing?
        }
    }

    private void StartConnection()
    {
        COMMS.Connect(IP_ADDRESS, PORT);
        COMMS.StartDataReceive();
    }

    public void SendConnectRequest()
    {
        PACKET_LIST.Add(SERIAL.SerialiseConnectData(name));
    }

    private void UpdateServerWithClientData()
    {
        float x = PLAYER_TRANSFORM.position.x;
        float y = PLAYER_TRANSFORM.position.y;
        float z = PLAYER_TRANSFORM.position.z;
        float r = PLAYER_TRANSFORM.rotation.y;
        
        if (IS_CONNECTED)
        {
            PACKET_LIST.Add(SERIAL.SerialisePositionData(x, y, z, r, PLAYER_ID));
        }
        
        //Send Packets
        COMMS.SendData(PACKET_LIST);
        PACKET_LIST.Clear();
    }

    private double GetMSTime()
    {
        return (DateTime.Now - DateTime.MinValue).TotalMilliseconds;
    }

    private void UpdateClientWithServerData()
    {
        List<byte[]> packets = COMMS.GetData();

        if (packets.Count > 0)
        {
            string meme = "a";
        }
        //SERIAL.DeserialiseServerPacket();
    }
}
