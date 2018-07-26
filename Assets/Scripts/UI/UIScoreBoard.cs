using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreBoard : MonoBehaviour {

    public Image rankImg;

    public Sprite[] rankRefs;

    public UIScoreBoardCell[] cells;

    public float cellSpacing = 19f;

    public float scoreUpdateTime = 0.3f;
    private float lastUpdateElapsedTime = 0;


    private void Update()
    {
        UpdateScores();
    }

    public void SetMyRankImg(int num)
    {
        if (num < 0 || num > rankRefs.Length) return;

        rankImg.sprite = rankRefs[num-1];
    }

    private void CalcRanks()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            int curRank = 1;

            for (int j=0; j<cells.Length; j++)
            {
                if (i == j) continue;

                if(cells[i].Score < cells[j].Score)
                {
                    curRank++;
                }
            }

            cells[i].Rank = curRank;

            if(cells[i].isMine)
            {
                SetMyRankImg(curRank);
            }
        }
    }

    private void SortingCells()
    {
        UIScoreBoardCell tmpCell;

        for(int i=1; i<cells.Length; i++)
        {
            tmpCell = cells[i];
            int j = i;

            while(j > 0 && cells[j-1].Score < tmpCell.Score)
            {
                cells[j] = cells[j - 1];
                j--;
            }
            cells[j] = tmpCell;
        }


        float curY = 0;

        for (int i=0; i<cells.Length; i++)
        {
            float cellHeight = cells[i].GetComponent<RectTransform>().sizeDelta.y + cellSpacing;
            
            cells[i].SetTargetPosition(new Vector3(0, curY));
            curY -= cellHeight;
        }
    }

    private void UpdateScores()
    {
        lastUpdateElapsedTime += Time.deltaTime;

        if(lastUpdateElapsedTime >= scoreUpdateTime)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].UpdateScore();
            }

            SortingCells();
            CalcRanks();

            lastUpdateElapsedTime = 0;
        }
    }

}
