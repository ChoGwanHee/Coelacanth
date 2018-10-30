using UnityEngine;

[CreateAssetMenu(menuName = "Item/Gel")]
public class ItemGel : UtilItem
{
    /// <summary>
    /// 지속시간
    /// </summary>
    public float duration;

    /// <summary>
    /// 최대 수명
    /// </summary>
    public float lifetime;

    /// <summary>
    /// 설치 대기 시간
    /// </summary>
    public float startDelay;

    /// <summary>
    /// 감소될 이동속도 퍼센트
    /// </summary>
    public float subSpeed;

    /// <summary>
    /// 젤 레퍼런스
    /// </summary>
    public GameObject gel_ref;


    public override void Execute(BuffController bc)
    {
        RaycastHit hit;
        Vector3 installPos;

        if (Physics.Raycast(bc.PC.Executer.firePoint.position, Vector3.down, out hit, 10.0f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
        {
            installPos = hit.point;
        }
        else
        {
            Debug.Log("설치할 공간이 없음");
            return;
        }

        object[] data = new object[2] { lifetime, startDelay };

        PhotonNetwork.Instantiate("Prefabs/" + gel_ref.name, installPos, Quaternion.identity, 0, data);

        FMODUnity.RuntimeManager.PlayOneShot(useSound);
    }
}
