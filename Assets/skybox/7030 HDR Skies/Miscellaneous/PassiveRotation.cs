using UnityEngine;
using System.Collections;

public class PassiveRotation : MonoBehaviour {

	public enum RotationAxis { xAxis = 0, yAxis = 1, zAxis = 2 };
	public RotationAxis axis = RotationAxis.xAxis;
	public float rotationSpeed;

	void Update(){

		if (axis == RotationAxis.xAxis)
		{
			transform.eulerAngles += new Vector3(rotationSpeed,0,0);
		}
		if (axis == RotationAxis.yAxis)
		{
			transform.eulerAngles += new Vector3(0,rotationSpeed,0);
		}
		if (axis == RotationAxis.zAxis)
		{
			transform.eulerAngles += new Vector3(0,0,rotationSpeed);
		}

	}
}
