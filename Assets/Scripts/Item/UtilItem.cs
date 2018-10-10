using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UtilItem : BaseItem {

    public float delayTime;

    public abstract void Execute(BuffController bc);
}
