using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Photon.PunBehaviour
{
    public static MapManager _instance;

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

    public void AddRegularMapFacility(BaseMapFacility facility)
    {
        regularMapFacilities.Add(facility);
    }

    public void StartMapFacilities()
    {
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(LoopMapFacilities());
        }
    }

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

    [PunRPC]
    private void MapFacilityActivate(int index)
    {
        regularMapFacilities[index].Activate();
    }
    
}
