using UnityEngine;

/// <summary>
/// 기본 아이템 박스 클래스
/// </summary>
public abstract class BaseItemBox : Photon.PunBehaviour
{
    public BaseItem item;
    
    [FMODUnity.EventRef]
    public string spawnSound;

    /// <summary>
    /// 리젠시 땅으로부터의 높이
    /// </summary>
    public float regenHeight;

    /// <summary>
    /// 아이템 매니저에 등록되어 있는 아이템 테이블의 인덱스.
    /// </summary>
    public int tableIndex;

    /// <summary>
    /// 아이템 매니저에 등록되어 있는 아이템의 인덱스.
    /// </summary>
    public int itemIndex;

    /// <summary>
    /// 아이템 매니저에 등록되어 있는 아이템 박스의 아이템풀 인덱스
    /// </summary>
    public int poolIndex;
    

    /// <summary>
    /// 활성화 여부
    /// </summary>
    public bool alive;

    protected virtual void Start()
    {
        InitIndex();
        transform.SetParent(GameObject.Find("ItemBoxes").transform);
        GameManagerPhoton._instance.itemManager.AddItemBox(this);
        gameObject.SetActive(false);
    }

    protected void InitIndex()
    {
        object[] data = photonView.instantiationData;
        tableIndex = (int)data[0];
        itemIndex = (int)data[1];
        poolIndex = (int)data[2];
    }

    [PunRPC]
    public virtual void SetActiveItemBox(bool active)
    {
        if (active)
        {
            gameObject.SetActive(true);
            alive = true;
            FMODUnity.RuntimeManager.PlayOneShot(spawnSound);
        }
        else
        {
            alive = false;
            transform.position = new Vector3(0f, 0f, -20f);
            GameManagerPhoton._instance.itemManager.curBoxCount[tableIndex]--;
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
