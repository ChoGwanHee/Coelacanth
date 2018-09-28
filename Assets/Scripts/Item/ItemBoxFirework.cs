using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:Assets/Scripts/Item/ItemFireworkBox.cs
public class ItemFireworkBox : BaseItemBox {
=======
public class ItemBoxFirework : BaseItemBox {
>>>>>>> ChaJinMin:Assets/Scripts/Item/ItemBoxFirework.cs

    [FMODUnity.EventRef]
    public string getSound;
    [FMODUnity.EventRef]
    public string spawnSound;
    

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        if (other.CompareTag("Player"))
        {
            int itemIndex = (base.itemIndex == -1) ? GameManagerPhoton._instance.itemManager.GetRandomItemIndex(tableIndex)
                : base.itemIndex;

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
<<<<<<< HEAD:Assets/Scripts/Item/ItemFireworkBox.cs
            GetComponent<Collider>().enabled = true;
=======
>>>>>>> ChaJinMin:Assets/Scripts/Item/ItemBoxFirework.cs
            FMODUnity.RuntimeManager.PlayOneShot(spawnSound);
        }
        else
        {
            alive = false;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot(getSound);
        }
    }
}
