using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Firework/Rocket")]
public class FireworkRocket : Firework {

    public float hitRadius;
    public float projectileSpeed;
    public GameObject projectile_ref;


    public override void Execute(FireworkExecuter executer)
    {
        ProjectileRocket bullet = PhotonNetwork.Instantiate("Prefabs/" + projectile_ref.name, executer.firePoint.position, executer.transform.rotation, 0).GetComponent<ProjectileRocket>();

        bullet.damage = damage;
        bullet.hitForce = hitForce;
        bullet.hitRadius = hitRadius;
        bullet.lifetime = lifetime;
        bullet.SetSpeed(projectileSpeed);

        executer.photonView.RPC("PlayStartSound", PhotonTargets.All, null);
    }
}
