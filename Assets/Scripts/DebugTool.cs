using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTool : MonoBehaviour {

    public static DebugTool _instance;

    public bool isGameScene = false;


    // 타이틀
    public GameObject mapSelectUI;

    public bool isSelect = false;
    public int mapNum = 0;


    // 게임 씬
    public Text debugText;
    public Text pingText;

    public bool debugEnable = false;
    


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote)) {
            if(debugEnable)
            {
                debugEnable = false;
                debugText.gameObject.SetActive(false);
                pingText.gameObject.SetActive(false);

                if (mapSelectUI != null && mapSelectUI.activeSelf)
                    mapSelectUI.SetActive(false);

                Debug.Log("Debug Mode Disable");
            }
            else
            {
                debugEnable = true;
                debugText.gameObject.SetActive(true);
                pingText.gameObject.SetActive(true);
                Debug.Log("Debug Mode Enable");
            }
            
        }

        if (!debugEnable)
            return;

        if (isGameScene)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetFireworks(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetFireworks(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetFireworks(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetFireworks(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetFireworks(5);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                mapSelectUI.SetActive(!mapSelectUI.activeSelf);
            }
        }

        pingText.text = "Ping: " + PhotonNetwork.GetPing().ToString();
    }

    private void SetFireworks(int fireworkNum)
    {
        FireworkExecuter executer = GameManagerPhoton._instance.GetPlayerByOwnerId(PhotonNetwork.player.ID).GetComponent<FireworkExecuter>();

        executer.photonView.RPC("ChangeFirework", PhotonTargets.All, 0, fireworkNum);
    }

    public void SelectMap(int selectedMap)
    {
        if(selectedMap == 0)
        {
            isSelect = false;
            Debug.Log("Select Map: random");
        }
        else
        {
            isSelect = true;
            mapNum = selectedMap - 1;
            Debug.LogFormat("Select Map:{0}", (GameMap)mapNum);
        }
        
    }
}
