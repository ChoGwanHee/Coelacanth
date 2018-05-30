using UnityEngine;
using System;

/// <summary>
/// 폭죽의 기본 클래스
/// </summary>
[Serializable]
public abstract class Firework : ScriptableObject {

    /// <summary>
    /// 공격 방식에 대한 유형 정의
    /// [ Range:범위, Projectile:투사체, Installation:설치 ]
    /// </summary>
    public enum AttackType
    {
        Range,
        Projectile,
        Installation
    }

    /// <summary>
    /// 폭죽 이름(한글), 현재 쓰이는 곳 없음
    /// </summary>
    public string fwName;

    /// <summary>
    /// 폭죽 유형
    /// </summary>
    public FireworkType fwType;

    /// <summary>
    /// 공격 유형
    /// </summary>
    public AttackType atType;

    /// <summary>
    /// 공격력
    /// </summary>
    public int damage;

    /// <summary>
    /// 탄창 용량
    /// </summary>
    public int capacity;

    /// <summary>
    /// 넉백력
    /// </summary>
    public float hitForce;

    /// <summary>
    /// 공격 반경
    /// </summary>
    public float hitRadius;

    /// <summary>
    /// 최대 수명
    /// </summary>
    public float lifetime;

    /// <summary>
    /// 재사용 대기 시간
    /// </summary>
    public float coolDown;

    /// <summary>
    /// 폭죽 UI 스프라이트
    /// </summary>
    public Sprite uiSprite;

    /// <summary>
    /// 폭죽 사용 시 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string startSound;


    public abstract void Execute(FireworkExecuter executer);
}
