using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour {
    void Update () {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                hit.collider.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }
    }
}
