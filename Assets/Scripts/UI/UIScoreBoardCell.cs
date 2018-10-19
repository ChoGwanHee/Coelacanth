using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreBoardCell : MonoBehaviour {

    public bool isMine = false;

    public float moveTime = 0.5f;
    private float elapsedTime = 0;
    private bool isMoving = false;

    private Vector3 targetPosition;

    private int score = 0;
    public int Score
    {
        get { return score; }
    }
    private int preScore = 0;
    private int rank = 1;
    public int Rank
    {
        get { return rank; }

        set
        {
            if (value != rank)
            {
                rank = value;
            }
        }
    }

    public Text nicknameText;
    public Text scoreText;
    public Text readyText;

    private Image picture;

    private PlayerStat refStat;
    public bool IsNotRegist
    {
        get { return refStat == null; }
    }
    private PhotonPlayer refPlayer;
    //private UIScoreBoard scoreBoard;


    private void Start()
    {
        //scoreBoard = transform.parent.parent.GetComponent<UIScoreBoard>();
        picture = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, elapsedTime / moveTime);

            if(elapsedTime / moveTime >= 1)
            {
                isMoving = false;
            }
            elapsedTime += Time.deltaTime;
        }

        UpdateScore();
    }

    public void SetNickname(string newNick)
    {
        nicknameText.text = newNick;
    }

    public void SetScore(int newScore)
    {
        if (preScore == newScore) return;

        preScore = score;
        score = newScore;
        scoreText.text = newScore.ToString();
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
        elapsedTime = 0;
        isMoving = true;
    }

    public void SetPlayerStat(PlayerStat stat)
    {
        refStat = stat;
        refPlayer = refStat.gameObject.GetPhotonView().owner;
        if(nicknameText != null)
            nicknameText.text = stat.nickname;
        refStat.onReadyChanged += ReadyChange;
    }

    public void SetPlayerPicture(Sprite newPicture)
    {
        picture.sprite = newPicture;
    }

    public void UpdateScore()
    {
        if (refStat != null)
        {
            SetScore(refPlayer.GetScore());
        }
    }

    private void ReadyChange(bool isReady)
    {
        readyText.gameObject.SetActive(isReady);
    }
}
