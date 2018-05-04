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
    public GameObject projectile_ref;           // 투사체 레퍼런스

    public override void Execute(FireworkExecuter executer)
    {
        throw new NotImplementedException();
    }

    private IEnumerator ChainFire()
    {
        yield return null;
    }
}
