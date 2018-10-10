using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/StarFruit")]
public class ItemStarFruit : UtilItem
{
    public float duration;
    

    public override void Execute(BuffController bc)
    {
        bc.SetBuff(BuffType.Unbeatable, Process(bc));
    }

    private IEnumerator Process(BuffController bc)
    {
        float elapsedTime = 0f;

        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.Unbeatable);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bc.photonView.RPC("RemoveBuff", PhotonTargets.All, (int)BuffType.Unbeatable);
    }
}
