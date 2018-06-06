using System;
using System.Net;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            string urlPrefix = "file://" + Application.dataPath + "/../../Logs/"; // Coelacanth/Assets/Logs/
        }
        
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
        }
    }
}