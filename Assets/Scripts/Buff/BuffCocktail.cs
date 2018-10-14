using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCocktail : Buff
{
    ItemCocktail item;


    public BuffCocktail(ItemCocktail item)
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
        bc.upEfx.Play(true);
        bc.PC.maxSpeedFactor += item.addSpeed;
        bc.PC.Executer.damageFactor += item.addDamage;
        active = true;
    }

    public override void OnEndBuff(BuffController bc)
    {
        bc.upEfx.Stop(true);
        bc.PC.maxSpeedFactor -= item.addSpeed;
        bc.PC.Executer.damageFactor -= item.addDamage;
        active = false;
    }
}
