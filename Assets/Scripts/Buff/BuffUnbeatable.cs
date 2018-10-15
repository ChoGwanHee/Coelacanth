public class BuffUnbeatable : Buff
{

    public BuffUnbeatable(ItemStarFruit item)
    {
        duration = item.duration;
    }

    protected override void OnStartBuff(BuffController bc)
    {
        bc.buffEfx.Play(true);
        bc.PC.isUnbeatable = true;
        bc.shiedEfx.SetActive(true);
    }

    protected override void OnEndBuff(BuffController bc)
    {
        bc.onUpdateBuff -= OnUpdateBuff;
        bc.PC.isUnbeatable = false;
        bc.shiedEfx.SetActive(false);
    }
    
}
