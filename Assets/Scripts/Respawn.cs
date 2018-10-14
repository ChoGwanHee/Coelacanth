using UnityEngine;

/// <summary>
/// 다이나믹 오브젝트를 리스폰할 때 사용하는 스크립트
/// </summary>
public class Respawn : Photon.PunBehaviour
{
    /// <summary>
    /// 리스폰 대기 시간
    /// </summary>
    public float respawnTime = 2.0f;

    /// <summary>
    /// 하위 오브젝트들. 이 오브젝트가 리스폰될 때 같이 리스폰 됩니다.
    /// </summary>
    public TableTopObject[] subObjects;

    /// <summary>
    /// 게임이 처음 시작했을 때의 오브젝트 위치
    /// </summary>
    private Vector3 originPos;

    /// <summary>
    /// 게임이 처음 시작했을 때의 오브젝트 회전
    /// </summary>
    private Quaternion originQuaternion;

    /// <summary>
    /// 경과 시간
    /// </summary>
    private float elapsedTime = 0f;

    /// <summary>
    /// 현재 리스폰 대기 중인지 여부
    /// </summary>
    private bool respawning = false;

    private Rigidbody rb;
    private Renderer objectRenderer;


    void Start()
    {
        originPos = transform.position;
        originPos.y += 3.0f;
        originQuaternion = transform.rotation;
        rb = GetComponent<Rigidbody>();
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!photonView.isMine) return;

        if (respawning)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= respawnTime)
            {
                photonView.RPC("RespawnObject", PhotonTargets.All, null);
                return;
            }
        }
        else
        {
            if (transform.position.y < -10)
            {
                photonView.RPC("HideObject", PhotonTargets.All, null);
                respawning = true;
                elapsedTime = 0f;
            }
        }
    }

    /// <summary>
    /// 오브젝트를 리스폰합니다.
    /// </summary>
    [PunRPC]
    private void RespawnObject()
    {
        transform.position = originPos;
        transform.rotation = originQuaternion;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        respawning = false;
        objectRenderer.enabled = true;

        // 하위 오브젝트 리스폰
        for (int i = 0; i < subObjects.Length; i++)
        {
            subObjects[i].Respawn();
        }
    }

    /// <summary>
    /// 오브젝트를 숨깁니다.
    /// </summary>
    [PunRPC]
    private void HideObject()
    {
        objectRenderer.enabled = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
