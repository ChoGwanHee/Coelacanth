using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTasangyeonhwa : Photon.PunBehaviour {

    /// <summary>
    /// 공격력
    /// </summary>
    public int damage;

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
    /// 투사체 속력
    /// </summary>
    public float speed;

    /// <summary>
    /// 화면 진동 세기
    /// </summary>
    public float amplitude;

    /// <summary>
    /// 화면 진동 지속 시간
    /// </summary>
    public float duration;

    public Vector3 Velocity
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }

    private Rigidbody rb;
    private int dynamicObjMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dynamicObjMask = LayerMask.GetMask("DynamicObject");
    }



    
}
