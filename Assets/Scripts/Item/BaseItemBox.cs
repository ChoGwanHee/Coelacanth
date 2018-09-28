using UnityEngine;

/// <summary>
/// 기본 아이템 박스 클래스
/// </summary>
public abstract class BaseItemBox : Photon.PunBehaviour
{
<<<<<<< HEAD
=======
    public BaseItem item;

    /// <summary>
    /// 리젠시 땅으로부터의 높이
    /// </summary>
    public float regenHeight;

>>>>>>> ChaJinMin
    /// <summary>
    /// 아이템 매니저에 등록되어 있는 아이템 테이블의 인덱스.
    /// </summary>
    public int tableIndex;

    /// <summary>
<<<<<<< HEAD
=======
    /// 아이템 매니저에 등록되어 있는 아이템의 인덱스.
    /// </summary>
    public int itemIndex;
    

    /// <summary>
>>>>>>> ChaJinMin
    /// 활성화 여부
    /// </summary>
    public bool alive;

    protected void Start()
    {
<<<<<<< HEAD
=======
        FindItemIndex();
>>>>>>> ChaJinMin
        transform.SetParent(GameObject.Find("ItemBoxes").transform);
        GameManagerPhoton._instance.itemManager.AddItemBox(this);
        gameObject.SetActive(false);
    }
<<<<<<< HEAD
=======

    /// <summary>
    /// 테이블 인덱스를 이용해 몇번째 아이템으로 등록되어 있는지 찾아서 저장합니다.
    /// </summary>
    protected void FindItemIndex()
    {
        if(item == null)
        {
            itemIndex = -1;
            return;
        }

        ItemTable curItemTable = GameManagerPhoton._instance.itemManager.itemTables[tableIndex];

        for (int i = 0; i < curItemTable.itemList.Length; i++)
        {
            if (curItemTable.itemList[i].item.Equals(item))
            {
                itemIndex = i;
                break;
            }
        }
    }
>>>>>>> ChaJinMin
}
