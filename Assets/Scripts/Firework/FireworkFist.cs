using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Firework/Fist")]
public class FireworkFist : Firework
{
    public float hitRadius;


    public override void Execute(FireworkExecuter executer)
    {
        Collider self = executer.GetComponent<Collider>();
        self.enabled = false;

        Collider[] effectedObjects = Physics.OverlapSphere(executer.firePoint.position, hitRadius, LayerMask.GetMask("DynamicObject"), QueryTriggerInteraction.Ignore);
        Vector3 direction = executer.transform.forward;

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage);
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(executer.firePoint.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
        }
        self.enabled = true;
    }
}
