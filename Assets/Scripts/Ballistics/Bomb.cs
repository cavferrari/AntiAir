using UnityEngine;

public class Bomb : Ballistics
{
    private Rigidbody rb;
    private TrailRenderer trailRenderer;
    private Transform poolParent;

    void Awake()
    {
        base.CustomAwake();
        poolParent = this.transform.parent;
        trailRenderer = this.GetComponentInChildren<TrailRenderer>();
        rb = this.GetComponent<Rigidbody>();
    }

    void Start()
    {
        CustomStart();
    }

    public override void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        this.transform.parent = poolParent;
        currentPosition = this.transform.position;
        currentVelocity = Vector3.zero;
        zPlane = plane;
        rb.isKinematic = false;
        rb.AddForce(direction * ballisticData.muzzleVelocity);
        trailRenderer.enabled = true;
        isInitialized = true;
    }

    public override void Reset()
    {
        this.transform.parent = poolParent;
        rb.isKinematic = true;
        trailRenderer.enabled = false;
        base.Reset();
    }

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
        if (isCollision || this.transform.position.y <= 0)
        {
            if (!isCollision && currentPosition.y <= 0)
            {
                CreateImpactEffect();
            }
            trailRenderer.enabled = false;
            rb.isKinematic = false;
            ObjectPooling.Instance.ReturnObject(this.gameObject);
            isInitialized = false;
        }
    }

    protected override void CustomStart()
    {
        base.CustomStart();
        rb.isKinematic = true;
        trailRenderer.enabled = false;
    }
}
