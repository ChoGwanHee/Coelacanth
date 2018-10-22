using System.Collections;
using UnityEngine;

public class ItemBoxUtil : BaseItemBox, IInteractable
{
    public int aniNum;
    public int owner = -1;
    protected Vector3 currentVelocity;
    protected Rigidbody rb;
    protected Collider col;
    protected Material mat;
    protected int groundMask;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mat = GetComponentInChildren<Renderer>().material;
        groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if(transform.position.y < -10f)
        {
            GameManagerPhoton._instance.photonView.RPC("DeactivateItemBox", PhotonTargets.All, poolIndex1, poolIndex2);
        }
    }

    public void Interact(PlayerController pc)
    {
        GameManagerPhoton._instance.photonView.RPC("RequestLift", PhotonTargets.MasterClient, poolIndex1, poolIndex2, pc.photonView.ownerId);
    }

    public bool IsInteractable()
    {
        return true;
    }

    public int GetButtonType()
    {
        return 0;
    }

    public virtual void Use(PlayerController pc)
    {
        (item as UtilItem).Execute(pc.BC);
        
        pc.photonView.RPC("PutUtilItem", pc.photonView.owner, false);
        GameManagerPhoton._instance.photonView.RPC("DeactivateItemBox", PhotonTargets.All, poolIndex1, poolIndex2);
    }

    public override void Activate()
    {
        base.Activate();
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
        StopAllCoroutines();
    }

    public void SetOutline(bool active)
    {
        if (active)
            mat.SetFloat("_Outline", 0.05f);
        else
            mat.SetFloat("_Outline", 0f);
    }

    public void SetTargetPlayer(int ownerId)
    {
        PlayerController pc = GameManagerPhoton._instance.GetPlayerByOwnerId(ownerId).PC;

        col.enabled = false;
        transform.SetParent(pc.itemPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        StopAllCoroutines();
    }

    public void ResetTarget(Vector3 pos)
    {
        owner = -1;
        transform.SetParent(GameManagerPhoton._instance.itemManager.itemBoxPoolParent.transform);
        transform.position = pos;
        col.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;

        StartCoroutine(ResetRotation());
    }

    private IEnumerator ResetRotation()
    {
        Vector3 curAngle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        float xVel = 0;
        float zVel = 0;
        float smoothTime = 0.1f;


        while ((Mathf.Abs(curAngle.x) > 1f && Mathf.Abs(curAngle.x) < 359f)
            || (Mathf.Abs(curAngle.z) > 1f && Mathf.Abs(curAngle.z) < 359f))
        {
            curAngle.x = Mathf.SmoothDampAngle(curAngle.x, 0f, ref xVel, smoothTime);
            curAngle.z = Mathf.SmoothDampAngle(curAngle.z, 0f, ref zVel, smoothTime);
            transform.eulerAngles = curAngle;

            yield return null;
        }
        curAngle.x = 0f;
        curAngle.z = 0f;
        transform.eulerAngles = curAngle;
    }

    protected int GetIndex()
    {
        BaseItemBox[][] itemBoxPool = GameManagerPhoton._instance.itemManager.itemBoxPool;

        for (int i=0; i< itemBoxPool[poolIndex1].Length; i++)
        {
            if(itemBoxPool[poolIndex1][i] == this)
            {
                return i;
            }
        }

        Debug.LogError("풀에 오브젝트가 없음");
        return -1;
    }

    
}
