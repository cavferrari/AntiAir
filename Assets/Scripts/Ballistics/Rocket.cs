using UnityEngine;

public class Rocket : Ballistics
{
    public override void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        this.transform.parent = poolParent;
        base.Initialize(position, direction, plane);
    }

    protected override void Move()
    {
        base.Move();
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
    }
}
