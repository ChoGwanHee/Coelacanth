﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 손에 들 수 있는 폭죽 오브젝트 묶음
/// </summary>
[CreateAssetMenu(menuName = "Firework/Hand Object Set")]
public class FireworkHandObjectSet : ScriptableObject {

    public GameObject[] objects;
}
