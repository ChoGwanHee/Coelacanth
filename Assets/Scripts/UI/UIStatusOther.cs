using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusOther : MonoBehaviour
{

    public Image illust;
    public Image backgroundImg;
    public Image[] hearts;

    public void SetColor(Color color)
    {
        backgroundImg.color = color;
    }

    public void SetImage(Sprite newSprite)
    {
        illust.sprite = newSprite;
    }

    public void SetHeart(int heartLeft)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i <= heartLeft - 1)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

}
