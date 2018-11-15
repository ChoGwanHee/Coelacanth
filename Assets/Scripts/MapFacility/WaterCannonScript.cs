using System.Collections;
using UnityEngine;

public class WaterCannonScript : MonoBehaviour {

    public float thrustForce = 5.0f;
    public float blockForce = 8.0f;
    public float duration = 3.0f;
    public float shakePower = 2.0f;
    public float shakeDuration = 0.3f;

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
    /// 물대포를 초기화 합니다.
    /// </summary>
    public void Initialize()
    {
        StopAllCoroutines();
        animator.SetBool("Blasting", false);
        hitRangeEffect.SetActive(false);
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
        Vector3 force;
        Collider[] effectedObjects;
        float elapsedTime = 0;

        PlaySound(1);
        ps.Play();
        hitRangeEffect.SetActive(false);
        GameManagerPhoton._instance.cameraController.Shake(shakePower, shakeDuration);

        force = transform.TransformDirection(Vector3.forward) * thrustForce;

        while (distance < maxDistance)
        {
            distance += 0.9f;
            checkPos.z = distance;

            //Debug.DrawRay(transform.TransformPoint(checkPos), Vector3.up * 3.0f, Color.green, 5.0f);

            effectedObjects = Physics.OverlapBox(transform.TransformPoint(checkPos), new Vector3(1.8f, 1.3f, 1.2f), Quaternion.identity, LayerMask.GetMask("DynamicObject"));

            for (int i = 0; i < effectedObjects.Length; i++)
            {
                // 플레이어 일 때
                if (effectedObjects[i].CompareTag("Player"))
                {
                    PlayerController pc = effectedObjects[i].GetComponent<PlayerController>();
                    pc.Pushed(force * 2);
                }
                else
                {
                    effectedObjects[i].GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        animator.SetBool("Blasting", false);


        Vector3 centerPos = transform.TransformPoint(new Vector3(0f, 0f, 10f));
        Vector3 extents = new Vector3(10.0f, 0.65f, 0.65f);
        Vector3 upDirection = Vector3.forward * blockForce;
        Vector3 downDirection = Vector3.back * blockForce;
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
                        force = upDirection;
                    }
                    else
                    {
                        force = downDirection;
                    }
                    
                    // 플레이어 일 때
                    if (effectedObjects[i].CompareTag("Player"))
                    {
                        PlayerController pc = effectedObjects[i].GetComponent<PlayerController>();
                        pc.Pushed(force);
                    }
                    else
                    {
                        effectedObjects[i].GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
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
