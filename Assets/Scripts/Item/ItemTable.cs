using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Table")]

public class ItemTable : ScriptableObject {

    [Serializable]
    public struct ItemElement
    {
        public BaseItem item;
        public float chance;
    }

    [SerializeField]
    public ItemElement[] itemList;

    /// <summary>
    /// 아이템 목록의 확률에 따라 무작위로 아이템을 골라 인덱스를 반환합니다.
    /// </summary>
    /// <returns>아이템 인덱스</returns>
    public int RandomChoose()
    {
        if (itemList == null || itemList.Length <= 0)
        {
            Debug.Log("아이템 리스트가 비어있거나 null입니다");
            return -1;
        }

        float random;
        float total = 0;

        for (int i = 0; i < itemList.Length; i++)
        {
            total += itemList[i].chance;
        }
        random = UnityEngine.Random.value * total;

        for (int i = 0; i < itemList.Length; i++)
        {
            if (random < itemList[i].chance)
            {
                return i;
            }
            else
            {
                random -= itemList[i].chance;
            }
        }

        return itemList.Length - 1;
    }
}
