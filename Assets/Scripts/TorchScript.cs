using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour {

    private Light torchLight;
    private float originIntensity;
    private float targetIntensity;
    private float preIntensity;

    public float defaultChangeTime = 0.2f;
    private float changeTime;
    private float elapsedTime = 0.0f;

	
	void Awake () {
        torchLight = GetComponent<Light>();
        originIntensity = preIntensity = torchLight.intensity;
        changeTime = defaultChangeTime;
        targetIntensity = originIntensity;
    }

	void Update () {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= changeTime)
        {
            preIntensity = targetIntensity;
            targetIntensity = originIntensity + Random.Range(-0.4f, 0.3f);
            changeTime = defaultChangeTime - Random.Range(0f, 0.1f);
            elapsedTime = 0f;
        }

        float progress = elapsedTime / changeTime;

        torchLight.intensity = Mathf.Lerp(preIntensity, targetIntensity, progress);
	}
}
