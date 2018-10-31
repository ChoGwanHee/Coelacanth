using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerModule;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MenuManager : MonoBehaviour
{
    public GameObject PlayButton;
    public InputField nicknameField;

    void Awake()
    {
    }

    // 닉네임 글자 수 제한 설정
    void Start()
    {
        nicknameField.characterLimit = 8;
    }

    // 닉네임 설정 후, 게임 시작
    public void PlayGame()
    {
        if (nicknameField.text.Length <= 0)
        {
            Debug.Log(nicknameField.text.Length + " : 닉네임 입력 필요");
        }
        else
        {
            if (NickNameRegexCheck(nicknameField.text) == true)
            {
                PlayButton.GetComponent<Button>().interactable = false;
                nicknameField.readOnly = true;
                InstanceValue.Nickname = nicknameField.text;
                int ownerID = Random.Range(0, 20000);
                InstanceValue.ID = ownerID;
                LobbyManager.Lobby.AvailablePhoton();
            }
            else
            {
                Debug.Log("특수문자 사용 불가");
            }
        }
    }

    public bool NickNameCheck()
    {


        return false;
    }

    // 특수문자 검출하는 정규표현식
    public bool NickNameRegexCheck(string _text)
    {
        string Pattern = @"^[a-zA-Z0-9가-힣]*$";
        return Regex.IsMatch(_text, Pattern);
    }

    // 게임 종료
    public void QuitGame()
    {
        Debug.Log("Scene Quit");
        Application.Quit();
    }
}