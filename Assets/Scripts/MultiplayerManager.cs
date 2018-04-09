﻿using System.Collections;
using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour 
{
    private Communication COMMS;
    private Serialisation SERIAL;
    public string IP_ADDRESS = "127.0.0.1";
    public int PORT = 4739;
    public int MS_INTERVAL = 16; // 1000/ 60 = 16.6666666667
    public string USERNAME = "Mega";
    public GameObject PLAYER_CHARACTER;
    public bool IS_CONNECTED = false;
    private Transform PLAYER_TRANSFORM;

    private List<byte[]> PACKET_LIST = new List<byte[]>();
    private double LAST_SEND = 0;
    private int PLAYER_ID = -1;

    //Put these somewhere else
    private int SERVER_PLAYER_ID = 1;
    private int CONNECT_ID = 2;
    private int DISCONNECT_ID = 3;
    private int ACKNOWLEDGMENT_ID = 4;
    
    public GameObject OTHER_PLAYER_PREFAB;
    List<OtherPlayer> OTHER_PLAYERS = new List<OtherPlayer>();

    struct OtherPlayer
    {
        public int PlayerID;
        public GameObject Prefab;
        public float LastUpdate;
    }

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
        //COMMS.StartDataReceive();
    }

    public void SendConnectRequest()
    {
        COMMS.StartDataReceive();
        PACKET_LIST.Add(SERIAL.SerialiseConnectData(USERNAME));
    }

    public void SendDisconnectRequest()
    {
        PACKET_LIST.Add(SERIAL.SerialiseDisconnecetData(PLAYER_ID));
        COMMS.SendData(PACKET_LIST);
        COMMS.Disconnect();
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
        if (PACKET_LIST.Count != 0)
        {
            COMMS.SendData(PACKET_LIST);
            PACKET_LIST.Clear();
        }
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
            foreach (var serverPacket in packets)
            {
                int ID = SERIAL.DeserialisePacketType(serverPacket);

                if (ID == SERVER_PLAYER_ID)
                {
                    ServerPlayerPacket packet = SERIAL.DeserialiseServerPositionPacket(serverPacket);
                    DealWithPlayerPacket(packet);
                }
                else if (ID == CONNECT_ID)
                {

                }
                else if (ID == DISCONNECT_ID)
                {
                    
                }
                else if (ID == ACKNOWLEDGMENT_ID)
                {
                    ServerAcknowledgementPacket packet = SERIAL.DeserialiseServerAcknowledgementPacket(serverPacket);
                    PLAYER_ID = packet.ClientID;
                    IS_CONNECTED ^= true;
                }
            }
        }
        
    }

    private void DealWithPlayerPacket(ServerPlayerPacket packet)
    {
        try
        {
            int index = -1;

            if (OTHER_PLAYERS.Count != 0)
            {
                index = OTHER_PLAYERS.FindIndex(x => x.PlayerID == packet.PlayerID);
            }

            if (index < 0)
            {
                CreatePlayerPrefab(packet);
            }
            else
            {
                UpdatePlayerPrefab(index, packet);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void CreatePlayerPrefab(ServerPlayerPacket packet)
    {
        OtherPlayer player;
        player.PlayerID = packet.PlayerID;
        player.Prefab = Instantiate(OTHER_PLAYER_PREFAB);
        player.LastUpdate = Time.time;

        player.Prefab.transform.position = new Vector3(packet.X, packet.Y, packet.Z);
        player.Prefab.transform.rotation = new Quaternion(0, packet.Rotation, 0, 0);

        OTHER_PLAYERS.Add(player);
    }

    private void UpdatePlayerPrefab(int index, ServerPlayerPacket packet)
    {
        OTHER_PLAYERS[index].Prefab.transform.position = new Vector3(packet.X, packet.Y, packet.Z);
        OTHER_PLAYERS[index].Prefab.transform.rotation = new Quaternion(0, packet.Rotation, 0, 0);
    }
}
