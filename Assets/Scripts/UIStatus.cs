using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour {

    //public Image illust;
    //public Image ringImg;
    public Image weapon;
    public Transform countDisplay;
    public Sprite[] numbers;

    private bool infinity = true;

    

    public void ChangeWeapon(Firework newFirework)
    {
        SetWeaponImg(newFirework.uiSprite);
    }

    [Obsolete()]
    public void SetColor(Color color)
    {
        //ringImg.color = color;
    }

    public void SetWeaponImg(Sprite newSprite)
    {
        if(newSprite == null)
        {
            weapon.enabled = false;
        } else
        {
            weapon.enabled = true;
            weapon.sprite = newSprite;
        }
    }

    // 숫자에 맞게 스프라이트 교체, 0~999까지 가능
    public void SetCount(int count)
    {
        if (count > 999) count = 999;

        if (count == -1)
        {
            countDisplay.GetChild(0).GetComponent<Image>().sprite = numbers[10];
            countDisplay.GetChild(0).GetComponent<Image>().SetNativeSize();
            infinity = true;
            countDisplay.GetChild(0).gameObject.SetActive(true);
            for (int i=1;i<countDisplay.childCount; i++)
            {
                countDisplay.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            if (count < 0) count = 0;

            int[] num = new int[3];
            int maxIndex = 0;
            int tmpIndex = 0;

            // 각 자리수 숫자 계산해서 배열에 추가

            for (int i = count; i > 0;)
            {
                num[maxIndex++]= i % 10;
                i = Mathf.FloorToInt(i / 10);
            }

            if(maxIndex == 0)
            {
                maxIndex++;
                num[0] = 0;
            }

            // 스프라이트 교체
            for (int i = maxIndex - 1; i >= 0; i--)
            {
                countDisplay.GetChild(tmpIndex++).GetComponent<Image>().sprite = numbers[num[i]];
            }
            if (infinity)
            {
                countDisplay.GetChild(0).GetComponent<Image>().SetNativeSize();
                infinity = false;
            }

            // 자리수에 맞게 게임 오브젝트 켜고 끄기
            for (int i=1; i<3; i++)
            {
                countDisplay.GetChild(i).gameObject.SetActive(i < maxIndex);
            }
            
        }
    }

    
}
