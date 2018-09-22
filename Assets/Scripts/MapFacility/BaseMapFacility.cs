using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMapFacility : MonoBehaviour
{
    /// <summary>
    /// 시설이 작동하는지 여부
    /// </summary>
    public bool enable;

    /// <summary>
    /// 작동 주기
    /// </summary>
    public float interval;

    /// <summary>
    /// 경과한 시간
    /// </summary>
    protected float elapsedTime = 0.0f;



    protected void Start()
    {
        if (enable)
            MapManager._instance.AddRegularMapFacility(this);
    }


    /// <summary>
    /// 시설을 작동시킵니다.
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// 시설이 작동할 시간이 되었는지 확인합니다.
    /// </summary>
    public bool CheckTime()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= interval)
        {
            elapsedTime = 0.0f;
            return true;
        }

        return false;
    }
}
