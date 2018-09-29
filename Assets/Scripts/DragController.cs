using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지금 사용 안함
public class DragController : MonoBehaviour {

    public float maxPower = 20f;

    private Transform selectedObject;
    private Vector3 startPoint;
    private Vector3 endPoint;

    Ray ray;
    RaycastHit hit;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            startPoint = Input.mousePosition;
            ray = cam.ScreenPointToRay(startPoint);
            
            if(Physics.Raycast(ray, out hit))
            {
                if (!hit.transform.CompareTag("Object")) return;
                selectedObject = hit.transform;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedObject == null) return;

            endPoint = Input.mousePosition;

            Vector3 subVector = startPoint - endPoint;
            Vector3 powerVector = new Vector3(subVector.x, 0, subVector.y).normalized * maxPower;


            selectedObject.GetComponent<Rigidbody>().velocity = powerVector;

            selectedObject = null;
        }

        
	}
}
