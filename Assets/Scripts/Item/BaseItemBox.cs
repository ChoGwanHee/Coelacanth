using UnityEngine;

/// <summary>
/// 기본 아이템 박스 클래스
/// </summary>
public abstract class BaseItemBox : Photon.PunBehaviour
{
    /// <summary>
    /// 아이템 매니저에 등록되어 있는 아이템 테이블의 인덱스.
    /// </summary>
    public int tableIndex;

    /// <summary>
    /// 활성화 여부
    /// </summary>
    public bool alive;

    protected void Start()
    {
        transform.SetParent(GameObject.Find("ItemBoxes").transform);
        GameManagerPhoton._instance.itemManager.AddItemBox(this);
        gameObject.SetActive(false);
    }
}
