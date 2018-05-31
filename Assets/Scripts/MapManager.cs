using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Photon.PunBehaviour {
    public static MapManager _instance;

    /// <summary>
    /// 물대포 재시작 간격
    /// </summary>
    public float waterCannonInterval = 60.0f;

    /// <summary>
    /// 마지막 물대포 작동으로 부터 지난 시간
    /// </summary>
    private float waterCannonElapsedTime = 30.0f;


    public WaterCannonScript[] waterCannons;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartMapFacilities();
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
            waterCannonElapsedTime += Time.deltaTime;

            if(waterCannonElapsedTime >= waterCannonInterval)
            {
                waterCannonElapsedTime = 0;
                photonView.RPC("StartWaterBlast", PhotonTargets.All, null);
            }

            yield return null;
        }
    }

    [PunRPC]
    private void StartWaterBlast()
    {
        for(int i=0; i<waterCannons.Length; i++)
        {
            waterCannons[i].StopAllCoroutines();
            waterCannons[i].StartCoroutine(waterCannons[i].Splash());
        }
    }

    
}
