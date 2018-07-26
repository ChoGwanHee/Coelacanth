using UnityEngine;

/// <summary>
/// 로켓 폭죽 클래스
/// </summary>
[CreateAssetMenu(menuName = "Firework/Rocket")]
public class FireworkRocket : Firework
{
    /// <summary>
    /// 투사체 속력
    /// </summary>
    public float projectileSpeed;

    /// <summary>
    /// 투사체 레퍼런스
    /// </summary>
    public GameObject projectile_ref;


    public override void Execute(FireworkExecuter executer)
    {
        ProjectileRocket bullet = PhotonNetwork.Instantiate("Prefabs/" + projectile_ref.name, executer.firePoint.position, executer.transform.rotation, 0).GetComponent<ProjectileRocket>();

        bullet.damage = damage;
        bullet.hitForce = hitForce;
        bullet.hitRadius = hitRadius;
        bullet.lifetime = lifetime;
        bullet.SetSpeed(projectileSpeed);
        bullet.gainScore = gainScore;
        bullet.amplitude = amplitude;
        bullet.duration = duration;

        executer.DecreaseAmmo();
    }
}
