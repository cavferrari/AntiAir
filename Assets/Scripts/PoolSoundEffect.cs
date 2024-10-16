using System;
using UnityEngine;

public class PoolSoundEffect : PoolEffect<AudioSource>
{
    public override bool CustomDestroyCondition()
    {
        return !effect.isPlaying;
    }

    public override void CustomPlay()
    {
        float distance = Math.Clamp(Vector3.Distance(Vector3.zero, this.transform.position), 0, GameManager.Instance.HorizontalBorderRight());
        float percentage = Math.Abs(GameManager.Instance.HorizontalBorderRight() - distance) / GameManager.Instance.HorizontalBorderRight();
        effect.volume *= percentage;
        effect.Play();
    }

    public override void CustomAwake()
    {
        base.CustomAwake();
    }
}
