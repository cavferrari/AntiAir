using UnityEngine;

public class Rocket : Ballistics
{
    public override void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        base.Initialize(position, direction, plane);
        visualEffect.Play(0);
    }

    protected override void Move()
    {
        base.Move();
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
    }

    protected override void CustomUpdate()
    {
        if (isActive)
        {
            if (currentPosition.y <= 0 || lifeTimer <= 0f)
            {
                visualEffect.Stop(0);
                if (currentPosition.y <= 0)
                {
                    visualEffect.Play(1);
                }
                isActive = false;
                StartCoroutine(Destroy());
            }
        }
    }

    protected override void CustomOnTriggerEnter()
    {
        if (isActive)
        {
            visualEffect.Stop(0);
            visualEffect.Play(1);
            isActive = false;
            StartCoroutine(Destroy());
        }
    }
}
