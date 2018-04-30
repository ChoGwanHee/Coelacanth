using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

    public Transform movePos;


    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.isMasterClient) return;

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetPhotonView().RPC("Teleport", other.gameObject.GetPhotonView().owner, movePos.position);
        }
    }
}
