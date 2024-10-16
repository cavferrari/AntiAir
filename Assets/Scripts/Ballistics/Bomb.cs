using UnityEngine;

public class Bomb : Ballistics
{
    protected override void Move()
    {
        currentPosition = this.transform.position;
        currentPosition.z = zPlane;
        this.transform.position = currentPosition;
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
        //CheckHit();
    }
}