using System.Collections;
using System.Collections.Generic;
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
            gameObject.SetActive(false);
        }
        
    }
}
