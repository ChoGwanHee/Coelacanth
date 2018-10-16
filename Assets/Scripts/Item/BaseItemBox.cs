using UnityEngine;

/// <summary>
/// 기본 아이템 박스 클래스
/// </summary>
public abstract class BaseItemBox : MonoBehaviour
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
    /// 아이템 박스의 아이템풀 인덱스 1
    /// </summary>
    public int poolIndex1;

    /// <summary>
    /// 아이템 박스의 아이템풀 인덱스 2
    /// </summary>
    public int poolIndex2;

    /// <summary>
    /// 활성화 여부
    /// </summary>
    public bool alive;


    protected virtual void Start()
    {
        gameObject.SetActive(false);
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);
        alive = true;
    }

    public virtual void Deactivate()
    {
        StopAllCoroutines();
        alive = false;
        transform.position = new Vector3(0f, 0f, -20f);
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
 
}
