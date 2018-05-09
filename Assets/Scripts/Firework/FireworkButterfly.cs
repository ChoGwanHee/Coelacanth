using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Firework/Butterfly")]
public class FireworkButterfly : Firework
{
    public float hitRadius;                     // 투사체가 터졌을 때 영향을 받는 범위
    public float projectileSpeed;               // 투사체 속도
    public float chainFireTime;                 // 연속발사 간격
    public int chainAmount;                     // 한번에 발사되는 개수
    public float fireAngle;                     // 발사각도
    public GameObject projectile_ref;           // 투사체 레퍼런스

    public override void Execute(FireworkExecuter executer)
    {
        executer.StartCoroutine(ChainFire(executer));
    }

    private IEnumerator ChainFire(FireworkExecuter executer)
    {
        for(int i=0; i<chainAmount; i++)
        {
            ProjectileButterfly bullet = PhotonNetwork.Instantiate("Prefabs/" + projectile_ref.name, executer.firePoint.position, executer.transform.rotation, 0).GetComponent<ProjectileButterfly>();

            bullet.damage = damage;
            bullet.hitForce = hitForce;
            bullet.hitRadius = hitRadius;
            bullet.lifetime = lifetime;
            bullet.SetSpeed(projectileSpeed);

            if (chainFireTime > 0)
                yield return new WaitForSeconds(chainFireTime);
            else
                yield return null;
        }

    }
}
