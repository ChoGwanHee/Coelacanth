using System.Collections;
using UnityEngine;

/// <summary>
/// 나비 폭죽 클래스
/// </summary>
[CreateAssetMenu(menuName = "Firework/Butterfly")]
public class FireworkButterfly : Firework
{
    /// <summary>
    /// 투사체 속력
    /// </summary>
    public float projectileSpeed;

    /// <summary>
    /// 초당 회전 각도
    /// </summary>
    public float rotateAngle;

    /// <summary>
    /// 1회 충전 시간
    /// </summary>
    public float chargingTime;

    /// <summary>
    /// 연속발사 지연시간
    /// </summary>
    public float chainFireTime;

    /// <summary>
    /// 최대 충전 개수
    /// </summary>
    public int maxChargingAmount;
    
    /// <summary>
    /// 투사체 레퍼런스
    /// </summary>
    public GameObject projectile_ref;

    /// <summary>
    /// 충전 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string chargingSound;


    public override void Execute(FireworkExecuter executer)
    {
        executer.charging = true;
        executer.StartCoroutine(Charging(executer));
    }

    /// <summary>
    /// 마우스 왼쪽 클릭을 누르는 동안 일정 시간마다 나비 폭죽을 충전합니다.
    /// </summary>
    /// <param name="executer"></param>
    /// <returns></returns>
    private IEnumerator Charging(FireworkExecuter executer)
    {
        int chargingAmount = 1;
        float elapsedTime = 0;

        UIManager._instance.chargingUI.SetActivate(true);
        UIManager._instance.chargingUI.SetAmount(chargingAmount);

        PlayerController pc = executer.GetComponent<PlayerController>();
        pc.SetAnimParam("Charging", true);

        while(Input.GetMouseButton(0))
        {
            if(chargingAmount < maxChargingAmount && chargingAmount < executer.ammo)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= chargingTime)
                {
                    elapsedTime -= chargingTime;
                    chargingAmount++;
                    UIManager._instance.chargingUI.SetAmount(chargingAmount);
                    FMODUnity.RuntimeManager.PlayOneShot(chargingSound);
                    Debug.Log("chargingAmount:" + chargingAmount);
                }
            }

            // UI 업데이트
            UIManager._instance.chargingUI.SetRatio(elapsedTime / chargingTime);

            yield return null;
        }
        executer.charging = false;
        pc.SetAnimParam("Charging", false);

        executer.StartCoroutine(ChainFire(executer, chargingAmount));
    }

    /// <summary>
    /// 충전된 개수만큼 나비 폭죽을 연속으로 발사합니다.
    /// </summary>
    /// <param name="executer"></param>
    /// <param name="chargingAmount">충전 개수</param>
    /// <returns></returns>
    private IEnumerator ChainFire(FireworkExecuter executer, int chargingAmount)
    {
        yield return new WaitForSeconds(0.15f);

        PlayerStat stat = executer.GetComponent<PlayerStat>();

        for (int i=0; i< chargingAmount; i++)
        {
            ProjectileButterfly bullet = PhotonNetwork.Instantiate("Prefabs/" + projectile_ref.name, executer.firePoint.position, executer.transform.rotation, 0).GetComponent<ProjectileButterfly>();

            bullet.damage = Mathf.RoundToInt(damage * executer.damageFactor);
            bullet.hitForce = hitForce * executer.forceFactor;
            bullet.hitRadius = hitRadius;
            bullet.lifetime = lifetime;
            bullet.SetSpeed(projectileSpeed);
            bullet.rotateAngle = rotateAngle;
            bullet.gainScore = gainScore;
            bullet.amplitude = amplitude;
            bullet.duration = duration;

            executer.DecreaseAmmo();

            int myIndex = GameManagerPhoton._instance.playerList.IndexOf(stat);
            bullet.SearchTarget(myIndex);

            if (chainFireTime > 0)
                yield return new WaitForSeconds(chainFireTime);
            else
                yield return null;
        }

    }
}
