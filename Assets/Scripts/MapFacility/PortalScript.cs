using UnityEngine;

/// <summary>
/// 포탈에 사용되는 스크립트
/// </summary>
public class PortalScript : MonoBehaviour {

    /// <summary>
    /// 연결된 포탈
    /// </summary>
    public PortalScript connectedPortal;

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
            PhotonView touchedPlayer = other.gameObject.GetPhotonView();
            touchedPlayer.RPC("Teleport", touchedPlayer.owner, connectedPortal.transform.position + connectedPortal.transform.forward * 0.9f);
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
