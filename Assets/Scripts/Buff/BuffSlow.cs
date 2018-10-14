using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSlow : Buff
{
    ItemGel item;


    public BuffSlow(ItemGel item)
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
        bc.debuffEfx.Play(true);
        bc.slowEfx.Play(true);
        active = true;
    }

    public override void OnEndBuff(BuffController bc)
    {
        bc.slowEfx.Stop(true);
        active = false;
    }
}
