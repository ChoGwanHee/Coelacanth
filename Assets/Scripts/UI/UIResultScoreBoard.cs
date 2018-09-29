using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResultScoreBoard : MonoBehaviour {

    public Text[] ids;
    public Text[] scores;

    public GameObject highlight;

    private void SetIdScore(int index, string id, int score)
    {
        ids[index].text = id;
        scores[index].text = score.ToString();
    }

    public void CalcResult()
    {
        PlayerStat[] stats = GameManagerPhoton._instance.playerList.ToArray();
        PlayerStat tmpStat;

        // 삽입 정렬
        for(int i=1; i<stats.Length; i++)
        {
            tmpStat = stats[i];
            int j = i;

            while (j > 0&& stats[j-1].Score < tmpStat.Score)
            {
                stats[j] = stats[j - 1];
                j--;
            }
            stats[j] = tmpStat;
        }

        // 닉네임과 점수 표시
        for (int i = 0; i < stats.Length; i++)
        {
            // 자기 자신이면 하이라이트로 표시
            if (stats[i].photonView.isMine)
            {
                highlight.transform.localPosition = new Vector3(17, -51 - (58 * i));
            }

            SetIdScore(i, stats[i].nickname, stats[i].Score);
        }
    }
}
