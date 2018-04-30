using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Firework/Fountain")]
public class FireworkFountain : Firework {

    public float hitRadius;
    public GameObject fountain_ref;


    public override void Execute(FireworkExecuter executer)
    {
        RaycastHit hit;
        Vector3 installPos;

        if (Physics.Raycast(executer.firePoint.position, Vector3.down, out hit, 10.0f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
        {
            installPos = hit.point;
        }
        else
        {
            Debug.Log("설치할 공간이 없음");
            return;
        }

        InstallationFountain fountain = PhotonNetwork.Instantiate("Prefabs/" + fountain_ref.name, installPos, Quaternion.identity, 0).GetComponent<InstallationFountain>();

        fountain.lifetime = lifetime;
        fountain.damage = damage;
        fountain.hitForce = hitForce;
        fountain.hitRadius = hitRadius;
    }
}
