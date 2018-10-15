public class BuffSlow : Buff
{
    private ItemGel item;


    public BuffSlow(ItemGel item)
    {
        this.item = item;
        duration = item.duration;
    }

    protected override void OnStartBuff(BuffController bc)
    {
        bc.debuffEfx.Play(true);
        bc.slowEfx.Play(true);
        bc.PC.maxSpeedFactor -= item.subSpeed;
    }

    protected override void OnEndBuff(BuffController bc)
    {
        bc.onUpdateBuff -= OnUpdateBuff;
        bc.slowEfx.Stop(true);
        bc.PC.maxSpeedFactor += item.subSpeed;
    }
}
