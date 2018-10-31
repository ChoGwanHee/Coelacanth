using System.Collections;
using System.Collections.Generic;
using ServerModule;
using UnityEngine;

public class UIDebugButton : MonoBehaviour {

    private void Start()
    {
        if(!PhotonNetwork.isMasterClient || !ConfigManager.ReadBool(2))
        {
            gameObject.SetActive(false);
        }
    }

    public void ForceGameStart()
    {
        if (GameManagerPhoton._instance.GameStartRequest())
        {
            Debug.Log("GAME START");
            ServerManager.Send(string.Format("BTNSTART:{0}:{1}", InstanceValue.Room, true));
            gameObject.SetActive(false);
        }
        
    }
}