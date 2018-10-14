using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : Photon.PunBehaviour
{
    public Coroutine[] coroutines = new Coroutine[(int)BuffType.Max];
    public Buff[] buffs = new Buff[(int)BuffType.Max];
    public UtilItem[] referenceItem;

    /// <summary>
    /// 무적 이펙트
    /// </summary>
    public GameObject shiedEfx;

    /// <summary>
    /// 버프 이펙트
    /// </summary>
    public ParticleSystem upEfx;

    /// <summary>
    /// 이동속도 감소 이펙트
    /// </summary>
    public ParticleSystem slowEfx;

    public ParticleSystem buffEfx;
    public ParticleSystem debuffEfx;

    /// <summary>
    /// 핫소스 폭발 이펙트 레퍼런스
    /// </summary>
    public GameObject hotSauceBoomEfx_ref;

    public Renderer[] renderers;
    private Material[] playerMats;


    private PlayerController pc;
    public PlayerController PC
    {
        get { return pc; }
    }
    private PlayerStat stat;
    public PlayerStat Stat
    {
        get { return stat; }
    }


    private void Start()
    {
        pc = GetComponent<PlayerController>();
        stat = pc.Stat;
        int index = 0;
        playerMats = new Material[renderers.Length * 2];
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                playerMats[index++] = renderers[i].materials[j];
            }
        }


        /*buffs[0] = new BuffUnbeatable(referenceItem[0] as ItemStarFruit);
        buffs[1] = new BuffSlow();
        buffs[2] = new BuffHotSauce();
        buffs[3] = new BuffCocktail();*/
        
    }

    [PunRPC]
    public void ApplyBuff(int buffType)
    {
        switch (buffType)
        {
            case (int)BuffType.Unbeatable:
                buffEfx.Play(true);
                pc.isUnbeatable = true;
                shiedEfx.SetActive(true);
                break;
            case (int)BuffType.Slow:
                debuffEfx.Play(true);
                slowEfx.Play(true);
                break;
            case (int)BuffType.HotSauce:
                buffEfx.Play(true);
                StartCoroutine(HotSauceCount());
                break;
            case (int)BuffType.Cocktail:
                buffEfx.Play(true);
                upEfx.Play(true);
                break;
        }
    }

    [PunRPC]
    public void RemoveBuff(int buffType)
    {
        switch (buffType)
        {
            case (int)BuffType.Unbeatable:
                pc.isUnbeatable = false;
                shiedEfx.SetActive(false);
                break;
            case (int)BuffType.Slow:
                slowEfx.Stop(true);
                break;
            case (int)BuffType.HotSauce:
                Instantiate(hotSauceBoomEfx_ref, transform.position, Quaternion.identity);
                break;
            case (int)BuffType.Cocktail:
                upEfx.Stop(true);
                break;
        }
    }

    public void SetBuff(BuffType buffType, IEnumerator coroutine)
    {
        int index = (int)buffType;
        if(coroutines[index] != null)
        {
            StopCoroutine(coroutines[index]);
            coroutines[index] = null;
        }
        coroutines[index] = StartCoroutine(coroutine);
    }

    public void RemoveAllBuff()
    {
        StopAllCoroutines();
        for(int i=0; i<(int)BuffType.Max; i++)
        {
            if (i == (int)BuffType.HotSauce) continue;
            RemoveBuff(i);
        }

        Color color = new Color(1f, 1f, 1f);
        for (int i = 0; i < playerMats.Length; i++)
        {
            if (playerMats[i] == null) break;
            playerMats[i].SetColor("_Color", color);
        }

        pc.maxSpeedFactor = 1.0f;
        pc.Executer.damageFactor = 1.0f;
    }

    private IEnumerator HotSauceCount()
    {
        float duration = 5.0f;
        float elapsedTime = 0f;
        float curColorValue = 1.0f;
        float changeValue = 0.01f;
        Color color = new Color(1f,1f,1f);
        bool isDown = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            if (isDown)
            {
                curColorValue -= changeValue;
                if (curColorValue <= 0f)
                {
                    curColorValue = 0f;
                    isDown = false;
                }
            }
            else
            {
                curColorValue += changeValue;
                if (curColorValue >= 1f)
                {
                    curColorValue = 1f;
                    isDown = true;
                }
            }
            changeValue += 0.001f;
            color.g = curColorValue;
            color.b = curColorValue;

            SetCharacterColor(color);

            yield return null;
        }
        color.g = 1f;
        color.b = 1f;
        SetCharacterColor(color);
    }

    public void SetCharacterColor(Color color)
    {
        for (int i = 0; i < playerMats.Length; i++)
        {
            if (playerMats[i] == null) break;
            playerMats[i].SetColor("_Color", color);
        }
    }
}
