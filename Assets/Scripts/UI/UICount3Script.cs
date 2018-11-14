
using UnityEngine;

public class UICount3Script : MonoBehaviour {

    [FMODUnity.EventRef]
    public string startCountSound;

    [FMODUnity.EventRef]
    public string endCountSound;

    [HideInInspector]
    public bool isStart;

    private Animator anim;
    public Animator Anim { get { return anim; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayCountSound()
    {
        if(isStart)
            FMODUnity.RuntimeManager.PlayOneShot(startCountSound);
        else
            FMODUnity.RuntimeManager.PlayOneShot(endCountSound);
    }
}
