using UnityEngine;

/// <summary>
/// 로망 폭죽 클래스
/// </summary>
[CreateAssetMenu(menuName = "Firework/Roman")]
public class FireworkRoman : Firework
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
        ProjectileRoman bullet = PhotonNetwork.Instantiate("Prefabs/" + projectile_ref.name, executer.firePoint.position, executer.transform.rotation, 0).GetComponent<ProjectileRoman>();

        bullet.damage = damage;
        bullet.hitForce = hitForce;
        bullet.hitRadius = hitRadius;
        bullet.lifetime = lifetime;
        bullet.SetSpeed(projectileSpeed);
        bullet.gainScore = gainScore;

        executer.DecreaseAmmo();

        FMODUnity.RuntimeManager.PlayOneShot(voiceSound);
    }
}
