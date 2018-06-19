using UnityEngine;

/// <summary>
/// 포탈에 사용되는 스크립트
/// </summary>
public class PortalScript : MonoBehaviour {

    /// <summary>
    /// 이동할 위치
    /// </summary>
    public Transform movePos;

    /// <summary>
    /// 순간이동 시 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string teleportSound;


    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.isMasterClient) return;

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetPhotonView().RPC("Teleport", other.gameObject.GetPhotonView().owner, movePos.position);
            FMODUnity.RuntimeManager.PlayOneShot(teleportSound);
        }
        else if(other.CompareTag("Object"))
        {
            if(other.GetComponent<DynamicObject>() != null)
            {
                other.gameObject.GetPhotonView().RPC("RespawnObject", PhotonTargets.All, null);
            }
        }
    }
}
