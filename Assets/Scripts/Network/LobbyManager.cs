using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject NetworkManager;

    private static LobbyManager _lobby;
    public static LobbyManager Lobby
    {
        get { return _lobby; }
        set { _lobby = value; }
    }

    void Start()
    {
        NetworkManager.SetActive(false);
        Lobby = GetComponent<LobbyManager>();
        //Debug.Log("Lobby Object Disabled");
    }

    public void AvailablePhoton()
    {
        //Debug.Log("Lobby object Availabled");
        NetworkManager.SetActive(true);
        Thread.Sleep(1000);
        SceneManager.LoadScene(1);
    }
}