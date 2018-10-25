using UnityEngine;

[CreateAssetMenu(menuName = "Item/StarFruit")]
public class ItemStarFruit : UtilItem
{
    public float duration;
    

    public override void Execute(BuffController bc)
    {
        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.Unbeatable);
        FMODUnity.RuntimeManager.PlayOneShot(useSound);
        UIManager._instance.buffInfoUI.DisplayBuffInfo(1, bc.transform);
    }
}
