using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxFirework : BaseItemBox
{
    private bool isMaster;

    protected override void Start()
    {
        base.Start();
        isMaster = PhotonNetwork.isMasterClient;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMaster) return;

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

            GameManagerPhoton._instance.photonView.RPC("DeactivateItemBox", PhotonTargets.All, poolIndex1, poolIndex2);
        }
    }

}
