using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTool : MonoBehaviour {

    public Text debugText;

    bool debugEnable = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote)) {
            if(debugEnable)
            {
                debugEnable = false;
                debugText.gameObject.SetActive(false);
                Debug.Log("Debug Mode Disable");
            }
            else
            {
                debugEnable = true;
                debugText.gameObject.SetActive(true);
                Debug.Log("Debug Mode Enable");
            }
            
        }

        if (!debugEnable)
            return;

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

    private void SetFireworks(int fireworkNum)
    {
        FireworkExecuter executer = GameManagerPhoton._instance.GetPlayerByOwnerId(PhotonNetwork.player.ID).GetComponent<FireworkExecuter>();

        executer.photonView.RPC("ChangeFirework", PhotonTargets.All, 0, fireworkNum);
    }
}
