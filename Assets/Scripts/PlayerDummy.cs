using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour {

    public Vector3 originPos;

    private Rigidbody rb;
    private PlayerStat stat;

	void Awake () {
        rb = GetComponent<Rigidbody>();
        stat = GetComponent<PlayerStat>();
	}

    private void Update()
    {
        CheckFall();
    }

    [PunRPC]
    public void Pushed(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void CheckFall()
    {
        if (!stat.onStage && transform.position.y < -10.0f)
        {
            Respawn();
        } else if(transform.position.y < 4f)
        {
            stat.onStage = false;
        }
    }

    private void Respawn()
    {
        transform.position = originPos;
        stat.onStage = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
