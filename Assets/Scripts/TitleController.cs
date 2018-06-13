using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleController : MonoBehaviour {

    public bool videoBlink = true;

    [FMODUnity.EventRef]
    public string blinkSound;

    [FMODUnity.EventRef]
    public string titleBGM;

    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(titleBGM);
        StartCoroutine(BlinkVideo());
    }

    IEnumerator BlinkVideo()
    {
        while (videoBlink)
        {
            while (videoPlayer.clip.length < videoPlayer.time)
            {
                yield return null;
            }

            float randomTime = Random.Range(1.7f, 5.0f);
            yield return new WaitForSeconds(randomTime);

            videoPlayer.Play();
            videoPlayer.time = 0.3f;
            //FMODUnity.RuntimeManager.PlayOneShot(blinkSound);
        }
    }
}
