using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityTasangyeonhwa : BaseMapFacility
{

    /// <summary>
    /// 떨어지는 폭죽 개수
    /// </summary>
    public int fallingAmount;

    /// <summary>
    /// 다음 폭죽 대기시간
    /// </summary>
    public float fallingInterval;

    /// <summary>
    /// 떨어지는 타상연화 폭죽
    /// </summary>
    public GameObject tasangProjectile;

    public TasangyeonhwaScript[] tasangs;

    public Transform[] hitRange;

    private int activatedTasang = -1;
    private int owner = -1;

    public Light directional;

    private void Start()
    {
        for (int i = 0; i < tasangs.Length; i++)
        {
            tasangs[i].parentFacility = this;
        }
    }

    public override void Activate()
    {
        photonView.RPC("AppearTasang", PhotonTargets.All, Random.Range(0, tasangs.Length));
    }

    public override void Deactivate()
    {
        StopAllCoroutines();
        for (int i = 0; i < tasangs.Length; i++)
        {
            tasangs[i].SetState(TasangyeonhwaScript.TasangState.Hide);
        }
    }

    public override bool CheckTime()
    {
        if (!enable) return false;

        if(activatedTasang == -1)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= interval)
            {
                elapsedTime = 0.0f;
                return true;
            }
        }

        return false;
    }

    [PunRPC]
    private void AppearTasang(int tasangNum)
    {
        activatedTasang = tasangNum;
        tasangs[activatedTasang].SetState(TasangyeonhwaScript.TasangState.Appear);
    }

    [PunRPC]
    public void RequestFire(int requestId)
    {
        if(owner == -1)
        {
            owner = requestId;

            photonView.RPC("Run", PhotonTargets.All, null);
        }
    }

    [PunRPC]
    private void Run()
    {
        StartCoroutine(FireProcess());
    }

    private IEnumerator FireProcess()
    {
        // 발사
        tasangs[activatedTasang].SetState(TasangyeonhwaScript.TasangState.Fire);
        yield return new WaitForSeconds(2.7f);

        // 하늘에 빛
        StartCoroutine(LightProcess());
        GameManagerPhoton._instance.cameraController.Shake(1.5f, 0.5f);
        yield return new WaitForSeconds(2.0f);

        // 하늘에서 떨어지기 시작
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(BombingProcess());
        }
        yield return new WaitForSeconds(3.0f);

        // 사라짐
        tasangs[activatedTasang].SetState(TasangyeonhwaScript.TasangState.Hide);
        activatedTasang = -1;
        owner = -1;
    }

    private IEnumerator BombingProcess()
    {
        for (int i = 0; i < fallingAmount; i++)
        {
            Vector3 hitPos = GetRandomHitPos();


            object[] data = new object[1] { owner };

            PhotonNetwork.Instantiate("Prefabs/" + tasangProjectile.name, hitPos, Quaternion.identity, 0, data);
            
            yield return new WaitForSeconds(fallingInterval);
        }
    }

    private IEnumerator LightProcess()
    {
        for(float f=1.8f; f>0.6f; f -= 0.02f)
        {
            directional.intensity = f;
            yield return null;
        }
        directional.intensity = 0.6f;
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
                randomIndex = Mathf.FloorToInt(UnityEngine.Random.value * (hitRange.Length / 2)) * 2;

                hitPos = GetRandomPos(hitRange[randomIndex].position, hitRange[randomIndex + 1].position);
                hitPos.y += 6.0f;

                retry1 = Physics.Raycast(hitPos, Vector3.down, out hit1, 6.1f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);

                if (++count > 10000)
                {
                    Debug.LogError("비정상적인 연산입니다.  count: " + count);
                    return Vector3.zero;
                }
            }
            while (!retry1);

            retry2 = Physics.SphereCast(hitPos, 1.0f, Vector3.down, out hit2, 6.1f, LayerMask.GetMask("StaticObject"));

        } while (retry2);

        hitPos = hit1.point;
        hitPos.y = 25.0f;

        return hitPos;
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

    
}
