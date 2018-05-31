﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UIManager : MonoBehaviour {
    public static UIManager _instance;


    public UIStatus userStatus;
    public UIButterflyCharging chargingUI;

    public Sprite[] illusts;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
}
