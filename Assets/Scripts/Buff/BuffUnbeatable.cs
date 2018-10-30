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

        bc.buffDuringSound = FMODUnity.RuntimeManager.CreateInstance(bc.starfruitSound);
        bc.buffDuringSound.start();
    }

    protected override void OnEndBuff(BuffController bc)
    {
        bc.onUpdateBuff -= OnUpdateBuff;
        bc.PC.isUnbeatable = false;
        bc.shiedEfx.SetActive(false);

        bc.buffDuringSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    
}
