using System.Collections;
using System.Collections.Generic;
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
        bc.SetBuff(BuffType.Cocktail, Process(bc));
    }

    private IEnumerator Process(BuffController bc)
    {
        float elapsedTime = 0f;

        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.Cocktail);
        bc.PC.maxSpeedFactor += addSpeed;
        bc.GetComponent<FireworkExecuter>().damageFactor += addDamage;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bc.PC.maxSpeedFactor -= addSpeed;
        bc.GetComponent<FireworkExecuter>().damageFactor -= addDamage;
        bc.photonView.RPC("RemoveBuff", PhotonTargets.All, (int)BuffType.Cocktail);
    }
}
