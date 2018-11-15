using System;
using System.IO;
using UnityEngine;

public class ConfigManager {

    private static string ReadLine(int lineNum)
    {
        string line = null;

        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + "\\config.txt");

            for (int i = 0; i <= lineNum; i++)
            {
                line = sr.ReadLine();

                if (string.IsNullOrEmpty(line) || i == lineNum)
                {
                    break;
                }
            }

            sr.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
        }

        return line;
    }

    public static string ReadValue(int lineNum)
    {
        string source = ReadLine(lineNum);

        if (string.IsNullOrEmpty(source)) return null;

        string[] values = source.Split(':');

        return values[1];
    }

    public static bool ReadValue(string key, out string value)
    {
        string source = null;
        value = null;

        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + "\\config.txt");

            while (true)
            {
                source = sr.ReadLine();

                if (string.IsNullOrEmpty(source))
                {
                    break;
                }

                string[] values = source.Split(':');

                if (values[0].Equals(key))
                {
                    value = values[1];
                    sr.Close();
                    return true;
                }
                
            }

            sr.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
            return false;
        }
    }

    public static bool ReadBool(int lineNum)
    {
        string source = ReadLine(lineNum);

        if (string.IsNullOrEmpty(source))
        {
            Debug.LogError("데이터가 존재하지 않습니다.");
            return false;
        }

        string[] values = source.Split(':');

        if(values[1] == "true")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool ReadBool(string key, out bool value)
    {
        string source = null;
        value = false;

        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + "\\config.txt");

            while (true)
            {
                source = sr.ReadLine();

                if (string.IsNullOrEmpty(source))
                {
                    break;
                }

                string[] values = source.Split(':');

                if (values[0].Equals(key))
                {
                    if(values[1] == "true")
                    {
                        value = true;
                    }
                    sr.Close();
                    return true;
                }
            }

            sr.Close();
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
            return false;
        }
    }


}
