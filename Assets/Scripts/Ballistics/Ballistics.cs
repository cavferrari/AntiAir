using System.Collections;
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

    protected Rigidbody rb;
    protected MeshRenderer meshRenderer;
    protected TrailRenderer trailRenderer;
    protected VFXEffect visualEffect;
    protected Transform poolParent;
    protected Vector3 currentPosition;
    protected Vector3 currentVelocity;
    protected Vector3 newPosition = Vector3.zero;
    protected Vector3 newVelocity = Vector3.zero;
    protected BallisticData ballisticData;
    protected float lifeTimer;
    protected float zPlane;
    protected bool isActive = false;

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
        CustomUpdate();
    }

    void FixedUpdate()
    {
        if (isActive)
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
        CustomOnTriggerEnter();
    }

    public virtual void Initialize(Vector3 position, Vector3 direction, float plane)
    {
        this.transform.parent = poolParent;
        currentPosition = position;
        currentVelocity = direction * ballisticData.muzzleVelocity;
        zPlane = plane;
        lifeTimer = lifeTime;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(currentVelocity);
        }
        if (trailRenderer) trailRenderer.enabled = true;
        isActive = true;
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

    protected virtual IEnumerator Destroy()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        meshRenderer.enabled = false;
        if (trailRenderer) trailRenderer.enabled = false;
        while (visualEffect.IsAlive())
        {
            yield return null;
        }
        meshRenderer.enabled = true;
        ObjectPooling.Instance.ReturnObject(this.gameObject);
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

    protected virtual void CustomUpdate()
    {
        if (isActive)
        {
            if (currentPosition.y <= 0 || lifeTimer <= 0f)
            {
                if (currentPosition.y <= 0)
                {
                    visualEffect.Play();
                }
                isActive = false;
                StartCoroutine(Destroy());
            }
        }
    }

    protected virtual void CustomOnTriggerEnter()
    {
        if (isActive)
        {
            visualEffect.Play();
            isActive = false;
            StartCoroutine(Destroy());
        }
    }

    protected virtual void CustomStart()
    {
    }

    protected virtual void CustomAwake()
    {
        rb = this.GetComponent<Rigidbody>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        trailRenderer = this.GetComponentInChildren<TrailRenderer>();
        visualEffect = this.GetComponentInChildren<VFXEffect>();
        poolParent = this.transform.parent;
        ballisticData = this.GetComponent<BallisticData>();
        if (rb != null) rb.isKinematic = true;
        if (trailRenderer) trailRenderer.enabled = false;
    }
}