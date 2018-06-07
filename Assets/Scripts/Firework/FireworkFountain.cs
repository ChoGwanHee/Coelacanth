using UnityEngine;

/// <summary>
/// 분수 폭죽 클래스
/// </summary>
[CreateAssetMenu(menuName = "Firework/Fountain")]
public class FireworkFountain : Firework
{
    /// <summary>
    /// 분수 레퍼런스
    /// </summary>
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
        fountain.gainScore = gainScore;

        executer.DecreaseAmmo();

        FMODUnity.RuntimeManager.PlayOneShot(voiceSound);
    }
}
