using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public enum CameraMode
{
    TotalView,
    PersonalView,
    FrontView,
    PersonalView2
}

public class CameraController : Photon.PunBehaviour {

    public Transform target;
    public CameraMode mode;


    // Move
    public float smoothMoveTime = 0.7f;
    public float fastSmoothMoveTime = 0.2f;
    public float fastMoveDistance = 5.0f;
    public float baseAddOffset = 6.0f;

    public float minPixel = 150.0f;
    public float maxPixel = 300.0f;
    //public float minDistance = 3.0f;
    //public float maxDistance = 6.0f;

    private Vector3 moveVelocity;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 minPos = Vector3.zero;
    private Vector3 maxPos = Vector3.zero;
    private float displayedPlayerCount = 0;
    private Vector3 originPosition;



    // Zoom
    public float smoothZoomTime = 0.9f;
    [Range(0.5f, 2)]
    public float zoomXFactor = 1.4f;
    [Range(0.5f, 2)]
    public float zoomZFactor = 1.5f;
    public float minZoomDistance = 20.0f;
    public float maxZoomDistance = 30.0f;

    private float targetZoomDistance;
    private float distanceVelocity;
    private float originZoomDistance;



    private float originXAngle;
    private float targetXAngle;
    private float rotationXVelocity;


    public PostProcessingProfile zoomProfile;
    private PostProcessingProfile originProfile;


    private Transform camT;
    private Transform pivotT;

    private PlayerStat targetStat;

    private PostProcessingBehaviour ppb;

    private Vector3 defaultMinMax = new Vector3(50f, 0f, 50f);
    

    void Start () {
        camT = Camera.main.transform;
        ppb = camT.GetComponent<PostProcessingBehaviour>();
        originProfile = ppb.profile;
        pivotT = transform.GetChild(0);
        targetPosition.y = transform.position.y;
        originPosition = transform.position;
        originZoomDistance = camT.localPosition.z;
        originXAngle = pivotT.eulerAngles.x;
        targetXAngle = originXAngle;

    }

    private void OnDisable()
    {
        ppb.profile = originProfile;
    }

    void FixedUpdate () {

        switch (mode)
        {
            case CameraMode.TotalView:
                CalculateMinMax();
                CalculateCenter2();
                AutoZoom();
                SmoothMovement();
                SmoothZoom();
                break;
            case CameraMode.PersonalView:
                SingleCalculateMinMax();
                FollowTargetFocusCenter();
                AutoZoom();
                SmoothMovement();
                SmoothZoom();
                break;
            case CameraMode.FrontView:
                FollowTarget();
                SmoothMovement();
                SmoothZoom();
                SmoothRotation();
                break;
            case CameraMode.PersonalView2:
                CalculateCenter3();
                SmoothMovement();
                break;
        }
    }

    // 최소, 최대 위치 계산
    void CalculateMinMax()
    {
        minPos = defaultMinMax;
        maxPos = defaultMinMax * -1;

        displayedPlayerCount = 0;
        List<PlayerStat> playerList = GameManagerPhoton._instance.playerList;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].gameObject.activeSelf) continue;
            if (!playerList[i].onStage) continue;

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

    // 최소, 최대 위치 계산
    void SingleCalculateMinMax()
    {
        if (target != null && targetStat != null && targetStat.onStage)
        {
            minPos = originPosition;
            maxPos = originPosition;

            if (originPosition.x > target.position.x)
                minPos.x = target.position.x;
            if (originPosition.z > target.position.z)
                minPos.z = target.position.z;

            if (originPosition.x < target.position.x)
                maxPos.x = target.position.x;
            if (originPosition.z < target.position.z)
                maxPos.z = target.position.z;
        }
        else
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

        List<PlayerStat> playerList = GameManagerPhoton._instance.playerList;

        for (int i=0; i<playerList.Count; i++)
        {
            if (!playerList[i].gameObject.activeSelf) continue;
            if (!playerList[i].onStage) continue;

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
    void FollowTargetFocusCenter()
    {
        if (target != null && targetStat != null && targetStat.onStage)
        {
            targetPosition = Vector3.Lerp(originPosition, target.position, 0.35f);
        }
        else
        {
            targetPosition = originPosition;
        }
    }

    void FollowTarget()
    {
        if (target != null && targetStat != null && targetStat.onStage)
        {
            targetPosition = target.position;
        }
        else
        {
            targetPosition = originPosition;
        }
    }

    void CalculateCenter3()
    {
        /*if (target != null && targetStat != null && targetStat.onStage)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(mouseRay, out hit, 50.0f, LayerMask.GetMask("Ground"));

            Vector3 centerPosition = Vector3.Lerp(target.position, hit.point, 0.5f);
            Vector3 subVec = centerPosition - target.position;


            if (subVec.sqrMagnitude < minDistance * minDistance)
            {
                targetPosition = target.position;
            }
            else if(subVec.sqrMagnitude > maxDistance * maxDistance)
            {
                targetPosition = target.position + subVec.normalized * maxDistance;
            }
            else
            {
                targetPosition = centerPosition;
            }

        }
        else
        {
            targetPosition = originPosition;
        }*/

        if(target != null && targetStat != null && targetStat.onStage)
        {
            Vector3 mouseVec = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
            float mouseVecSqrMag = mouseVec.sqrMagnitude;

            Vector3 toMouseDirection = new Vector3(mouseVec.x, 0, mouseVec.y).normalized;

            if (mouseVecSqrMag < minPixel * minPixel)
            {
                targetPosition = target.position;
            }
            else if(mouseVecSqrMag > maxPixel * maxPixel)
            {
                targetPosition = target.position + toMouseDirection * (maxPixel / 100);
            }
            else
            {
                targetPosition = target.position + toMouseDirection * (mouseVec.magnitude / 100);
            }
            
        }
        else
        {
            targetPosition = originPosition;
        }

    }



    // 부드러운 움직임
    void SmoothMovement()
    {
        // Pivot 이동
        float subDistance = (targetPosition - transform.position).magnitude;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref moveVelocity,
            (subDistance < fastMoveDistance) ? smoothMoveTime : fastSmoothMoveTime);
    }

    // 부드러운 확대 축소
    void SmoothZoom()
    {
        // 카메라 이동(줌 인,아웃)
        float distance = Mathf.SmoothDamp(camT.localPosition.z, targetZoomDistance, ref distanceVelocity, smoothZoomTime);
        Vector3 targetZoomPosition = camT.localPosition;
        targetZoomPosition.z = distance;
        camT.localPosition = targetZoomPosition;
    }

    // 줌 길이 계산
    void AutoZoom()
    {
        if (target != null && targetStat != null && targetStat.onStage)
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

            if (targetZoomDistance < minZoomDistance)
            {
                targetZoomDistance = minZoomDistance;
            }
            else if (targetZoomDistance > maxZoomDistance)
            {
                targetZoomDistance = maxZoomDistance;
            }

            targetZoomDistance *= -1;
        }
        else
        {
            targetZoomDistance = originZoomDistance;
        }
    }

    // 부드러운 각도 조절
    void SmoothRotation()
    {
        float newXAngle = Mathf.SmoothDampAngle(pivotT.localEulerAngles.x, targetXAngle, ref rotationXVelocity, smoothZoomTime);
        Vector3 newRotation = pivotT.localEulerAngles;
        newRotation.x = newXAngle;
        pivotT.localEulerAngles = newRotation;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetStat = target.GetComponent<PlayerStat>();
    }

    public void ChangeMode(CameraMode newMode)
    {
        switch (newMode)
        {
            case CameraMode.TotalView:
                break;
            case CameraMode.PersonalView:
                break;
            case CameraMode.FrontView:
                targetXAngle = 3.0f;
                targetZoomDistance = -4.0f;
                ppb.profile = zoomProfile;
                break;
            case CameraMode.PersonalView2:
                targetXAngle = originXAngle;
                targetZoomDistance = originZoomDistance;
                ppb.profile = originProfile;
                break;
        }

        mode = newMode;
    }
}
