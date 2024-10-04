using UnityEngine;

public class Rocket : Ballistics
{
    private TrailRenderer trailRenderer;
    private Transform poolParent;

    void Awake()
    {
        poolParent = this.transform.parent;
    }

    public override void Initialize(Vector3 position, Vector3 velocity)
    {
        base.Initialize(position, velocity);
        trailRenderer.enabled = true;
    }

    protected override void Destroy()
    {
        if (currentPosition.y <= 0 || timer <= 0f)
        {
            this.transform.parent = poolParent;
            trailRenderer.enabled = false;
        }
        base.Destroy();
    }

    protected override void CustomStart()
    {
        base.CustomStart();
        trailRenderer = this.GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
    }
}
