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

    protected override void Destroy(bool isCollision = false)
    {
        if (isCollision || currentPosition.y <= 0)
        {
            if (!isCollision && currentPosition.y <= 0)
            {
                CreateImpactEffect();
            }
            rb.isKinematic = true;
            trailRenderer.enabled = false;
            isInitialized = false;
            ObjectPooling.Instance.ReturnObject(this.gameObject);
        }
    }
}