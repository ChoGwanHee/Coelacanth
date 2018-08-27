﻿using UnityEngine;

/// <summary>
/// 기본 투사체 클래스
/// </summary>
public abstract class BaseProjectile : Photon.PunBehaviour {

    /// <summary>
    /// 공격력
    /// </summary>
    [HideInInspector]
    public int damage;

    /// <summary>
    /// 넉백력
    /// </summary>
    [HideInInspector]
    public float hitForce;

    /// <summary>
    /// 공격 반경
    /// </summary>
    [HideInInspector]
    public float hitRadius;

    /// <summary>
    /// 최대 수명
    /// </summary>
    [HideInInspector]
    public float lifetime;

    /// <summary>
    /// 투사체 속력
    /// </summary>
    [HideInInspector]
    public float speed;

    /// <summary>
    /// 얻는 점수
    /// </summary>
    [HideInInspector]
    public int gainScore;

    /// <summary>
    /// 화면 진동 세기
    /// </summary>
    [HideInInspector]
    public float amplitude;

    /// <summary>
    /// 화면 진동 지속 시간
    /// </summary>
    [HideInInspector]
    public float duration;

    /// <summary>
    /// 날아가는 도중 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string duringSound;

    /// <summary>
    /// 사라질 때 재생되는 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string endSound;


    protected Rigidbody rb;
    protected int dynamicObjMask;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dynamicObjMask = LayerMask.GetMask("DynamicObject");
    }

    protected virtual void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 투사체의 속도을 설정합니다
    /// </summary>
    /// <param name="newVelocity">새로운 속도</param>
    public void SetVelocity(Vector3 newVelocity)
    {
        rb.velocity = newVelocity;
    }

    /// <summary>
    /// 투사체의 속력을 설정합니다
    /// </summary>
    /// <param name="newSpeed">새로운 속력</param>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        rb.velocity = transform.forward * speed;
    }

    /// <summary>
    /// 투사체가 사라질 때의 사운드를 재생합니다.
    /// </summary>
    [PunRPC]
    public void PlayEndSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(endSound);
    }
}
