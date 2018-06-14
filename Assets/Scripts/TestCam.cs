﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCam : MonoBehaviour {

    public Transform target;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update () {
        transform.position = target.position + offset;
	}
}
