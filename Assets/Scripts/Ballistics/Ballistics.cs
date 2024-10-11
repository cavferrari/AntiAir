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
    protected float lifeTimer;
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
            if (lifeTimer > 0f)
            {
                lifeTimer -= Time.fixedDeltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        CreateImpactEffect();
        Destroy(true);
    }

    public virtual void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        currentPosition = position;
        currentVelocity = direction * ballisticData.muzzleVelocity;
        zPlane = plane;
        lifeTimer = lifeTime;
        isInitialized = true;
    }

    public virtual void Reset()
    {
        lifeTimer = 0f;
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

    protected virtual void Destroy(bool isCollision = false)
    {
        if (isCollision || currentPosition.y <= 0 || lifeTimer <= 0f)
        {
            if (!isCollision && currentPosition.y <= 0)
            {
                CreateImpactEffect();
            }
            isInitialized = false;
            ObjectPooling.Instance.ReturnObject(this.gameObject);
        }
    }

    protected virtual void CreateImpactEffect()
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
