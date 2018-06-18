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

    void Awake()
    {
    }

    // 닉네임 글자 수 제한 설정
    void Start()
    {
        PlayButton.GetComponentInChildren<InputField>().characterLimit = 16;
    }

    // 닉네임 설정 후, 게임 시작
    public void PlayGame()
    {
        if (PlayButton.GetComponentInChildren<InputField>().text.Length <= 0)
        {
            Debug.Log(PlayButton.GetComponentInChildren<InputField>().text.Length + " : 닉네임 입력 필요");
        }
        else
        {
            if (NickNameRegexCheck(PlayButton.GetComponentInChildren<InputField>().text) == true)
            {
                InstanceValue.Nickname = PlayButton.GetComponentInChildren<InputField>().text;
                Debug.Log("나의 닉네임 : " + InstanceValue.Nickname);
                LobbyManager.Lobby.AvailablePhoton();
            }
            else
            {
                Debug.Log("특수문자 사용 불가");
            }
        }
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