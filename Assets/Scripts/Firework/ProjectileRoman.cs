using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRoman : BaseProjectile {

    protected override void Update()
    {
        if (!photonView.isMine) return;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Explosion();
            //photonView.RPC("Explosion", PhotonTargets.All, null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;

        if (other.CompareTag("Item")) return;

        Explosion();
        //photonView.RPC("Explosion", PhotonTargets.All, null);
    }

    private void Explosion()
    {
        Collider[] effectedObjects = Physics.OverlapSphere(transform.position, hitRadius, dynamicObjMask);

        for(int i=0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale( effectedObjects[i].transform.position - transform.position, new Vector3(1,0,1)).normalized;

            //effectedObjects[i].GetComponent<Rigidbody>().AddExplosionForce(hitForce, transform.position, hitRadius);
            //effectedObjects[i].GetComponent<Rigidbody>().AddForce(direction * hitForce, ForceMode.Impulse);
            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if(effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage);
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
        }

        FMODUnity.RuntimeManager.PlayOneShot(endSound);

        PhotonNetwork.Instantiate("Prefabs/Romang_Hit_fx",transform.position, transform.rotation, 0);
        PhotonNetwork.Destroy(gameObject);
    }
    
}
