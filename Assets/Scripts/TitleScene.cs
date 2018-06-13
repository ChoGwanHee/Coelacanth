using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {

    // 시작 버튼
    public void StartButton()
    {
        StartCoroutine(LoadLobbyScene());
    }

    private IEnumerator LoadLobbyScene()
    {
        // 게임씬을 완벽하게 로딩 후 씬을 변경한다
        AsyncOperation oper = SceneManager.LoadSceneAsync("PhotonLobby");

        yield return oper; // 로딩이 완료될때까지 대기 한다
    }

    // 종료 버튼
    public void ExitButton()
    {
        Application.Quit();
    }
}
