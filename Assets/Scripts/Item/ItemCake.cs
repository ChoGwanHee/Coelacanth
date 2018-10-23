using UnityEngine;


[CreateAssetMenu(menuName = "Item/Cake")]
public class ItemCake : UtilItem
{
    public int gainScore;

    public override void Execute(BuffController bc)
    {
        bc.Stat.AddScore(gainScore);
        bc.buffEfx.Play(true);
        FMODUnity.RuntimeManager.PlayOneShot(useSound);
    }
}
