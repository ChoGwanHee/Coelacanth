using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCannonScript : Photon.PunBehaviour {

    public float thrustForce = 5.0f;
    public float blockForce = 8.0f;
    public float duration = 3.0f;

    public GameObject hitEffect_ref;
    public GameObject hitRangeEffect;

    public bool soundActive = true;

    [FMODUnity.EventRef]
    public string metalQuakeSound;

    [FMODUnity.EventRef]
    public string waterFireSound;

    private float maxDistance = 19f;

    private ParticleSystem ps;
    private Animator animator;


    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();

        ps.Stop();
    }


    /// <summary>
    /// 물대포 발사
    /// </summary>
    public IEnumerator Splash()
    {
        animator.SetBool("Blasting", true);
        PlaySound(0);
        hitRangeEffect.SetActive(true);
        yield return new WaitForSeconds(3.4f);

        
        float distance = 0;
        Vector3 checkPos = Vector3.zero;
        Collider[] effectedObjects;
        float elapsedTime = 0;

        PlaySound(1);
        ps.Play();
        hitRangeEffect.SetActive(false);

        while (distance < maxDistance)
        {
            distance += 0.9f;
            checkPos.z = distance;

            Debug.DrawRay(transform.TransformPoint(checkPos), Vector3.up * 3.0f, Color.green, 5.0f);

            effectedObjects = Physics.OverlapBox(transform.TransformPoint(checkPos), new Vector3(1.8f, 1.3f, 1.2f), Quaternion.identity, LayerMask.GetMask("DynamicObject"));

            for (int i = 0; i < effectedObjects.Length; i++)
            {
                //if(effectedObjects[i].gameObject.GetPhotonView().isMine)

                effectedObjects[i].GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * thrustForce, ForceMode.Impulse);

                // 플레이어가 피격 되었을 때 점수 감산
                if (effectedObjects[i].CompareTag("Player"))
                {
                    effectedObjects[i].GetComponent<PlayerStat>().AddScore(-10);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        animator.SetBool("Blasting", false);


        Vector3 centerPos = transform.TransformPoint(new Vector3(0f, 0f, 10f));
        Vector3 extents = new Vector3(10.0f, 0.65f, 0.65f);
        Vector3 direction;
        float tickTime = GameManagerPhoton._instance.GameTick;
        float tickElapsedTime = tickTime;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            tickElapsedTime += Time.deltaTime;


            if (tickElapsedTime >= tickTime)
            {
                effectedObjects = Physics.OverlapBox(centerPos, extents, Quaternion.identity, LayerMask.GetMask("DynamicObject"));

                for (int i = 0; i < effectedObjects.Length; i++)
                {
                    if (effectedObjects[i].transform.position.z > transform.position.z)
                    {
                        direction = Vector3.forward;
                    }
                    else
                    {
                        direction = Vector3.back;
                    }
                    //effectedObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    effectedObjects[i].GetComponent<Rigidbody>().AddForce(direction * blockForce, ForceMode.Impulse);

                    // 플레이어가 피격 되었을 때 점수 감산
                    if (effectedObjects[i].CompareTag("Player"))
                    {
                        effectedObjects[i].GetComponent<PlayerStat>().AddScore(-10);
                    }

                    Vector3 efxPos = effectedObjects[i].GetComponent<Collider>().ClosestPoint(transform.position);
                    Instantiate(hitEffect_ref, efxPos, Quaternion.identity);
                }

                tickElapsedTime = 0;
            }

            yield return null;
        }

    }

    public void PlaySound(int soundNum)
    {
        if (!soundActive) return;

        switch (soundNum)
        {
            case 0:
                FMODUnity.RuntimeManager.PlayOneShot(metalQuakeSound);
                break;
            case 1:
                FMODUnity.RuntimeManager.PlayOneShot(waterFireSound);
                break;
        }

    }
}
