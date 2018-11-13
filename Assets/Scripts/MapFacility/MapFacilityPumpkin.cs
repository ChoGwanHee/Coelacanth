using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityPumpkin : BaseMapFacility
{

    /// <summary>
    /// 호박이 생성되는 개수
    /// </summary>
    public int pumpkinAmount;

    /// <summary>
    /// 다음 생성 대기시간
    /// </summary>
    public float createInterval;
    

    /// <summary>
    /// 호박 오브젝트 레퍼런스
    /// </summary>
    public GameObject[] pumpkin_ref;

    /// <summary>
    /// 생성 범위
    /// </summary>
    public Transform[] createRange;

    public float minDistance = 1.5f;
    private float minDistanceSqr;
    public int totalCount = 5;
    private int curIndex = 0;
    private Vector3[] spawnedPoints;
    

	void Awake () {
        spawnedPoints = new Vector3[totalCount];
        minDistanceSqr = minDistance * minDistance;
    }

    public override void First()
    {
    }

    public override void Activate()
    {
        InitSpawnedPoints();
        StartCoroutine(PumpkinProcess());
    }

    public override void Deactivate()
    {
        StopAllCoroutines();
    }

    private IEnumerator PumpkinProcess()
    {
        for(int i=0; i<pumpkinAmount; i++)
        {
            Vector3 spawnPos;
            int count = 0;
            spawnedPoints[curIndex] = new Vector3(99f, 99f);
            do
            {
                spawnPos = GetRandomHitPos();
                if(++count > 10000)
                {
                    Debug.LogError("비정상적인 연산입니다.  count: " + count);
                    break;
                }
            }
            while (IsClosePoint(ref spawnPos));
            spawnedPoints[curIndex++] = spawnPos;
            if (curIndex >= totalCount) curIndex = 0;

            int random = Random.Range(0, pumpkin_ref.Length);
            PhotonNetwork.Instantiate("Prefabs/" + pumpkin_ref[random].name, spawnPos, Quaternion.Euler(0.0f, 180.0f, 0.0f), 0);
            yield return new WaitForSeconds(createInterval);
        }
    }

    private Vector3 GetRandomHitPos()
    {
        Vector3 hitPos;
        RaycastHit hit1, hit2;
        bool retry1, retry2;
        int randomIndex;
        int count = 0;

        do
        {
            do
            {
                randomIndex = Mathf.FloorToInt(UnityEngine.Random.value * (createRange.Length / 2)) * 2;

                hitPos = GetRandomPos(createRange[randomIndex].position, createRange[randomIndex + 1].position);
                hitPos.y += 3.0f;

                retry1 = Physics.Raycast(hitPos, Vector3.down, out hit1, 3.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

                if (++count > 10000)
                {
                    Debug.LogError("비정상적인 연산입니다.  count: " + count);
                    return Vector3.zero;
                }
            }
            while (!retry1);

            retry2 = Physics.SphereCast(hitPos, 1.0f, Vector3.down, out hit2, 3.1f, LayerMask.GetMask("StaticObject"));

        } while (retry2);

        return hit1.point;
    }

    /// <summary>
    /// 두 위치를 기준으로 사각형 범위 안의 위치를 랜덤으로 얻습니다.
    /// </summary>
    /// <param name="pos1">첫 번째 기준</param>
    /// <param name="pos2">두 번째 기준</param>
    /// <returns>랜덤 위치</returns>
    private Vector3 GetRandomPos(Vector3 pos1, Vector3 pos2)
    {
        Vector3 minPos = Vector3.zero;
        Vector3 maxPos = Vector3.zero;

        if (pos1.x > pos2.x)
        {
            minPos.x = pos2.x;
            maxPos.x = pos1.x;
        }
        else
        {
            minPos.x = pos1.x;
            maxPos.x = pos2.x;
        }

        if (pos1.z > pos2.z)
        {
            maxPos.z = pos1.z;
            minPos.z = pos2.z;
        }
        else
        {
            maxPos.z = pos2.z;
            minPos.z = pos1.z;
        }

        Vector3 result = Vector3.zero;

        result.x = UnityEngine.Random.Range(minPos.x, maxPos.x);
        result.z = UnityEngine.Random.Range(minPos.z, maxPos.z);
        result.y = pos1.y;

        return result;
    }

    private void InitSpawnedPoints()
    {
        for (int i = 0; i < spawnedPoints.Length; i++)
        {
            spawnedPoints[i] = new Vector3(99f, 99f);
        }
    }

    private bool IsClosePoint(ref Vector3 newPoint)
    {
        for (int i = 0; i < spawnedPoints.Length; i++)
        {
            float diffSqr = (spawnedPoints[i] - newPoint).sqrMagnitude;

            if(diffSqr <= minDistanceSqr)
            {
                return true;
            }
        }
        return false;
    }
}
