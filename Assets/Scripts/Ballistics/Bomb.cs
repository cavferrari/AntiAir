using UnityEngine;

public class Bomb : Ballistics
{
    private Rigidbody rb;
    private TrailRenderer trailRenderer;
    private Transform poolParent;

    void Awake()
    {
        poolParent = this.transform.parent;
    }

    void Start()
    {
        CustomStart();
    }

    public override void Initialize(Vector3 position, Vector3 velocity)
    {
        this.transform.parent = poolParent;
        currentPosition = this.transform.position;
        currentVelocity = Vector3.zero;
        rb.isKinematic = false;
        rb.AddForce(velocity * 2f);
        trailRenderer.enabled = true;
        isInitialized = true;
    }

    protected override void Move()
    {
        this.transform.Rotate(0.0f, 0.0f, 360f * Time.fixedDeltaTime);
        //CheckHit();
    }

    protected override void Destroy()
    {
        if (this.transform.position.y <= 0)
        {
            trailRenderer.enabled = false;
            rb.isKinematic = false;
            ObjectPooling.Instance.ReturnObject(this.gameObject);
            isInitialized = false;
        }
    }

    protected override void CustomStart()
    {
        base.CustomStart();
        rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        trailRenderer = this.GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
    }
}
