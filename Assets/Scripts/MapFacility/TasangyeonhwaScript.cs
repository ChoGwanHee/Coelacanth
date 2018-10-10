using System.Collections;
using UnityEngine;


public class TasangyeonhwaScript : Photon.PunBehaviour, IInteractable {

    public enum TasangState
    {
        Hide,
        Appear,
        Fire
    }
    
    public GameObject boxCover;
    public GameObject boxBody;
    public GameObject flyCover;
    private Rigidbody coverRb;
    private Material[] mat;

    public float boomPower = 13.0f;

    public ParticleSystem boomEfx;
    public ParticleSystem boomEfx2;
    public GameObject dragon;
    public Renderer dragonRenderer;


    /// <summary>
    /// 위로 올라가는 타상연화 폭죽
    /// </summary>
    public GameObject tasangUp;
    
    /// <summary>
    /// 위로 올라갈 때 발사되는 위치
    /// </summary>
    public Transform firePos;

    private int layerMask;
    private Animator animator;
    private Collider col;

    [HideInInspector]
    public MapFacilityTasangyeonhwa parentFacility;
    


	void Start () {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        coverRb = flyCover.GetComponent<Rigidbody>();
        mat = new Material[2];
        mat[0] = boxCover.GetComponent<Renderer>().material;
        mat[1] = boxBody.GetComponent<Renderer>().material;
        layerMask = LayerMask.GetMask("DynamicObject", "Item");

        boomEfx.Stop(true);
        boomEfx2.Stop(true);
        SetState(TasangState.Hide);
    }
    

    public void Boom()
    {
        boxCover.SetActive(false);
        flyCover.SetActive(true);
        coverRb.useGravity = true;
        coverRb.velocity = new Vector3(Random.Range(-0.1f, 0.1f), boomPower, Random.Range(-0.1f, 0.1f));
        coverRb.angularVelocity = new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f));
        boomEfx.Play(true);
        dragonRenderer.enabled = true;
        col.enabled = true;
    }

    public void SetState(TasangState stateNum)
    {
        switch (stateNum)
        {
            case TasangState.Hide:
                for (int i = 0; i < mat.Length; i++)
                {
                    mat[i].SetFloat("_slider", 0.5f);
                }
                animator.SetBool("IsAppear", false);
                flyCover.transform.localPosition = Vector3.zero;
                flyCover.transform.localEulerAngles = new Vector3(-90.0f, 0f);
                dragonRenderer.enabled = false;
                dragon.SetActive(false);
                col.enabled = false;
                break;
            case TasangState.Appear:
                StopAllCoroutines();
                StartCoroutine(AppearBox(1.0f));
                break;
            case TasangState.Fire:
                FireEffect();
                break;
        }
    }

    private void HideBoxMesh()
    {
        boxBody.SetActive(false);
    }

    public void FireEffect()
    {
        Instantiate(tasangUp, firePos.position, Quaternion.identity);
        dragon.GetComponent<Animator>().SetTrigger("Fire");
        boomEfx2.Play(true);
    }
    
    public IEnumerator AppearBox(float appearTime)
    {
        float elapsedTime = 0.0f;
        float ratio = 0.0f;
        Vector3 subVec = new Vector3(1f, 0f, 1f);

        boxCover.SetActive(true);
        boxBody.SetActive(true);

        while (elapsedTime < appearTime)
        {
            elapsedTime += Time.deltaTime;
            ratio = 1 - (elapsedTime / appearTime);

            for(int i=0; i < mat.Length; i++)
            {
                mat[i].SetFloat("_slider", 0.5f * ratio);
            }

            // 주변 물체 밀어내기
            Collider[] cols = Physics.OverlapSphere(transform.position, 0.9f, layerMask);
            Vector3 toVec;

            for (int i = 0; i < cols.Length; i++)
            {
                toVec = cols[i].transform.position - transform.position;
                toVec.Scale(subVec);
                toVec = toVec.normalized;

                cols[i].transform.Translate(toVec * 0.1f, Space.World);
            }

            yield return null;
        }
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].SetFloat("_slider", 0.0f);
        }
        dragon.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        animator.SetBool("IsAppear", true);

        yield return new WaitForSeconds(7.0f);
        coverRb.useGravity = false;
        coverRb.velocity = Vector3.zero;
        coverRb.angularVelocity = Vector3.zero;
        flyCover.SetActive(false);
    }

    public void Interact(PlayerController pc)
    {
        parentFacility.photonView.RPC("RequestFire", PhotonTargets.MasterClient, pc.photonView.ownerId);
    }
}
