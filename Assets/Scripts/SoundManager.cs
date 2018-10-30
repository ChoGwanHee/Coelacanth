using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager _instance;

    /// <summary>
    /// 게임 배경음악
    /// </summary>
    public FMOD.Studio.EventInstance BGMEvent;


    bool bgmEnable = true;


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!DebugTool._instance.debugEnable) return;

            
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (bgmEnable)
            {
                BGMEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                bgmEnable = false;
            }
            else
            {
                BGMEvent.start();
                bgmEnable = true;
            }
        }
    }

    public void SetBGM(string sound)
    {
        if (sound.Length < 1) return;

        FMOD.Studio.PLAYBACK_STATE soundState;
        BGMEvent.getPlaybackState(out soundState);

        if(soundState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            BGMEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        BGMEvent = FMODUnity.RuntimeManager.CreateInstance(sound);
        BGMEvent.start();
    }
}
