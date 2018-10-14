using UnityEngine;

/// <summary>
/// 맵 안에서 물리 영향을 받아 자유롭게 움직이는 오브젝트 클래스
/// </summary>
public class DynamicObject : Photon.PunBehaviour
{
    /// <summary>
    /// 위치 동기화 여부
    /// </summary>
    public bool syncObject = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!syncObject) return;

        if (PhotonNetwork.isMasterClient && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerDummy>() != null) return;

            if (collision.gameObject.GetPhotonView().owner != PhotonNetwork.masterClient)
            {
                photonView.TransferOwnership(collision.gameObject.GetPhotonView().owner);
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!syncObject) return;

        if (PhotonNetwork.isMasterClient && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerDummy>() != null) return;

            if (collision.gameObject.GetPhotonView().owner != PhotonNetwork.masterClient)
            {
                photonView.TransferOwnership(PhotonNetwork.masterClient);
            }
        }
    }

    /// <summary>
    /// 오브젝트에 특정 방향으로 힘을 줍니다.
    /// </summary>
    /// <param name="force">힘</param>
    [PunRPC]
    public void Pushed(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }
}
