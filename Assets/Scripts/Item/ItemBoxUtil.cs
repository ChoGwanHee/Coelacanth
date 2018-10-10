using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxUtil : BaseItemBox, IInteractable
{
    public Transform target;
    public Vector3 offset;
    public int aniNum;
    protected Vector3 currentVelocity;
    protected int owner = -1;
    protected Collider col;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider>();
    }

    public void Interact(PlayerController pc)
    {
        photonView.RPC("RequestLift", PhotonTargets.MasterClient, pc.photonView.ownerId);
    }

    public virtual void Use(PlayerController pc)
    {
        (item as UtilItem).Execute(pc.BC);
        
        pc.photonView.RPC("PutUtilItem", pc.photonView.owner, false);
        photonView.RPC("SetActiveItemBox", PhotonTargets.All, false);
    }

    [PunRPC]
    public void RequestLift(int requestId)
    {
        if (owner == -1)
        {
            owner = requestId;

            PhotonView pv = GameManagerPhoton._instance.GetPlayerByOwnerId(requestId).photonView;
            pv.RPC("LiftUtilItem", pv.owner, poolIndex, GetIndex());
            photonView.RPC("SetTargetPlayer", PhotonTargets.All, pv.ownerId);
        }
    }

    [PunRPC]
    protected void SetTargetPlayer(int ownerId)
    {
        PlayerController pc = GameManagerPhoton._instance.GetPlayerByOwnerId(ownerId).PC;

        col.enabled = false;
        target = pc.weaponPoint;
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    [PunRPC]
    protected void ResetTarget()
    {
        owner = -1;
        col.enabled = true;
        target = null;
    }

    protected IEnumerator Move()
    {
        while(target != null)
        {
            //transform.position = Vector3.SmoothDamp(transform.position, target.position, ref currentVelocity, moveTime);
            transform.position = target.position + target.TransformVector(offset);
            yield return null;
        }
    }

    protected int GetIndex()
    {
        BaseItemBox[][] itemBoxPool = GameManagerPhoton._instance.itemManager.itemBoxPool;

        for (int i=0; i< itemBoxPool[poolIndex].Length; i++)
        {
            if(itemBoxPool[poolIndex][i] == this)
            {
                return i;
            }
        }

        Debug.LogError("풀에 오브젝트가 없음");
        return -1;
    }
}
