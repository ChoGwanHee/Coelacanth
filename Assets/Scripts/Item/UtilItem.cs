using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UtilItem : BaseItem {

    public float delayTime;

    [FMODUnity.EventRef]
    public string usingSound;

    [FMODUnity.EventRef]
    public string useSound;

    public abstract void Execute(BuffController bc);
}
