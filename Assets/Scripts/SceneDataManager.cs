using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataManager : MonoBehaviour {

    public static SceneDataManager _instance;


    public int mapNum;
    public bool isSelect;

    private void Awake()
    {
        if(_instance == null)
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


    public void SelectMap(int selectedMap)
    {
        if (selectedMap == 0)
        {
            isSelect = false;
            Debug.Log("Select Map: Random");
        }
        else
        {
            isSelect = true;
            mapNum = selectedMap - 1;
            Debug.LogFormat("Select Map:{0}", (GameMap)mapNum);
        }

    }
}
