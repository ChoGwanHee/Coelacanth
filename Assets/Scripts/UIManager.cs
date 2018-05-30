using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UIManager : MonoBehaviour {
    public static UIManager _instance;


    public UIStatus userStatus;
    public UIStatusOther[] otherStatus;
    public UIButterflyCharging chargingUI;

    public Sprite[] illusts;

    [HideInInspector]
    public int[] otherUserIndexLink = new int[4];
    private bool[] otherUserEnter = new bool[4];

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void UIInitialize(int playerIndex)
    {
        Color bgColor;

        // 본인 UI 설정
        switch (playerIndex)
        {
            case 0:
                bgColor = new Color(0.815f, 0.0f, 0.0f);
                break;
            case 1:
                bgColor = new Color(0.0f, 0.407f, 0.807f);
                break;
            case 2:
                bgColor = new Color(0.807f, 0.752f, 0.0f);
                break;
            case 3:
                bgColor = new Color(0.431f, 0.807f, 0.0f);
                break;
            default:
                bgColor = new Color(1.0f, 1.0f, 1.0f);
                break;
        }

        userStatus.ringImg.color = bgColor;
    }

    public void ActiveOtherStatus(int playerIndex)
    {
        Color bgColor;

        switch (playerIndex)
        {
            case 0:
                bgColor = new Color(0.815f, 0.0f, 0.0f);
                break;
            case 1:
                bgColor = new Color(0.0f, 0.407f, 0.807f);
                break;
            case 2:
                bgColor = new Color(0.807f, 0.752f, 0.0f);
                break;
            case 3:
                bgColor = new Color(0.431f, 0.807f, 0.0f);
                break;
            default:
                bgColor = new Color(1.0f, 1.0f, 1.0f);
                break;
        }

        int otherUserIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            if (!otherUserEnter[i])
            {
                otherUserEnter[i] = true;
                otherUserIndexLink[playerIndex] = i;
                otherUserIndex = i;
                break;
            }
        }

        if (otherUserIndex == -1)
        {
            Debug.Log("남은 플레이어 UI가 없습니다");
            return;
        }

        otherStatus[otherUserIndex].SetHeart(6);
        //otherStatus[otherUserIndex].backgroundImg.color = bgColor;
        otherStatus[otherUserIndex].illust.sprite = illusts[playerIndex];
        otherStatus[otherUserIndex].gameObject.SetActive(true);
    }

    public void DeActiveOtherStatus(int playerIndex)
    {
        otherStatus[otherUserIndexLink[playerIndex]].gameObject.SetActive(false);
        otherStatus[otherUserIndexLink[playerIndex]].SetHeart(6);
        otherUserEnter[otherUserIndexLink[playerIndex]] = false;
        otherUserIndexLink[playerIndex] = 0;
    }
}
