using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    private Rigidbody rb;
    private Collider col;

    private bool spawnable = true;
    public bool Spawnable
    {
        get { return spawnable; }
    }
    

	void Awake () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void TileShake()
    {
        spawnable = false;
        StartCoroutine(TileShakeProcess(2.0f));
    }

    private void Fall()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        col.enabled = false;
    }

    private void TileDisable()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private IEnumerator TileShakeProcess(float duration)
    {
        Vector3 originAngle = transform.localEulerAngles;
        Vector3 realAngle = originAngle;
        Vector3 noise = Vector3.zero;
        Vector3 noiseOffset = Vector3.zero;

        float frequency = 7.0f;
        float amplitude = 20.0f;
        float timeRemaining = duration;

        float rand = 32.0f;

        noiseOffset.x = Random.Range(0.0f, rand);
        noiseOffset.y = Random.Range(0.0f, rand);

        while (timeRemaining > 0.0f)
        {
            timeRemaining -= Time.deltaTime;

            float noiseOffsetDelta = Time.deltaTime * frequency;

            noiseOffset.x += noiseOffsetDelta;
            noiseOffset.y += noiseOffsetDelta;

            noise.x = Mathf.PerlinNoise(noiseOffset.x, 0.0f);
            noise.y = Mathf.PerlinNoise(noiseOffset.y, 1.0f);

            noise.x -= 0.5f;
            noise.y -= 0.5f;

            noise *= amplitude;

            realAngle = originAngle;
            realAngle.x += noise.x;
            realAngle.y += noise.y;
            transform.localEulerAngles = realAngle;

            yield return null;
        }

        transform.localEulerAngles = originAngle;


        // 타일 낙하
        Fall();

        yield return new WaitForSeconds(5.0f);

        // 타일 비활성화
        TileDisable();
    }
}
