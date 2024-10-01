using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    private Vector3 currentPosition;
    private Vector3 currentVelocity;
    private Vector3 newPosition = Vector3.zero;
    private Vector3 newVelocity = Vector3.zero;
    private BulletData bulletData;
    private float timer;

    void Start()
    {
        bulletData = GetComponent<BulletData>();
    }

    void Update()
    {
        DestroyBullet();
    }

    void FixedUpdate()
    {
        MoveBullet();
        if (timer > 0f)
        {
            timer -= Time.fixedDeltaTime;
        }
    }

    public void Initialize(Vector3 position, Vector3 velocity)
    {
        currentPosition = position;
        currentVelocity = velocity;
        timer = lifeTime;
    }

    void MoveBullet()
    {
        //Use an integration method to calculate the new position of the bullet
        IntegrationMethods.CurrentIntegrationMethod(Time.fixedDeltaTime, currentPosition, currentVelocity, bulletData, out newPosition, out newVelocity);
        //First we need these coordinates to check if we have hit something
        //CheckHit();
        currentPosition = newPosition;
        currentVelocity = newVelocity;
        //Add the new position to the bullet
        transform.position = currentPosition;
    }

    void DestroyBullet()
    {
        if (currentPosition.y <= 0 || timer <= 0f)
        {
            ObjectPooling.Instance.ReturnObject(this.gameObject);
        }
    }

    //Did we hit a target
    void CheckHit()
    {
        Vector3 fireDirection = (newPosition - currentPosition).normalized;
        float fireDistance = Vector3.Distance(newPosition, currentPosition);

        RaycastHit hit;

        if (Physics.Raycast(currentPosition, fireDirection, out hit, fireDistance))
        {
            Debug.Log("Hit target!");
        }
    }
}
