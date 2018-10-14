using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff {

    public bool active;
    public float duration;
    public float elapsedTime;


    public abstract void OnStartBuff(BuffController bc);

    public abstract void OnEndBuff(BuffController bc);

    public virtual void OnStopBuff(BuffController bc)
    {
        if (active)
            OnEndBuff(bc);
    }

    public virtual void OnUpdateBuff(BuffController bc)
    {
        if (!active) return;

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= duration)
        {
            elapsedTime = 0f;
            active = false;
            OnEndBuff(bc);
        }
    }
    
}
