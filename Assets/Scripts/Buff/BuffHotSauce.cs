using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHotSauce : Buff
{
    ItemHotSauce item;
    

    public BuffHotSauce(ItemHotSauce item)
    {
        this.item = item;
        duration = item.duration;
    }

    public override void OnStartBuff(BuffController bc)
    {
        if (active)
        {
            elapsedTime = 0f;
            return;
        }
        bc.buffEfx.Play(true);
    }

    public override void OnEndBuff(BuffController bc)
    {
    }

    public override void OnUpdateBuff(BuffController bc)
    {
        base.OnUpdateBuff(bc);
    }
}
