using UnityEngine;

public class Rocket : Ballistics
{
    private TrailRenderer trailRenderer;
    private Transform poolParent;

    void Awake()
    {
        base.CustomAwake();
        poolParent = this.transform.parent;
    }

    public override void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        this.transform.parent = poolParent;
        base.Initialize(position, direction, plane);
        trailRenderer.enabled = true;
    }

    protected override void Move()
    {
        base.Move();
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
    }

    protected override void Destroy()
    {
        if (currentPosition.y <= 0 || timer <= 0f)
        {
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
