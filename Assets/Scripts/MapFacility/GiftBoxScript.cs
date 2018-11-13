using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBoxScript : MonoBehaviour {

    public Transform cover;

    public float boomPower = 13.0f;

    public ParticleSystem boomEfx;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        boomEfx.Stop(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            anim.SetBool("IsFire", true);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            anim.SetBool("IsFire", false);
            Rigidbody coverRb = cover.GetComponent<Rigidbody>();
            coverRb.useGravity = false;
        }
    }
    

    public void Boom()
    {
        Rigidbody coverRb = cover.GetComponent<Rigidbody>();
        coverRb.useGravity = true;
        coverRb.velocity = new Vector3(0.0f, boomPower, 0.0f);
        coverRb.angularVelocity = new Vector3(Random.Range(0.0f, 0.6f), Random.Range(0.0f, 0.6f), Random.Range(0.0f, 0.6f));

        boomEfx.Play(true);
    }

}
