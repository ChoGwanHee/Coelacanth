using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Photon.PunBehaviour
{
    public static MapManager _instance;

    /// <summary>
    /// 플레이어가 떨어지기 시작하는 높이 (트윈빌라 기준 4.4f)
    /// </summary>
    public float fallingHeight = 4.4f;

    public BaseMapFacility[] mapFacilities;

    /// <summary>
    /// 시간이 지남에 따라 정기적으로 작동하는 맵시설의 리스트
    /// </summary>
    private List<BaseMapFacility> regularMapFacilities = new List<BaseMapFacility>();


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //StartMapFacilities();
    }

    /// <summary>
    /// 주기적으로 작동하는 맵 시설 리스트에 맵 시설을 추가합니다.
    /// </summary>
    /// <param name="facility"></param>
    public void AddRegularMapFacility(BaseMapFacility facility)
    {
        regularMapFacilities.Add(facility);
    }

    /// <summary>
    /// 맵 시설들의 작동을 시작합니다.
    /// </summary>
    public void StartMapFacilities()
    {
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(LoopMapFacilities());
        }
    }

    /// <summary>
    /// 맵 시설들의 작동을 정지시킵니다.
    /// </summary>
    [PunRPC]
    public void StopMapFacilities()
    {
        StopAllCoroutines();
        for (int i = 0; i < mapFacilities.Length; i++)
        {
            mapFacilities[i].Deactivate();
        }
    }

    /// <summary>
    /// 주기적으로 작동하는 맵 시설의 시간을 확인합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoopMapFacilities()
    {
        while (true)
        {
            for(int i=0; i<regularMapFacilities.Count; i++)
            {
                if (regularMapFacilities[i].CheckTime())
                {
                    photonView.RPC("MapFacilityActivate", PhotonTargets.All, i);
                }
            }

            yield return new WaitUntil(() => GameManagerPhoton._instance.timeProgress == true);
        }
    }

    /// <summary>
    /// 개별적인 맵 시설을 작동시킵니다.
    /// </summary>
    /// <param name="index"></param>
    [PunRPC]
    private void MapFacilityActivate(int index)
    {
        regularMapFacilities[index].Activate();
    }
}
