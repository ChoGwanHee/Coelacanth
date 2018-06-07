
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemManager : Photon.PunBehaviour {

    public FireworkItemTable[] fireworkItemTables;
    public BaseItem[] itemBoxPool;
    
    public float regenTime = 5.0f;
    private bool regenTimerEnable = true;
    private float elapsedTime = 0f;
    public int maxBoxCount = 4;
    [SerializeField]
    public int curBoxCount = 0;

    public Transform[] itemRegenPos;

    public bool active = false;

    private void Start()
    {
        if(!PhotonNetwork.player.IsMasterClient) return;

        for (int i=0; i <= 4; i++)
        {
            RegenItemBox();
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.player.IsMasterClient || !active) return;

        if (regenTimerEnable)
        {
            elapsedTime += Time.deltaTime;

            if (curBoxCount >= maxBoxCount)
            {
                regenTimerEnable = false;
            }

            if (elapsedTime >= regenTime)
            {
                elapsedTime = 0f;
                RegenItemBox();
            }

        }
        else
        {
            if(curBoxCount < maxBoxCount)
            {
                regenTimerEnable = true;
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(curBoxCount);
        }
        else
        {
            this.curBoxCount = (int)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 특정 아이템 테이블의 아이템 리스트 중 특정 아이템을 얻어옵니다.
    /// </summary>
    /// <param name="tableIndex"></param>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public Firework GetFireworkItem(int tableIndex, int itemIndex)
    {
        return fireworkItemTables[tableIndex].itemList[itemIndex].item;
    }

    /// <summary>
    /// 특정 아이템 테이블의 아이템 리스트 중에서 랜덤으로 아이템의 인덱스를 얻어옵니다.
    /// </summary>
    /// <param name="tableIndex">얻고 싶은 아이템 테이블의 인덱스 입니다.</param>
    /// <returns></returns>
    public int GetRandomItem(int tableIndex)
    {
        return fireworkItemTables[tableIndex].RandomChoose();
    }

    /// <summary>
    /// 맵에 아이템박스를 다시 생성합니다.
    /// </summary>
    public void RegenItemBox()
    {
        Vector3 regenPos;
        RaycastHit hit1, hit2;
        bool retry1, retry2;
        int randomIndex;
        int count = 0;

        do
        {
            do
            {
                //randomIndex = Random.Range(0, 2) * 2;
                randomIndex = Mathf.FloorToInt(Random.value * 2) *2;

                regenPos = GetRandomPos(itemRegenPos[randomIndex].position, itemRegenPos[randomIndex + 1].position);
                regenPos.y += 6.0f;

                retry1 = Physics.BoxCast(regenPos, new Vector3(0.9f, 1.0f, 0.9f), Vector3.down, out hit1, Quaternion.identity, 6.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

                if(++count > 10000)
                {
                    Debug.Log("비정상적인 연산입니다. 아이템 생성을 비활성화 합니다.");
                    active = false;
                    return;
                }
            }
            while (!retry1);

            retry2 = Physics.BoxCast(regenPos, new Vector3(0.9f, 1.0f, 0.9f), Vector3.down, out hit2, Quaternion.identity, 6.1f, LayerMask.GetMask("DynamicObject", "StaticObject", "Item"));

        } while (retry2);


        int selectIndex = -1;

        for(int i=0; i<itemBoxPool.Length; i++)
        {
            if(!itemBoxPool[i].alive)
            {
                selectIndex = i;
                break;
            }
        }

        if (selectIndex == -1)
        {
            Debug.Log("비활성화된 아이템 박스가 없음");
            return;
        }

        itemBoxPool[selectIndex].transform.position = hit1.point;
        itemBoxPool[selectIndex].photonView.RPC("SetActiveItemBox", PhotonTargets.All, true);
        curBoxCount++;
    }

    private Vector3 GetRandomPos(Vector3 pos1, Vector3 pos2)
    {
        Vector3 minPos = Vector3.zero;
        Vector3 maxPos = Vector3.zero;

        if(pos1.x > pos2.x)
        {
            minPos.x = pos2.x;
            maxPos.x = pos1.x;
        }
        else
        {
            minPos.x = pos1.x;
            maxPos.x = pos2.x;
        }

        if (pos1.z > pos2.z)
        {
            maxPos.z = pos1.z;
            minPos.z = pos2.z;
        }
        else
        {
            maxPos.z = pos2.z;
            minPos.z = pos1.z;
        }

        Vector3 result = Vector3.zero;

        result.x = Random.Range(minPos.x, maxPos.x);
        result.z = Random.Range(minPos.z, maxPos.z);
        result.y = pos1.y;

        return result;
    }
}
