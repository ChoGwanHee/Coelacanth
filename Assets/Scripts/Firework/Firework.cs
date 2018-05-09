using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Firework : ScriptableObject {
    public enum AttackType
    {
        Range,
        Projectile,
        Installation
    }

    public string fwName;
    public FireworkType fwType;
    public AttackType atType;
    public int damage;
    public int capacity;
    public float hitForce;
    public float lifetime;
    public float coolDown;
    public Sprite uiSprite;
    [FMODUnity.EventRef]
    public string startSound;

    public virtual void Initialize(FireworkExecuter executer)
    {
        executer.Initialize();
    }

    public abstract void Execute(FireworkExecuter executer);
}
