using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : Photon.PunBehaviour {

    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float hitForce;
    [HideInInspector]
    public float hitRadius;
    [HideInInspector]
    public float lifetime;
    public float speed;
    [FMODUnity.EventRef]
    public string duringSound;
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

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        rb.velocity = transform.forward * speed;
    }
}
