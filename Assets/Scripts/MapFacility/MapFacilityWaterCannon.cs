using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFacilityWaterCannon : BaseMapFacility
{
    public WaterCannonScript[] waterCannons;

    private void Awake()
    {
        elapsedTime = 30.0f;
    }

    public override void Activate()
    {
        for (int i = 0; i < waterCannons.Length; i++)
        {
            waterCannons[i].StopAllCoroutines();
            waterCannons[i].StartCoroutine(waterCannons[i].Splash());
        }
    }
    
}
