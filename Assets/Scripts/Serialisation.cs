using System;
using System.Collections.Generic;

public struct ServerPositionPacket
{
    public int PlayerID;
    public float X;
    public float Y;
    public float Z;
    public float Rotation;
}

public class Serialisation
{
    private int POSITION_ID = 1;
    private int CONNECT_ID = 2;
    private int DISCONNECT_ID = 3;

    public byte[] SerialiseConnectData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(CONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));

        return byteList.ToArray();
    }

    public byte[] SerialiseDisconnecetData(string username)
    {
        List<byte> byteList = new List<byte>();

        byteList.AddRange(BitConverter.GetBytes(DISCONNECT_ID)); //4 bytes
        byteList.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes(username));

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

    public object DeserialiseServerPacket(byte[] packet)
    {
        ServerPositionPacket p = new ServerPositionPacket();

        p.PlayerID = BitConverter.ToInt32(packet, 0);
        p.X = BitConverter.ToSingle(packet, 4);
        p.Y = BitConverter.ToSingle(packet, 8);
        p.Z = BitConverter.ToSingle(packet, 12);
        p.Rotation = BitConverter.ToSingle(packet, 16);

        return p;
    }
}
