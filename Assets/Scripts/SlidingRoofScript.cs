using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingRoofScript : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

            pc.isControlable = false;
        }*/
    }
}
