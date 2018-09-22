using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class ItemManager : Photon.PunBehaviour
{
    public bool active = false;

    public FireworkItemTable[] fireworkItemTables;
    public List<BaseItemBox> itemBoxPool;
    public BaseItemBox[] itemBoxReferences;

    /// <summary>
    /// 아이템 박스 종류당 여분 비율 (maxBoxCount * poolExtraRate = 여분 개수)
    /// </summary>
    [Range(0, 1)]
    public float poolExtraRate = 1.0f;

    /// <summary>
    /// 아이템 박스 종류당 여분 개수
    /// </summary>
    private int poolExtraAmount;

    public float regenTime = 5.0f;
    private bool regenTimerEnable = true;
    private float elapsedTime = 0f;
    public int maxBoxCount = 10;
    public int startBoxCount = 6;
    [SerializeField]
    public int curBoxCount = 0;

    public Transform[] itemRegenPos;


    private void Start()
    {
        Initialize();
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
    /// 아이템 매니저를 초기화 합니다.
    /// </summary>
    public void Initialize()
    {
        if (GameObject.Find("ItemBoxes") == null)
        {
            new GameObject("ItemBoxes");
        }

        // 이후부터 마스터만 실행
        if (!PhotonNetwork.isMasterClient) return;

        InitItemBoxPool();
    }

    /// <summary>
    /// 아이템 박스 풀 초기화
    /// </summary>
    private void InitItemBoxPool()
    {
        for (int i = 0; i < itemBoxPool.Count; i++)
        {
            PhotonNetwork.Destroy(itemBoxPool[i].gameObject);
        }
        itemBoxPool.Clear();


        if (itemBoxReferences.Length > 1)
        {
            poolExtraAmount = Mathf.CeilToInt(maxBoxCount * poolExtraRate);
        }
        else
        {
            poolExtraAmount = maxBoxCount;
        }

        string folderPath = "Prefabs/";

        for (int i = 0; i < itemBoxReferences.Length; i++)
        {
            string path = folderPath + itemBoxReferences[i].name;
            for (int j = 0; j < poolExtraAmount; j++)
            {
                PhotonNetwork.Instantiate(path, Vector3.zero, Quaternion.identity, 0);
            }
        }
    }

    /// <summary>
    /// 아이템 박스 풀에 아이템 박스를 추가합니다.
    /// </summary>
    /// <param name="itemBox">추가할 아이템 박스</param>
    public void AddItemBox(BaseItemBox itemBox)
    {
        itemBoxPool.Add(itemBox);
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
    /// 겜 시작시 아이템 박스 생성
    /// </summary>
    public void FirstRegen()
    {
        for (int i = 0; i <= startBoxCount; i++)
        {
            RegenItemBox();
        }
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
                randomIndex = Mathf.FloorToInt(Random.value * 2) * 2;

                regenPos = GetRandomPos(itemRegenPos[randomIndex].position, itemRegenPos[randomIndex + 1].position);
                regenPos.y += 6.0f;

                retry1 = Physics.Raycast(regenPos, Vector3.down, out hit1, 6.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

                if (++count > 10000)
                {
                    Debug.LogError("비정상적인 연산입니다. 아이템 생성을 비활성화 합니다.");
                    active = false;
                    return;
                }
            }
            while (!retry1);

            retry2 = Physics.SphereCast(regenPos, 1.0f, Vector3.down, out hit2, 6.1f, LayerMask.GetMask("DynamicObject", "StaticObject", "Item"));

        } while (retry2);


        int selectIndex = -1;

        for (int i = 0; i < itemBoxPool.Count; i++)
        {
            if (!itemBoxPool[i].alive)
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

        itemBoxPool[selectIndex].transform.position = hit1.point + Vector3.up * 0.1f;
        itemBoxPool[selectIndex].photonView.RPC("SetActiveItemBox", PhotonTargets.All, true);
        curBoxCount++;
    }

    /// <summary>
    /// 아이템 매니저의 모든 기능을 중단하고 아이템 박스들을 비활성화 합니다.
    /// </summary>
    public void StopAllFeature()
    {
        StopAllCoroutines();
        active = false;

        // 아이템 박스들 비활성화
        for (int i = 0; i < itemBoxPool.Count; i++)
        {
            itemBoxPool[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 두 위치를 기준으로 사각형 범위 안의 위치를 랜덤으로 얻습니다.
    /// </summary>
    /// <param name="pos1">첫 번째 기준</param>
    /// <param name="pos2">두 번째 기준</param>
    /// <returns>랜덤 위치</returns>
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
