using UnityEngine;

/// <summary>
/// 기본 설치물 클래스
/// </summary>
public abstract class BaseInstallation : Photon.PunBehaviour
{
    /// <summary>
    /// 최대 수명
    /// </summary>
    [HideInInspector]
    public float lifetime;

    /// <summary>
    /// 경과 시간
    /// </summary>
    protected float elapsedTime = 0;


    protected virtual void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= lifetime)
        {
            if (photonView.isMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}
