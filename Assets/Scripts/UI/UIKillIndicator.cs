using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKillIndicator : MonoBehaviour {

    public Sprite[] faceList;

    [FMODUnity.EventRef]
    public string killSound;

    private Animator anim;
    private Image faceImg;
	

    void Awake()
    {
        anim = GetComponent<Animator>();
        faceImg = GetComponentInChildren<Image>();
    }

	public void SetIndicator(int characterNum)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        faceImg.sprite = faceList[characterNum];
        anim.SetTrigger("Trigger");
        FMODUnity.RuntimeManager.PlayOneShot(killSound);
    }

    public void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
