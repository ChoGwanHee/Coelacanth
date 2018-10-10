using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shake Event")]
public class ShakeEvent : ScriptableObject {

    public bool useFireworkConfig;
    public float amplitude;
    public float duration;
    public Firework referenceFirework;

    public float Amplitude
    {
        get { return useFireworkConfig ? referenceFirework.amplitude : amplitude; }
    }
    public float Duration
    {
        get { return useFireworkConfig ? referenceFirework.duration : duration; }
    }
}
