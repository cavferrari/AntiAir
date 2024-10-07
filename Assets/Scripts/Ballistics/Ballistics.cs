using UnityEngine;

public class Ballistics : MonoBehaviour
{
    public enum BallisticsType
    {
        BULLET, ROCKET, BOMB
    }

    public BallisticsType ballisticsType = BallisticsType.BULLET;
    public float rateFire = 1f;
    public float distanceFromTargetTrigger = 200f;
    public float lifeTime = 3f;

    protected Vector3 currentPosition;
    protected Vector3 currentVelocity;
    protected Vector3 newPosition = Vector3.zero;
    protected Vector3 newVelocity = Vector3.zero;
    protected BallisticData ballisticData;
    protected float timer;
    protected bool isInitialized = false;

    void Awake()
    {
        CustomAwake();
    }

    void Start()
    {
        CustomStart();
    }

    void Update()
    {
        if (isInitialized)
        {
            Destroy();
        }
    }

    void FixedUpdate()
    {
        if (isInitialized)
        {
            Move();
            if (timer > 0f)
            {
                timer -= Time.fixedDeltaTime;
            }
        }
    }

    public virtual void Initialize(Vector3 position, Vector3 direction)
    {
        currentPosition = position;
        currentVelocity = direction * ballisticData.muzzleVelocity;
        timer = lifeTime;
        isInitialized = true;
    }

    protected virtual void Move()
    {
        IntegrationMethods.CurrentIntegrationMethod(Time.fixedDeltaTime, currentPosition, currentVelocity, ballisticData, out newPosition, out newVelocity);
        //CheckHit();
        currentPosition = newPosition;
        currentVelocity = newVelocity;
        this.transform.position = currentPosition;
    }

    protected virtual void Destroy()
    {
        if (currentPosition.y <= 0 || timer <= 0f)
        {
            ObjectPooling.Instance.ReturnObject(this.gameObject);
            isInitialized = false;
        }
    }

    //Did we hit a target
    protected virtual void CheckHit()
    {
        Vector3 fireDirection = (newPosition - currentPosition).normalized;
        float fireDistance = Vector3.Distance(newPosition, currentPosition);
        RaycastHit hit;
        if (Physics.Raycast(currentPosition, fireDirection, out hit, fireDistance))
        {
            Debug.Log("Hit target!");
        }
    }

    protected virtual void CustomStart()
    {
    }

    protected virtual void CustomAwake()
    {
        ballisticData = this.GetComponent<BallisticData>();
    }
}
