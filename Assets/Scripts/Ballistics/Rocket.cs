using UnityEngine;

public class Rocket : Ballistics
{
    protected override void Move()
    {
        base.Move();
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
    }
}
