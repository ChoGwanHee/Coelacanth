using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/HotSauce")]
public class ItemHotSauce : UtilItem
{
    public float damage;
    public float duration;
    public float addSpeed;
    public int gainScore;
    public float hitForce;
    public float hitRadius;


    public override void Execute(BuffController bc)
    {
        bc.Stat.AddScore(gainScore);
        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.HotSauce);
        FMODUnity.RuntimeManager.PlayOneShot(useSound);
        UIManager._instance.buffInfoUI.DisplayBuffInfo(3, bc.transform);
    }
}
