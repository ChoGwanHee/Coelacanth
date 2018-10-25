using System.Collections;
using UnityEngine;

public class InstallationGel : BaseInstallation
{
    /// <summary>
    /// 밟았을 때 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string stepSound;

    private float startDelay;

    private bool enable = false;

    private void Start()
    {
        lifetime = (float)photonView.instantiationData[0];
        startDelay = (float)photonView.instantiationData[1];

        StartCoroutine(ActiveDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enable || !photonView.isMine) return;

        if (other.CompareTag("Player"))
        {
            BuffController bc = other.GetComponent<BuffController>();
            bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.Slow);
            photonView.RPC("PlayStepSound", PhotonTargets.All, null);
            UIManager._instance.buffInfoUI.DisplayBuffInfo(2, bc.transform);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private IEnumerator ActiveDelay()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        enable = true;

        yield return new WaitForSeconds(lifetime);

        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void PlayStepSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(stepSound);
    }

}
