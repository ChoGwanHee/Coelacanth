using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemManager : Photon.PunBehaviour
{
    public bool active = false;

    public ItemTable[] itemTables;
    public BaseItemBox[][] itemBoxPool;
    private int[] lastIndex;
    [HideInInspector]
    [SerializeField]
    public int[] curBoxCount;
    public int[] maxBoxCount;

    /// <summary>
    /// 아이템 박스 종류당 여분 비율 (maxBoxCount * poolExtraRate = 여분 개수)
    /// </summary>
    [Range(0, 1)]
    public float poolExtraRate = 1.0f;


    [Header("Firework")]
    public float regenTime = 5.0f;
    private bool regenTimerEnable = true;
    private float elapsedTime = 0f;
    public int startBoxCount = 6;
    public Transform[] itemRegenPos;


    [Header("Util")]
    public float utilRegenTime = 20.0f;
    private bool utilRegenTimerEnable = true;
    private float utilElapsedTime = 0f;
    public int utilStartBoxCount = 1;
    public Transform[] utilItemRegenPos;

    public UtilItem[] buffUtilItemReference;


    private void Start()
    {
        curBoxCount = new int[itemTables.Length];

        Initialize();
    }

    private void Update()
    {
        if (!PhotonNetwork.player.IsMasterClient || !active) return;

        if (regenTimerEnable)
        {
            elapsedTime += Time.deltaTime;

            if (curBoxCount[0] >= maxBoxCount[0])
            {
                regenTimerEnable = false;
            }

            if (elapsedTime >= regenTime)
            {
                elapsedTime = 0f;
                RegenItemBox(0);
            }
        }
        else
        {
            if(curBoxCount[0] < maxBoxCount[0])
            {
                regenTimerEnable = true;
            }
        }

        if (utilRegenTimerEnable)
        {
            utilElapsedTime += Time.deltaTime;

            if (curBoxCount[1] >= maxBoxCount[1])
            {
                utilRegenTimerEnable = false;
            }

            if (utilElapsedTime >= utilRegenTime)
            {
                utilElapsedTime = 0f;
                RegenItemBox(1);
            }
        }
        else
        {
            if (curBoxCount[1] < maxBoxCount[1])
            {
                utilRegenTimerEnable = true;
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(curBoxCount[0]);
        }
        else
        {
            this.curBoxCount[0] = (int)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 아이템 매니저를 초기화 합니다.
    /// </summary>
    public void Initialize()
    {
        InitItemBoxPool();
    }

    /// <summary>
    /// 아이템 박스 풀 초기화
    /// </summary>
    private void InitItemBoxPool()
    {
        if (itemBoxPool != null)
        {
            for (int i = 0; i < itemBoxPool.Length; i++)
            {
                if(itemBoxPool[i] != null)
                {
                    for (int j = 0; j < itemBoxPool[i].Length; j++)
                    {
                        if (itemBoxPool[i][j] != null)
                        {
                            Destroy(itemBoxPool[i][j].gameObject);
                            itemBoxPool[i][j] = null;
                        }
                    }
                    itemBoxPool[i] = null;
                }
            }
            itemBoxPool = null;
        }

        int totalBoxRefCount = 0;
        for (int i = 0; i < itemTables.Length; i++)
        {
            totalBoxRefCount += itemTables[i].itemList.Length;
        }

        int itemTableIndex = 0;
        int poolExtraAmount = CalcExtraAmount(0);

        itemBoxPool = new BaseItemBox[totalBoxRefCount][];
        for(int i=0; i<itemBoxPool.Length; i++)
        {
            int startIndex = 0;
            for (int j=0; j<itemTableIndex; j++)
            {
                startIndex += itemTables[j].itemList.Length;
            }
            int itemBoxRefIndex = i - startIndex;


            if (itemBoxRefIndex >= itemTables[itemTableIndex].itemList.Length)
            {
                itemTableIndex++;
                itemBoxRefIndex = 0;

                poolExtraAmount = CalcExtraAmount(itemTableIndex);
            }

            if (itemTables[itemTableIndex].itemList[itemBoxRefIndex].itemBoxRef != null)
            {
                itemBoxPool[i] = new BaseItemBox[poolExtraAmount];
            }
        }

        if (lastIndex == null)
            lastIndex = new int[itemBoxPool.Length];
        
        for(int i = 0; i < itemBoxPool.Length; i++)
        {
            lastIndex[i] = 0;
        }

        GameObject parent = GameObject.Find("ItemBoxes");
        if (parent == null)
        {
            parent = new GameObject("ItemBoxes");
        }

        int poolIndex = 0;
        for(int i=0; i<itemTables.Length; i++)
        {
            poolExtraAmount = CalcExtraAmount(i);

            for (int j=0; j<itemTables[i].itemList.Length; j++)
            {
                if (itemTables[i].itemList[j].itemBoxRef != null)
                {
                    for (int k = 0; k < poolExtraAmount; k++)
                    {
                        BaseItemBox itemBox = Instantiate(itemTables[i].itemList[j].itemBoxRef, Vector3.zero, Quaternion.identity, parent.transform).GetComponent<BaseItemBox>();

                        if(itemBox != null)
                        {
                            itemBox.tableIndex = i;
                            itemBox.itemIndex = j;
                            itemBox.poolIndex1 = poolIndex;
                            itemBox.poolIndex2 = k;

                            itemBoxPool[poolIndex][k] = itemBox;
                        }
                    }
                }
                poolIndex++;
            }
        }
    }

    [PunRPC]
    public void ActivateItemBox(int firstIndex, int secondIndex, Vector3 movePos)
    {
        BaseItemBox itemBox = itemBoxPool[firstIndex][secondIndex];

        itemBox.transform.position = movePos;
        itemBox.gameObject.SetActive(true);
        itemBox.alive = true;
        FMODUnity.RuntimeManager.PlayOneShot(itemBox.spawnSound);
    }

    [PunRPC]
    public void DeactivateItemBox(int firstIndex, int secondIndex)
    {
        BaseItemBox itemBox = itemBoxPool[firstIndex][secondIndex];

        itemBox.alive = false;
        itemBox.transform.position = new Vector3(0f, 0f, -20f);
        itemBox.gameObject.SetActive(false);
        curBoxCount[itemBox.tableIndex]--;
    }

    [PunRPC]
    public void RequestLift(int firstIndex, int secondIndex, int requestId)
    {
        ItemBoxUtil itemBox = itemBoxPool[firstIndex][secondIndex] as ItemBoxUtil;

        if (itemBox.owner == -1)
        {
            itemBox.owner = requestId;

            PhotonView pv = GameManagerPhoton._instance.GetPlayerByOwnerId(requestId).photonView;
            pv.RPC("LiftUtilItem", pv.owner, itemBox.poolIndex1, itemBox.poolIndex2);
            photonView.RPC("SetItemBoxFollower", PhotonTargets.All, firstIndex, secondIndex, pv.ownerId);
        }
    }

    [PunRPC]
    public void SetItemBoxFollower(int firstIndex, int secondIndex, int ownerId)
    {
        (itemBoxPool[firstIndex][secondIndex] as ItemBoxUtil).SetTargetPlayer(ownerId);
    }

    [PunRPC]
    public void RemoveItemBoxFollower(int firstIndex, int secondIndex)
    {
        (itemBoxPool[firstIndex][secondIndex] as ItemBoxUtil).ResetTarget();
    }

    /// <summary>
    /// 특정 아이템 테이블의 아이템 리스트 중 특정 아이템을 얻어옵니다.
    /// </summary>
    /// <param name="tableIndex"></param>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    public BaseItem GetItem(int tableIndex, int itemIndex)
    {
        return itemTables[tableIndex].itemList[itemIndex].item;
    }

    /// <summary>
    /// 특정 아이템 테이블의 아이템 리스트 중에서 랜덤으로 아이템의 인덱스를 얻어옵니다.
    /// </summary>
    /// <param name="tableIndex">얻고 싶은 아이템 테이블의 인덱스 입니다.</param>
    /// <returns></returns>
    public int GetRandomItemIndex(int tableIndex)
    {
        return itemTables[tableIndex].RandomChoose();
    }

    /// <summary>
    /// 아이템 박스의 여분을 계산합니다.
    /// </summary>
    /// <param name="tableIndex">테이블 인덱스</param>
    /// <returns></returns>
    private int CalcExtraAmount(int tableIndex)
    {
        if (itemTables[tableIndex].itemList.Length > 1)
        {
            return Mathf.CeilToInt(maxBoxCount[tableIndex] * poolExtraRate);
        }
        else
        {
            return maxBoxCount[tableIndex];
        }
    }

    /// <summary>
    /// 겜 시작시 아이템 박스 생성
    /// </summary>
    public void GameStartRegen()
    {
        for (int i = 0; i < startBoxCount; i++)
        {
            RegenItemBox(0);
        }

        for(int i=0;i<utilStartBoxCount; i++)
        {
            RegenItemBox(1);
        }
    }

    /// <summary>
    /// 맵에 아이템박스를 다시 생성합니다.
    /// </summary>
    public void RegenItemBox(int tableIndex)
    {
        int startIndex = 0;
        int randomItemBox = 0;
        int selectIndex = -1;

        for (int i = 0; i < tableIndex; i++)
        {
            startIndex += itemTables[i].itemList.Length;
        }
        randomItemBox = startIndex + GetRandomItemIndex(tableIndex);

        for (int i = 0; i < itemBoxPool[randomItemBox].Length; i++)
        {
            if (!itemBoxPool[randomItemBox][i].alive)
            {
                selectIndex = i;
                break;
            }
        }

        if (selectIndex == -1)
        {
            Debug.LogWarning("비활성화된 아이템 박스가 없음");
            return;
        }

        Vector3 finalPos = GetRegenPos(tableIndex) + Vector3.up * itemBoxPool[randomItemBox][selectIndex].regenHeight;
        
        photonView.RPC("ActivateItemBox", PhotonTargets.All, randomItemBox, selectIndex, finalPos);
        curBoxCount[tableIndex]++;
    }

    /// <summary>
    /// 아이템 매니저의 모든 기능을 중단하고 아이템 박스들을 비활성화 합니다.
    /// </summary>
    public void StopAllFeature()
    {
        StopAllCoroutines();
        active = false;

        // 아이템 박스들 비활성화
        for (int i = 0; i < itemBoxPool.Length; i++)
        {
            if (itemBoxPool[i] == null) continue;

            for (int j = 0; j < itemBoxPool[i].Length; j++)
            {

                itemBoxPool[i][j].gameObject.SetActive(false);
            }
        }
    }

    private Vector3 GetRegenPos(int type)
    {
        if(type == 0)
        {
            Vector3 regenPos;
            RaycastHit hit1, hit2;
            bool retry1, retry2;
            int count = 0;

            do
            {
                do
                {
                    int regenIndex = Random.Range(0, itemRegenPos.Length / 2) * 2;

                    regenPos = GetRandomPosOnSquare(itemRegenPos[regenIndex].position, itemRegenPos[regenIndex + 1].position);
                    regenPos.y += 6.0f;

                    retry1 = Physics.Raycast(regenPos, Vector3.down, out hit1, 6.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

                    if (++count > 10000)
                    {
                        Debug.LogError("비정상적인 연산입니다. 아이템 생성을 비활성화 합니다.");
                        active = false;
                        return Vector3.zero;
                    }
                }
                while (!retry1);

                retry2 = Physics.SphereCast(regenPos, 1.0f, Vector3.down, out hit2, 6.1f, LayerMask.GetMask("DynamicObject", "StaticObject", "Item"));

            } while (retry2);

            return hit1.point;
        }
        else if(type == 1)
        {
            Vector3 regenPos = Vector3.zero;
            int index;
            bool retry = false;
            List<int> regenable = new List<int>();
            for (int i = 0; i < utilItemRegenPos.Length; i++)
            {
                regenable.Add(i);
            }

            for (int i = 0; i < utilItemRegenPos.Length; i++)
            {
                index = Random.Range(0, regenable.Count);
                regenPos = utilItemRegenPos[regenable[index]].position;

                Collider[] cols = Physics.OverlapSphere(regenPos, 3.0f, LayerMask.GetMask("Item"));
                for(int j=0; j<cols.Length; j++)
                {
                    if (cols[j].GetComponent<ItemBoxUtil>() != null)
                    {
                        retry = true;
                        break;
                    }
                }
                if (!retry)
                    break;
                regenable.Remove(index);
            }

            return regenPos;
        }
        else
        {
            Debug.LogError("존재하지 않는 랜덤 위치 타입 입니다.");
            return Vector3.zero;
        }
    }

    /// <summary>
    /// 두 위치를 기준으로 사각형 범위 안의 위치를 랜덤으로 얻습니다.
    /// </summary>
    /// <param name="pos1">첫 번째 기준</param>
    /// <param name="pos2">두 번째 기준</param>
    /// <returns>랜덤 위치</returns>
    private Vector3 GetRandomPosOnSquare(Vector3 pos1, Vector3 pos2)
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
