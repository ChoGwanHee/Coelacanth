using UnityEngine;

public class BuffController : Photon.PunBehaviour
{
    public Buff[] buffs = new Buff[(int)BuffType.Max];
    

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

    /// <summary>
    /// 좋은 효과 아이템 섭취 시 이펙트
    /// </summary>
    public ParticleSystem buffEfx;

    /// <summary>
    /// 나쁜 효과 아이템 섭취 시 이펙트
    /// </summary>
    public ParticleSystem debuffEfx;

    /// <summary>
    /// 핫소스 폭발 이펙트
    /// </summary>
    public GameObject hotSauceBoomEfx_ref;

    [FMODUnity.EventRef]
    public string hotSauceCountSound;

    [FMODUnity.EventRef]
    public string hotSauceBoomSound;

    [FMODUnity.EventRef]
    public string starfruitSound;

    public FMOD.Studio.EventInstance buffDuringSound;


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


    public delegate void OnUpdateBuffDelegate(BuffController bc);
    public OnUpdateBuffDelegate onUpdateBuff;


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

        UtilItem[] reference = GameManagerPhoton._instance.itemManager.buffUtilItemReference;

        buffs[0] = new BuffUnbeatable(reference[0] as ItemStarFruit);
        buffs[1] = new BuffSlow(reference[1] as ItemGel);
        buffs[2] = new BuffHotSauce(reference[2] as ItemHotSauce);
        buffs[3] = new BuffCocktail(reference[3] as ItemCocktail);
    }

    private void Update()
    {
        if(onUpdateBuff != null)
        {
            onUpdateBuff(this);
        }
    }

    [PunRPC]
    public void ApplyBuff(int buffType)
    {
        buffs[buffType].StartBuff(this);
    }

    [PunRPC]
    public void RemoveBuff(int buffType)
    {
        buffs[buffType].StopBuff(this);
    }

    public void RemoveAllBuff()
    {
        for(int i=0; i< buffs.Length; i++)
        {
            RemoveBuff(i);
        }
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
