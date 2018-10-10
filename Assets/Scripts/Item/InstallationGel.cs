using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationGel : BaseInstallation
{
    private float duration;
    private float startDelay;
    private float subSpeed;

    private bool enable = false;

    private void Start()
    {
        duration = (float)photonView.instantiationData[0];
        lifetime = (float)photonView.instantiationData[1];
        startDelay = (float)photonView.instantiationData[2];
        subSpeed = (float)photonView.instantiationData[3];

        StartCoroutine(ActiveDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enable) return;

        if (other.CompareTag("Player"))
        {
            BuffController bc = other.GetComponent<BuffController>();
            bc.SetBuff(BuffType.Slow, SlowBuff(bc));
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

    private IEnumerator SlowBuff(BuffController bc)
    {
        float elapsedTime = 0f;
        PlayerController pc = bc.GetComponent<PlayerController>();

        pc.maxSpeedFactor -= subSpeed;
        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.Slow);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pc.maxSpeedFactor += subSpeed;
        bc.photonView.RPC("RemoveBuff", PhotonTargets.All, (int)BuffType.Slow);
    }

}
