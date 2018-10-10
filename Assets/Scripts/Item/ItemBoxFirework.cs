using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxFirework : BaseItemBox {

    [FMODUnity.EventRef]
    public string getSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        if (other.CompareTag("Player"))
        {
            int itemIndex = (base.itemIndex == -1) ? GameManagerPhoton._instance.itemManager.GetRandomItemIndex(tableIndex)
                : base.itemIndex;

            FireworkExecuter executer = other.GetComponent<FireworkExecuter>();

            if (executer != null)
            {
                executer.photonView.RPC("ChangeFirework", PhotonTargets.All, tableIndex, itemIndex);
            }
            else
            {
                Debug.LogError("Executer가 null 입니다");
            }

            photonView.RPC("SetActiveItemBox", PhotonTargets.All, false);
        }
    }

    [PunRPC]
    public override void SetActiveItemBox(bool active)
    {
        base.SetActiveItemBox(active);
        if (!active)
        {
            FMODUnity.RuntimeManager.PlayOneShot(getSound);
        }
    }

}
