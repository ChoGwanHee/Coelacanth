using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseInstallation : MonoBehaviour {

    [HideInInspector]
    public float lifetime;
    protected float elapsedTime = 0;

    protected virtual void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= lifetime)
        {
            if(gameObject.GetPhotonView().isMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}
