using UnityEngine;

public class SoundEffect : Effect<AudioSource>
{
    public override bool CustomDestroyCondition()
    {
        return !effect.isPlaying;
    }

    public override void CustomPlay()
    {
        effect.Play();
    }

    public override void CustomStart()
    {
        effect.maxDistance = GameManager.Instance.HorizontalBorderRight();
    }

    public override void CustomAwake()
    {
        base.CustomAwake();
    }
}
