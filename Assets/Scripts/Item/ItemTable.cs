using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 확장성을 위해 필요하다고 생각했으나 Generic 형식을 쓰면 유니티 인스펙터에 안나타나는 문제가 있어서
/// 보류, 현재 사용하지 않고 있음
/// </summary>
[Serializable]
public abstract class ItemTable : ScriptableObject {

    [Serializable]
	public struct Item<T>
    {
        public T item;
        public float chance;
    }

}
