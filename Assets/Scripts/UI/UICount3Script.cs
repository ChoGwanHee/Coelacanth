using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICount3Script : MonoBehaviour {

    [FMODUnity.EventRef]
    public string countSound;

    public void PlayCountSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(countSound);
    }
}
