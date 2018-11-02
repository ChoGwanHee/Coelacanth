﻿using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager _instance;


    public int mapNum;
    public bool isSelect;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            _instance.isSelect = false;
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

}
