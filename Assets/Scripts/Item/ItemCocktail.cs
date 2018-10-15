using UnityEngine;

[CreateAssetMenu(menuName = "Item/Cocktail")]
public class ItemCocktail : UtilItem
{
    public float duration;
    public float addSpeed;
    public float addDamage;
    public int gainScore;


    public override void Execute(BuffController bc)
    {
        bc.Stat.AddScore(gainScore);
        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.Cocktail);
    }
}
