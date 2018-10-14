using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUnbeatable : Buff
{
    ItemStarFruit item;


    public BuffUnbeatable(ItemStarFruit item)
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
        bc.PC.isUnbeatable = true;
        bc.shiedEfx.SetActive(true);
        active = true;
    }

    public override void OnEndBuff(BuffController bc)
    {
        bc.PC.isUnbeatable = false;
        bc.shiedEfx.SetActive(false);
        active = false;
    }
    
}
