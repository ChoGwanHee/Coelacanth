using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : Photon.PunBehaviour {

    private Rigidbody rb;

	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(5, 0, 0);
	}

    /*private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.isMasterClient && collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.GetPhotonView().owner != PhotonNetwork.masterClient)
            {
                photonView.TransferOwnership(collision.gameObject.GetPhotonView().owner);
                print(gameObject.name + "의 새 소유자:" + PhotonNetwork.player);
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (PhotonNetwork.isMasterClient && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetPhotonView().owner != PhotonNetwork.masterClient)
            {
                photonView.TransferOwnership(collision.gameObject.GetPhotonView().owner);
                print(gameObject.name + "의 새 소유자:" + PhotonNetwork.masterClient);
            }
        }
    }

    [PunRPC]
    public void Pushed(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }*/

    private void OnMouseDown()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector3(-5, 0, 0), ForceMode.Impulse);
    }
}
