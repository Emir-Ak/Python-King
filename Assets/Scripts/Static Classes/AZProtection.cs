using System;
using System.Text;
using System.Collections;
using UnityEngine;
public class AZProtection {
    /// <summary>
    /// Encrypts the string
    /// </summary>
    /// <param name="data">String data to encrypt</param>
    /// <returns></returns>
    public static string SetString(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        string hex = BitConverter.ToString(bytes);

        return hex.Replace("-", "");

    }

    /// <summary>
    /// Decrypts string that has been encrypted by AZProtection.SetString()
    /// </summary>
    /// <param name="data">String data to decrypt</param>
    /// <returns></returns>
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

    public static bool isCheatEngineDetected = false;
    /// <summary>
    /// Detects harmful processes, CheatEngine and Artmoney for now
    /// </summary>
    /// <param name="timeBetweenChecks">Time between each check</param>
    /// <param name="closeApp">Should app be closed if any were detected</param>
    /// <param name="delay">Time in seconds before detecting starts</param> 
    public static IEnumerator DetectCheatProcessesCoroutine(float timeBetweenChecks = 10f,bool closeApp = true, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    if (pro.ProcessName.ToLower().Contains("cheat") || pro.ProcessName.ToLower().Contains("engine") || pro.ProcessName.ToLower().Contains("art") && pro.ProcessName.ToLower().Contains("money"))
                    {
                        Debug.Log(pro.ProcessName);
                    }
                }
                catch
                {
                    Debug.Log("Skiped");
                }
            }

            yield return new WaitForSeconds(timeBetweenChecks);
        }
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
    public static bool operator >(SecureInt d1, SecureInt d2)
    {
        if (d1.GetValue() > d2.GetValue())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool operator <(SecureInt d1, SecureInt d2)
    {
        if (d1.GetValue() < d2.GetValue())
        {
            return true;
        }
        else
        {
            return false;
        }
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
