using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Damage Event")]
public class DamageEvent : ScriptableObject {

    public bool useFireworkConfig;
    public int damage;
    public float amplitude;
    public float duration;
    public Firework referenceFirework;

	public int Damage
    {
        get { return useFireworkConfig ? referenceFirework.damage : damage; }
    }
    public float Amplitude
    {
        get { return useFireworkConfig ? referenceFirework.amplitude : amplitude; }
    }
    public float Duration
    {
        get { return useFireworkConfig ? referenceFirework.duration : duration; }
    }
}
