using System;
using System.Collections.Generic;

public struct ServerPlayerPacket
{
    public int PlayerID;
    public float X;
    public float Y;
    public float Z;
    public float Rotation;
    public string Username;
}

public struct ServerAcknowledgementPacket
{
    public int ClientID;
}

public class Serialisation
{
    int POSITION_ID = 1;
    int CONNECT_ID = 2;
    int DISCONNECT_ID = 3;
    int ACKNOWLEDGMENT_ID = 4;

    public byte[] SerialiseConnectData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(CONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));

        return byteList.ToArray();
    }

    public byte[] SerialiseDisconnecetData(int clientID)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(DISCONNECT_ID)); //4 bytes
        byteList.AddRange(BitConverter.GetBytes(clientID));

        return byteList.ToArray();
    }

    public byte[] SerialisePositionData (float x, float y, float z, float r, int playerID)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(POSITION_ID));
        byteList.AddRange(BitConverter.GetBytes(playerID));
        byteList.AddRange(BitConverter.GetBytes(x));
        byteList.AddRange(BitConverter.GetBytes(y));
        byteList.AddRange(BitConverter.GetBytes(z));
        byteList.AddRange(BitConverter.GetBytes(r));

        return byteList.ToArray();

    }

    public int DeserialisePacketType(byte[] packet)
    {
        return BitConverter.ToInt32(packet, 0);
    }

    public ServerPlayerPacket DeserialiseServerPositionPacket(byte[] packet)
    {
        ServerPlayerPacket p = new ServerPlayerPacket();
        
        p.PlayerID = BitConverter.ToInt32(packet, 4);
        p.X = BitConverter.ToSingle(packet, 8);
        p.Y = BitConverter.ToSingle(packet, 12);
        p.Z = BitConverter.ToSingle(packet, 16);
        p.Rotation = BitConverter.ToSingle(packet, 20);
        p.Username = BitConverter.ToString(packet, 24);

        return p;
    }

    public ServerAcknowledgementPacket DeserialiseServerAcknowledgementPacket(byte[] packet)
    {
        ServerAcknowledgementPacket p = new ServerAcknowledgementPacket();
        p.ClientID = BitConverter.ToInt32(packet, 4);

        return p;
    }
}
