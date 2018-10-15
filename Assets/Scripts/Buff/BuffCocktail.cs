public class BuffCocktail : Buff
{
    private ItemCocktail item;


    public BuffCocktail(ItemCocktail item)
    {
        this.item = item;
        duration = item.duration;
    }

    protected override void OnStartBuff(BuffController bc)
    {
        bc.buffEfx.Play(true);
        bc.upEfx.Play(true);
        bc.PC.maxSpeedFactor += item.addSpeed;
        bc.PC.Executer.damageFactor += item.addDamage;
    }

    protected override void OnEndBuff(BuffController bc)
    {
        bc.onUpdateBuff -= OnUpdateBuff;
        bc.upEfx.Stop(true);
        bc.PC.maxSpeedFactor -= item.addSpeed;
        bc.PC.Executer.damageFactor -= item.addDamage;
    }
}
