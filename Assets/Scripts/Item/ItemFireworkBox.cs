using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFireworkBox : BaseItemBox {

    [FMODUnity.EventRef]
    public string getSound;
    [FMODUnity.EventRef]
    public string spawnSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        if (other.CompareTag("Player"))
        {
            int itemIndex = GameManagerPhoton._instance.itemManager.GetRandomItem(tableIndex);

            FireworkExecuter executer = other.GetComponent<FireworkExecuter>();

            if(executer != null)
            {
                executer.photonView.RPC("ChangeFirework", PhotonTargets.All, tableIndex, itemIndex);
            }
            else
            {
                Debug.LogError("Executer가 null 입니다");
            }

            photonView.RPC("SetActiveItemBox", PhotonTargets.All, false);
            GameManagerPhoton._instance.itemManager.curBoxCount--;
        }
    }

    [PunRPC]
    public void SetActiveItemBox(bool active)
    {
        if (active)
        {
            gameObject.SetActive(true);
            alive = true;
            GetComponent<Collider>().enabled = true;
            FMODUnity.RuntimeManager.PlayOneShot(spawnSound);
        }
        else
        {
            alive = false;
            GetComponent<Collider>().enabled = false;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot(getSound);
        }
    }
}
