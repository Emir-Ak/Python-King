using System;
using System.Text;

public class AZProtection {
    public static string SetString(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        string hex = BitConverter.ToString(bytes);

        return hex.Replace("-", "");

    }

    public static string GetString(string data)
    {
        int charsCount = data.Length;
        byte[] bytes = new byte[charsCount / 2];
        for (int i = 0; i < charsCount; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
        }

        data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

        return data;
    }
}
public struct SecureInt
{
    private int offset;
    private int value;

    public SecureInt(int value)
    {
        offset = UnityEngine.Random.Range(-1000, 1000);
        this.value = value + offset;
    }

    public int GetValue()
    {
        return value - offset;
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    public static SecureInt operator +(SecureInt d1, SecureInt d2)
    {
        return new SecureInt(d1.GetValue() + d2.GetValue());
    }

    public static SecureInt operator -(SecureInt d1, SecureInt d2)
    {
        return new SecureInt(d1.GetValue() - d2.GetValue());
    }

    public static SecureInt operator *(SecureInt d1, SecureInt d2)
    {
        return new SecureInt(d1.GetValue() * d2.GetValue());
    }

    public static SecureInt operator /(SecureInt d1, SecureInt d2)
    {
        return new SecureInt(d1.GetValue() / d2.GetValue());
    }
}

public struct SecureFloat
{
    private float offset;
    private float value;

    public SecureFloat(float value)
    {
        offset = UnityEngine.Random.Range(-1000, 1000);
        this.value = value + offset;
    }

    public float GetValue()
    {
        return value - offset;
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    public static SecureFloat operator +(SecureFloat d1, SecureFloat d2)
    {
        return new SecureFloat(d1.GetValue() + d2.GetValue());
    }

    public static SecureFloat operator -(SecureFloat d1, SecureFloat d2)
    {
        return new SecureFloat(d1.GetValue() - d2.GetValue());
    }

    public static SecureFloat operator *(SecureFloat d1, SecureFloat d2)
    {
        return new SecureFloat(d1.GetValue() * d2.GetValue());
    }

    public static SecureFloat operator /(SecureFloat d1, SecureFloat d2)
    {
        return new SecureFloat(d1.GetValue() / d2.GetValue());
    }
}

public struct SecureDouble
{
    private double offset;
    private double value;

    public SecureDouble(double value)
    {
        offset = UnityEngine.Random.Range(-1000, 1000);
        this.value = value + offset;
    }

    public double GetValue()
    {
        return value - offset;
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    public static SecureDouble operator +(SecureDouble d1, SecureDouble d2)
    {
        return new SecureDouble(d1.GetValue() + d2.GetValue());
    }

    public static SecureDouble operator -(SecureDouble d1, SecureDouble d2)
    {
        return new SecureDouble(d1.GetValue() - d2.GetValue());
    }

    public static SecureDouble operator *(SecureDouble d1, SecureDouble d2)
    {
        return new SecureDouble(d1.GetValue() * d2.GetValue());
    }

    public static SecureDouble operator /(SecureDouble d1, SecureDouble d2)
    {
        return new SecureDouble(d1.GetValue() / d2.GetValue());
    }
}
