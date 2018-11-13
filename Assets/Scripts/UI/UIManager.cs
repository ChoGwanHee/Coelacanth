using UnityEngine;
using System.Collections;


public class UIManager : MonoBehaviour {
    public static UIManager _instance;

    public enum ScreenType
    {
        GamePlay,
        Result
    }

    public bool UIControl
    {
        get { return uiControl; }
    }
    private bool uiControl = false;
    private int uiControlCount = 0;


    // Ready
    public GameObject readyInfo;

    // Game Play
    public GameObject gamePlayCanvas;
    public UIStatus userStatus;
    public UIButterflyCharging chargingUI;
    public UIScoreBoard scoreBoard;
    public UIAmmoCount ammoCounter;
    public UITimer timer;
    public UIEButton eButton;
    public UIRespawn respawnUI;
    public UIBuffInfo buffInfoUI;
    public UIItemUsingGauge itemUsingGauge;
    public Animator counterAnim;

    public GameObject escUI;
    public GameObject mouseHoldUI;
    public GameObject controlKeyInfoUI;

    // Result
    public GameObject resultCanvas;
    public UIResultScoreBoard resultScoreBoard;
    public UIResultEmotion resultEmotion;
    public GameObject exitButton;
    public float exitButtonAppearTime;

    // Chat
    public GameObject chatUI;
    private bool chatEnable = true;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            controlKeyInfoUI.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            controlKeyInfoUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            chatEnable = !chatEnable;
            chatUI.SetActive(chatEnable);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(escUI.activeSelf)
            {
                escUI.SetActive(false);
                TakeBackControl();
            }
            else
            {
                escUI.SetActive(true);
                TakeControl();
            }
        }
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
                StartCoroutine(AppearExitButton());
                break;
        }
    }

    public void TakeControl()
    {
        uiControl = true;
        uiControlCount++;
        if (uiControlCount == 1)
            GameManagerPhoton._instance.GetPlayerByOwnerId(PhotonNetwork.player.ID).PC.InitInput();
    }

    public void TakeBackControl()
    {
        if(uiControlCount >= 1)
            uiControlCount--;

        if (uiControlCount <= 0)
            uiControl = false;
    }
    
    private IEnumerator AppearExitButton()
    {
        yield return new WaitForSeconds(exitButtonAppearTime);
        exitButton.SetActive(true);
    }
}
