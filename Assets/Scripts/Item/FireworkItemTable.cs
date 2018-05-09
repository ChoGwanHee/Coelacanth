using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName ="ItemTable/Firework")]

public class FireworkItemTable : ScriptableObject {

    [Serializable]
    public struct Item
    {
        public Firework item;
        public float chance;
    }

    [SerializeField]
    public Item[] itemList;

    public int RandomChoose()
    {
        if (itemList == null || itemList.Length <= 0) {
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
