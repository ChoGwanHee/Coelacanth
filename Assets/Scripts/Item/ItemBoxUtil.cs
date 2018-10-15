using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxUtil : BaseItemBox, IInteractable
{
    public Transform target;
    public Vector3 offset;
    public int aniNum;
    public int owner = -1;
    protected Vector3 currentVelocity;
    protected Collider col;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider>();
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
        return 1;
    }

    public virtual void Use(PlayerController pc)
    {
        (item as UtilItem).Execute(pc.BC);
        
        pc.photonView.RPC("PutUtilItem", pc.photonView.owner, false);
        GameManagerPhoton._instance.photonView.RPC("DeactivateItemBox", PhotonTargets.All, poolIndex1, poolIndex2);
    }

    public void SetTargetPlayer(int ownerId)
    {
        PlayerController pc = GameManagerPhoton._instance.GetPlayerByOwnerId(ownerId).PC;

        col.enabled = false;
        target = pc.weaponPoint;
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    public void ResetTarget()
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
