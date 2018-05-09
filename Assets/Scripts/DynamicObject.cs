using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : Photon.PunBehaviour
{

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        /*if (PhotonNetwork.isMasterClient)
        {
            rb.useGravity = true;
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.isMasterClient && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerDummy>() != null) return;

            if (collision.gameObject.GetPhotonView().owner != PhotonNetwork.masterClient)
            {
                photonView.TransferOwnership(collision.gameObject.GetPhotonView().owner);
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (PhotonNetwork.isMasterClient && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerDummy>() != null) return;

            if (collision.gameObject.GetPhotonView().owner != PhotonNetwork.masterClient)
            {
                photonView.TransferOwnership(PhotonNetwork.masterClient);
            }
        }
    }

    [PunRPC]
    public void Pushed(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }
}
