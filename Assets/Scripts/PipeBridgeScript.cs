using System.Collections;
using UnityEngine;

/// <summary>
/// 파이프 다리를 관리하는 클래스
/// </summary>
public class PipeBridgeScript : Photon.PunBehaviour
{
    /// <summary>
    /// 다리가 부서져 있으면 True 아니면 false 입니다.
    /// </summary>
    public bool isBroken = false;

    /// <summary>
    /// 다리가 부서진 후 재생성 되기까지 걸리는 시간입니다.
    /// </summary>
    public float respawnTime = 2.0f;

    /// <summary>
    /// 다리가 부서지는 확률입니다. (0 ~ 1)
    /// </summary>
    [Range(0, 1)]
    public float breakChance = 0.2f;

    [FMODUnity.EventRef]
    public string preBreakSound;

    [FMODUnity.EventRef]
    public string breakSound;

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isBroken) return;
        if (!photonView.isMine) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            float random = Random.value;
            if (random <= breakChance)
            {
                photonView.RPC("Break", PhotonTargets.All, true);
                StartCoroutine(Respawn());
            }
        }
    }

    /// <summary>
    /// 다리의 부서짐 여부를 지정합니다.
    /// </summary>
    /// <param name="value">부서짐 여부</param>
    [PunRPC]
    public void Break(bool value)
    {
        isBroken = value;
        animator.SetBool("IsBroken", value);
        if(isBroken)
            PlaySound(0);
    }

    /// <summary>
    /// 일정 시간이 지난 후 다리를 재생성합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Respawn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < respawnTime + 1.2f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        photonView.RPC("Break", PhotonTargets.All, false);
    }

    public void PlaySound(int soundNum)
    {
        switch (soundNum)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot(preBreakSound);
                break;
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot(breakSound);
                break;
        }

    }
}
