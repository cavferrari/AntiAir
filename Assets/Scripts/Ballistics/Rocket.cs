using UnityEngine;

public class Rocket : Ballistics
{
    private TrailRenderer trailRenderer;
    private Transform poolParent;

    void Awake()
    {
        base.CustomAwake();
        trailRenderer = this.GetComponentInChildren<TrailRenderer>();
        poolParent = this.transform.parent;
    }

    public override void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        this.transform.parent = poolParent;
        base.Initialize(position, direction, plane);
        trailRenderer.enabled = true;
    }

    public override void Reset()
    {
        this.transform.parent = poolParent;
        trailRenderer.enabled = false;
        base.Reset();
    }

    protected override void Move()
    {
        base.Move();
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
    }

    protected override void Destroy(bool isCollision = false)
    {
        if (isCollision || currentPosition.y <= 0 || lifeTimer <= 0f)
        {
            trailRenderer.enabled = false;
        }
        base.Destroy(isCollision);
    }

    protected override void CustomStart()
    {
        base.CustomStart();
        trailRenderer.enabled = false;
    }
}
