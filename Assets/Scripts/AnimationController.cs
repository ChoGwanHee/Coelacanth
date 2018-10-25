using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    public int aniNum;
    public int subAniNum;

    public Animator[] animator;

    bool isSub = false;

    int character = 0;
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetAniNum(0, isSub);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAniNum(1, isSub);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAniNum(2, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetAniNum(3, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetAniNum(4, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetAniNum(5, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetAniNum(6, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetAniNum(7, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetAniNum(8, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetAniNum(9, isSub);
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            SetAniNum(10, isSub);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            isSub = false;
            character = 0;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            isSub = false;
            character = 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isSub = false;
            character = 2;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            isSub = true;
            character = 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            isSub = true;
            character = 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isSub = true;
            character = 2;
        }

    }

    private void SetAniNum(int aniNum, bool isSub)
    {
        if (!isSub)
            animator[character].SetInteger("AniNum", aniNum);
        else
            animator[character].SetInteger("SubAniNum", aniNum);
    }

}
