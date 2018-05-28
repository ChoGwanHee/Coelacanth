using UnityEngine;

/// <summary>
/// 포탈에 사용되는 스크립트
/// </summary>
public class PortalScript : MonoBehaviour {

    /// <summary>
    /// 이동할 위치
    /// </summary>
    public Transform movePos;


    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.isMasterClient) return;

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetPhotonView().RPC("Teleport", other.gameObject.GetPhotonView().owner, movePos.position);
        }
    }
}
