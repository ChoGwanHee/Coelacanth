using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UIManager : MonoBehaviour {
    public static UIManager _instance;

    public enum ScreenType
    {
        GamePlay,
        Result
    }

    // Game Play
    public GameObject gamePlayCanvas;
    public UIStatus userStatus;
    public UIButterflyCharging chargingUI;
    public UIScoreBoard scoreBoard;
    public UIAmmoGauge gauge;
    public UIAmmoCount ammoCounter;
    public UITimer timer;
    public Animator counterAnim;

    public Sprite[] illusts;


    // Result
    public GameObject resultCanvas;
    public UIResultScoreBoard resultScoreBoard;
    public UIResultEmotion resultEmotion;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 화면에 표시되는 UI들을 바꿉니다.
    /// </summary>
    /// <param name="newScreen">바꿀 타입</param>
    public void ChangeScreen(ScreenType newScreen)
    {
        switch (newScreen)
        {
            case ScreenType.GamePlay:
                gamePlayCanvas.SetActive(true);
                resultCanvas.SetActive(false);
                break;
            case ScreenType.Result:
                gamePlayCanvas.SetActive(false);
                resultCanvas.SetActive(true);
                break;
        }
    }
}
