using UnityEngine;

public abstract class Buff {

    protected bool active;
    protected float duration;
    protected float elapsedTime;


    public virtual void StartBuff(BuffController bc)
    {
        elapsedTime = 0;
        if (!active)
        {
            OnStartBuff(bc);
            bc.onUpdateBuff += OnUpdateBuff;
            active = true;
        }
    }

    public virtual void StopBuff(BuffController bc)
    {
        if (active)
        {
            OnEndBuff(bc);
            active = false;
        }
    }

    protected abstract void OnStartBuff(BuffController bc);
    protected abstract void OnEndBuff(BuffController bc);
    

    public virtual void OnUpdateBuff(BuffController bc)
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= duration)
        {
            OnEndBuff(bc);
            active = false;
        }
    }
    
}
