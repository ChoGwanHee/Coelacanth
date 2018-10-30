using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRespawn : MonoBehaviour {

    public UIRespawnCounter[] counters;

    public void SetRespawnCounter(float remainTime, Vector3 pos, int chrNum, bool isMine)
    {
        for(int i=0; i<counters.Length; i++)
        {
            if (!counters[i].alive)
            {
                counters[i].SetCount(remainTime, pos, chrNum, isMine);
                break;
            }
        }
    }

    public void Init()
    {
        for (int i = 0; i < counters.Length; i++)
        {
            if(counters[i].alive)
                counters[i].Disable();
        }
    }
}
