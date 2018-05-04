using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="Firework/Roman")]
public class FireworkRoman : Firework
{
    public float hitRadius;                     // 투사체가 터졌을 때 영향을 받는 범위
    public float projectileSpeed;               // 투사체 속도
    public GameObject projectile_ref;           // 투사체 레퍼런스


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
