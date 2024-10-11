using UnityEngine;

public class Ballistics : MonoBehaviour
{
    [System.Serializable]
    public class BallisticsEffect
    {
        public GameObject prefab;
        public bool alwaysCreate = true;
    }

    public enum BallisticsType
    {
        BULLET, ROCKET, BOMB
    }

    public BallisticsType ballisticsType = BallisticsType.BULLET;
    public float rateFire = 1f;
    public float distanceFromTargetTrigger = 200f;
    public float lifeTime = 3f;
    public BallisticsEffect impactExplosion;

    protected Vector3 currentPosition;
    protected Vector3 currentVelocity;
    protected Vector3 newPosition = Vector3.zero;
    protected Vector3 newVelocity = Vector3.zero;
    protected BallisticData ballisticData;
    protected float timer;
    protected float zPlane;
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

    void OnTriggerEnter(Collider other)
    {
        if (impactExplosion.prefab != null)
        {
            bool createEffect = impactExplosion.alwaysCreate ? true : Random.Range(0, 2) == 1 ? true : false;
            if (createEffect)
            {
                GameObject explosion = ObjectPooling.Instance.Get(impactExplosion.prefab.name + "Pool",
                                                                  this.transform.position,
                                                                  Quaternion.identity);
                explosion.GetComponent<FxEffect>().Play();
            }
        }
        Destroy();
    }

    public virtual void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        currentPosition = position;
        currentVelocity = direction * ballisticData.muzzleVelocity;
        zPlane = plane;
        timer = lifeTime;
        isInitialized = true;
    }

    public virtual void Reset()
    {
        timer = 0f;
        isInitialized = false;
        ObjectPooling.Instance.ReturnObject(this.gameObject);
    }

    protected virtual void Move()
    {
        IntegrationMethods.CurrentIntegrationMethod(Time.fixedDeltaTime, currentPosition, currentVelocity, ballisticData, out newPosition, out newVelocity);
        //CheckHit();
        currentPosition = newPosition;
        currentPosition.z = zPlane;
        currentVelocity = newVelocity;
        this.transform.position = currentPosition;
    }

    protected virtual void Destroy()
    {
        if (currentPosition.y <= 0 || timer <= 0f)
        {
            isInitialized = false;
            ObjectPooling.Instance.ReturnObject(this.gameObject);
        }
    }

    /* protected virtual void CheckHit()
    {
        Vector3 fireDirection = (newPosition - currentPosition).normalized;
        float fireDistance = Vector3.Distance(newPosition, currentPosition);
        RaycastHit hit;
        if (Physics.Raycast(currentPosition, fireDirection, out hit, fireDistance))
        {
            Debug.Log("Hit target!");
        }
    } */

    protected virtual void CustomStart()
    {
    }

    protected virtual void CustomAwake()
    {
        ballisticData = this.GetComponent<BallisticData>();
    }
}
