using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    AllView,
    PersonalView
}

public class CameraController : Photon.PunBehaviour {

    public Transform target;
    public CameraMode mode;
    public float smoothMoveTime = 1.0f;
    public float smoothZoomTime = 0.8f;
    public float baseAddOffset = 6.0f;
    [Range(0.5f, 2)]
    public float zoomXFactor = 0.85f;
    [Range(0.5f, 2)]
    public float zoomZFactor = 1.0f;
    public float minZoomDistance = 10.0f;
    public float maxZoomDistance = 80.0f;

    // Pivot
    private Vector3 moveVelocity;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 minPos = Vector3.zero;
    private Vector3 maxPos = Vector3.zero;
    private float displayedPlayerCount = 0;
    private Vector3 originPosition;

    // Zoom 관련
    private float targetZoomDistance;
    private float distanceVelocity;


    private List<GameObject> playerList = new List<GameObject>();
    private Transform camT;

    private Vector3 defaultMinMax = new Vector3(50f, 0f, 50f);
    

    void Start () {
        camT = Camera.main.transform;
        targetPosition.y = transform.position.y;
        originPosition = transform.position;
    }
	
	void FixedUpdate () {

        if (playerList.Count <= 0) return;

        switch (mode)
        {
            case CameraMode.AllView:
                CalculateMinMax();
                CalculateCenter2();
                AutoZoom();
                break;
            case CameraMode.PersonalView:
                FollowTarget();
                break;
        }

        SmoothMovement();
    }

    public void FindPlayers()
    {
        playerList.Clear();
        playerList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    // 최소, 최대 위치 계산
    void CalculateMinMax()
    {
        minPos = defaultMinMax;
        maxPos = defaultMinMax * -1;

        displayedPlayerCount = 0;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].gameObject.activeSelf) continue;
            if (!playerList[i].GetComponent<PlayerStat>().onStage) continue;

            displayedPlayerCount++;

            if (minPos.x > playerList[i].transform.position.x)
                minPos.x = playerList[i].transform.position.x;
            if (minPos.z > playerList[i].transform.position.z)
                minPos.z = playerList[i].transform.position.z;

            if (maxPos.x < playerList[i].transform.position.x)
                maxPos.x = playerList[i].transform.position.x;
            if (maxPos.z < playerList[i].transform.position.z)
                maxPos.z = playerList[i].transform.position.z;
        }

        if (displayedPlayerCount == 0)
        {
            minPos = Vector3.zero;
            maxPos = Vector3.zero;
        }

    }

    // 전체적인 위치만 판단
    void CalculateCenter()
    {
        targetPosition.x = (minPos.x + maxPos.x) / 2;
        targetPosition.z = (minPos.z + maxPos.z) / 2;
    }

    // 사람 몰려있는 곳에 좀 더 집중 됨
    void CalculateCenter2()
    {
        Vector3 sumPos = Vector3.zero;

        for(int i=0; i<playerList.Count; i++)
        {
            if (!playerList[i].gameObject.activeSelf) continue;
            if (!playerList[i].GetComponent<PlayerStat>().onStage) continue;

            sumPos += playerList[i].transform.position;
        }

        sumPos += originPosition;


        if(displayedPlayerCount > 0)
            targetPosition = sumPos / (displayedPlayerCount+1);
        else
        {
            targetPosition = originPosition;
        }

    }

    // 타겟 따라 다니기
    void FollowTarget()
    {
        if (target == null) return;

        targetPosition = target.position;
    }

    // 부드러운 움직임
    void SmoothMovement()
    {
        // Pivot 이동
        transform.position = Vector3.SmoothDamp(transform.position, this.targetPosition, ref moveVelocity, smoothMoveTime);

        // 카메라 이동(줌 인,아웃)
        //camT.localPosition = Vector3.SmoothDamp(camT.localPosition, offsetPosition, ref offsetVelocity, smoothMoveTime);
        float distance = Mathf.SmoothDamp(camT.localPosition.z, targetZoomDistance, ref distanceVelocity, smoothZoomTime);
        Vector3 targetPosition = camT.localPosition;
        targetPosition.z = distance;
        camT.localPosition = targetPosition;
    }

    // 줌 길이 계산
    void AutoZoom()
    {
        float subX = maxPos.x - minPos.x;
        float subZ = maxPos.z - minPos.z;
        float bigger;

        if (subX > subZ)
        {
            bigger = subX * zoomXFactor;
        }
        else
        {
            bigger = subZ * zoomZFactor;
        }

        targetZoomDistance = bigger + baseAddOffset;

        if( targetZoomDistance < minZoomDistance)
        {
            targetZoomDistance = minZoomDistance;
        }
        else if(targetZoomDistance > maxZoomDistance)
        {
            targetZoomDistance = maxZoomDistance;
        }

        targetZoomDistance *= -1;
        //offsetPosition = offsetDirection * (bigger + zoomAddOffset);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
