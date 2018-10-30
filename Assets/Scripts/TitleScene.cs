using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {

    /// <summary>
    /// 게임 배경음악
    /// </summary>
    [FMODUnity.EventRef]
    public string BGM;

    /// <summary>
    /// 배경에서 터지는 폭죽 사운드
    /// </summary>
    [FMODUnity.EventRef]
    public string backgroundFirework;
    FMOD.Studio.EventInstance backgroundFireworkEvent;


    private void Start()
    {
        // BGM
        SoundManager._instance.SetBGM(BGM);

        backgroundFireworkEvent = FMODUnity.RuntimeManager.CreateInstance(backgroundFirework);
        backgroundFireworkEvent.start();
    }

    private void OnDestroy()
    {
        backgroundFireworkEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
