using UnityEngine;

public class PoolVFXEffect : PoolEffect<ParticleSystem>
{
    private ParticleSystem.MainModule mainModule;

    public override void SetStartSize(float startSizeMin, float startSizeMax)
    {
        mainModule.startSize = new ParticleSystem.MinMaxCurve(startSizeMin, startSizeMax);
    }

    public override bool CustomDestroyCondition()
    {
        return !effect.IsAlive();
    }

    public override void CustomPlay()
    {
        effect.Play();
    }

    public override void CustomStop()
    {
        effect.Stop();
    }

    public override void CustomAwake()
    {
        base.CustomAwake();
        mainModule = effect.main;
    }
}