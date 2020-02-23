using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Networking.Transport;
using UnityEngine;

public static class NetworkUtils
{
    public enum MessageTypes
    {
        DEFAULT = -1,
    }


    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return null;
    }


    public static void ParseConnection(string input, out string ip, out ushort port)
    {
        string[] inputArray = input.Split(':');
        ip = inputArray[0];
        port = Convert.ToUInt16(inputArray[1]);
    }


    /// <summary>
    /// reads a package
    /// </summary>
    /// <param name="stream">stream of the package</param>
    /// <returns>byte array of the package data</returns>
    public static byte[] ReadPackage(DataStreamReader stream)
    {
        DataStreamReader.Context readerCtx = default(DataStreamReader.Context);
        /*
        //Read Header
        int dataLength = BitConverter.ToInt32(stream.ReadBytesAsArray(ref readerCtx, 4), 0);

        //get Data from Package
        return stream.ReadBytesAsArray(ref readerCtx, dataLength);*/
        return stream.ReadBytesAsArray(ref readerCtx, 4);
    }

    public static byte[] AddStateHeader(byte[] data, int type)
    {
        byte[] dataType = BitConverter.GetBytes(type); //4 bytes
        byte[] dataLengthByte = BitConverter.GetBytes(data.Length); //4 bytes

        byte[] dataWithHeader = new byte[data.Length + dataType.Length + dataLengthByte.Length];
        dataType.CopyTo(dataWithHeader, 0);
        dataLengthByte.CopyTo(dataWithHeader, dataType.Length);
        data.CopyTo(dataWithHeader, dataType.Length + dataLengthByte.Length);

        return dataWithHeader;
    }


    public static void readStateHeader(byte[] data, int pointer, out int pointerOut, out int type, out int dataLength)
    {
        type = BitConverter.ToInt32(data, pointer);
        pointer += 4;
        dataLength = BitConverter.ToInt32(data, pointer);
        pointer += 4;
        pointerOut = pointer;
    }

    public static byte[] ConcatByteArrays(byte[] arr1, byte[] arr2)
    {
        int length = arr1.Length + arr2.Length;
        byte[] newArr = new byte[length];
        arr1.CopyTo(newArr, 0);
        arr2.CopyTo(newArr, arr1.Length);

        return newArr;
    }
}