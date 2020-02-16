using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Converter
{

    public static void AddBool(byte[] data, int pointer, bool value, out byte[] dataOut, out int pointerAfter)
    {
        dataOut = data;
        byte[] dataValue = BitConverter.GetBytes(value); //1 byte

        foreach (var element in dataValue)
        {
            dataOut[pointer] = element;
            pointer++;
        }
        pointerAfter = pointer;
    }

    public static bool GetBool(byte[] data, int pointer, out int pointerAfter)
    {
        bool value = BitConverter.ToBoolean(data, pointer);
        pointerAfter = pointer + 1;

        return value;
    }

    public static void AddInteger32(byte[] data, int pointer, int value, out byte[] dataOut, out int pointerAfter)
    {
        dataOut = data;
        byte[] dataValue = BitConverter.GetBytes(value); //4 bytes

        foreach (var element in dataValue)
        {
            dataOut[pointer] = element;
            pointer++;
        }
        pointerAfter = pointer;
    }

    public static void AddQuaternion(byte[] data, int pointer, Quaternion value, out byte[] dataOut, out int pointerAfter)
    {
        Converter.AddFloat(data, pointer, value.x, out data, out pointer);
        Converter.AddFloat(data, pointer, value.y, out data, out pointer);
        Converter.AddFloat(data, pointer, value.z, out data, out pointer);
        Converter.AddFloat(data, pointer, value.w, out data, out pointer);
        dataOut = data;
        pointerAfter = pointer;
    }

    public static int GetInteger32(byte[] data, int pointer, out int pointerAfter)
    {
        int value = BitConverter.ToInt32(data, pointer);
        pointerAfter = pointer + 4;

        return value;
    }

    /*
    public static void CastFloatInt32(ref float f, out int i)
    {
        unsafe
        {
            fixed (float* ptr = &f)
                i = *(int*)ptr;
        }
    }
    public static void CastInt32Float(ref int i, out float f)
    {
        unsafe
        {
            fixed (int* ptr = &i)
                f = *(float*)ptr;
        }
    }
    */

    public static void AddFloat(byte[] data, int pointer, float value, out byte[] dataOut, out int pointerAfter)
    {
        dataOut = data;
        byte[] dataValue = BitConverter.GetBytes(value); //4 bytes

        foreach (var element in dataValue)
        {
            dataOut[pointer] = element;
            pointer++;
        }
        pointerAfter = pointer;
    }

    public static float GetFloat(byte[] data, int pointer, out int pointerAfter)
    {
        float value = BitConverter.ToSingle(data, pointer);
        pointerAfter = pointer + 4;

        return value;
    }

    public static Quaternion GetQuaternion(byte[] data, int pointer, out int pointerAfter)
    {
        float x = Converter.GetFloat(data, pointer, out pointer);
        float y = Converter.GetFloat(data, pointer, out pointer);
        float z = Converter.GetFloat(data, pointer, out pointer);
        float w = Converter.GetFloat(data, pointer, out pointer);
        pointerAfter = pointer;

        return new Quaternion(x, y, z, w);
    }

    public static List<int> GetSerializableState(byte[] data, int pointer, out int pointerOut)
    {
        List<int> values = new List<int>();
        values.Add(Converter.GetInteger32(data, pointer, out pointer));
        values.Add(Converter.GetInteger32(data, pointer, out pointer));
        values.Add(Converter.GetInteger32(data, pointer, out pointer));
        pointerOut = pointer;
        return values;
    }

    public static List<float> GetSerializableGameObjectState(byte[] data, int pointer, out int pointerOut)
    {
        List<float> values = new List<float>();
        values.Add(Converter.GetFloat(data, pointer, out pointer));
        values.Add(Converter.GetFloat(data, pointer, out pointer));
        values.Add(Converter.GetFloat(data, pointer, out pointer));
        pointerOut = pointer;
        return values;
    }
}
