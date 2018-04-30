using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Firework/Roman")]
public class FireworkRoman : Firework
{
    public float hitRadius;
    public float projectileSpeed;
    public GameObject projectile_ref;


    public override void Execute(FireworkExecuter executer)
    {
        ProjectileRoman bullet = PhotonNetwork.Instantiate("Prefabs/" + projectile_ref.name, executer.firePoint.position, executer.transform.rotation, 0).GetComponent<ProjectileRoman>();

        bullet.damage = damage;
        bullet.hitForce = hitForce;
        bullet.hitRadius = hitRadius;
        bullet.lifetime = lifetime;
        bullet.SetSpeed(projectileSpeed);

        executer.photonView.RPC("PlayStartSound", PhotonTargets.All, null);
    }
}
