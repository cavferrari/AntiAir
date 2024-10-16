using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxSpeed = 30f;
    public float minSpeed = 20f;
    public float gravityAcceleration = 0.1f;
    public float gravityDeceleration = 0.2f;
    public float rotationSpeed = 50f;
    public float rollEntryDistance = 20f;
    public float rollEndDistance = 20f;
    public float rotationPauseTime = 2f;
    public float speed;
    public float zAxisRotation;

    private Rigidbody rb;
    protected MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;
    protected VFXEffect visualEffect;
    private Path path;
    private EnemyFireControl enemyFireControl;
    private Quaternion targetRotation;
    private Vector3 direction;
    private Vector3 currentTargetPosition;
    private int indexCount;
    private Vector3 up = Vector3.up;
    private GameObject[] targets;
    private bool isAttacking = false;
    private bool isActive = false;
    private bool isDestroyed = false;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        trailRenderer = this.GetComponentInChildren<TrailRenderer>();
        visualEffect = this.GetComponentInChildren<VFXEffect>();
        path = this.GetComponent<Path>();
        enemyFireControl = this.GetComponent<EnemyFireControl>();
    }

    void Update()
    {
        if (isActive && !isDestroyed)
        {
            if (path.GetPathCount() != 0 && indexCount < path.GetPathCount())
            {
                UpdatePitch();
                UpdateYaw();
                this.transform.position = Vector3.MoveTowards(this.transform.position, path.GetPathPoint(indexCount), speed * Time.deltaTime);
                if (Vector3.Distance(this.transform.position, path.GetPathPoint(indexCount)) < 0.001f)
                {
                    this.transform.position = path.GetPathPoint(indexCount);
                    indexCount += 1;
                }
                UpdateFire();
            }
            else
            {
                currentTargetPosition = GetNewTarget();
                path.GeneratePath(this.transform.position, currentTargetPosition, rollEntryDistance, rollEndDistance);
                indexCount = 0;
            }
            zAxisRotation = this.transform.rotation.eulerAngles.z;
        }
        if (Input.GetMouseButtonDown(1))
        {
            isActive = false;
            isDestroyed = true;
            StartCoroutine(Destroy());
        }
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            UpdateSpeed();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive && !isDestroyed)
        {
            isActive = false;
            isDestroyed = true;
            StartCoroutine(Destroy());
        }
    }

    public void Initialize()
    {
        path.Reset();
        enemyFireControl.Reset();
        speed = maxSpeed;
        currentTargetPosition = Vector3.zero;
        indexCount = 0;
        isAttacking = false;
        trailRenderer.enabled = true;
        rb.isKinematic = true;
        meshRenderer.enabled = true;
        targets = GameObject.FindGameObjectsWithTag("Target");
        isDestroyed = false;
        isActive = true;
    }

    public Vector3 Velocity()
    {
        return (currentTargetPosition - this.transform.position).normalized * speed;
    }

    private void UpdatePitch()
    {
        direction = path.GetPathPoint(indexCount) - this.transform.position;
        if (direction != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(direction, up);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void UpdateYaw()
    {
        if (path.IsTurningDown(this.transform.position))
        {
            isAttacking = true;
            StartCoroutine(SpinRight(rotationPauseTime));
        }
        if (path.IsTurningUp(this.transform.position))
        {
            isAttacking = false;
            StartCoroutine(SpinLeft(rotationPauseTime));
        }
    }

    private void UpdateSpeed()
    {
        if (this.transform.rotation.eulerAngles.x == 0)
        {
            if (speed < maxSpeed)
            {
                speed += gravityAcceleration;
            }
        }
        else if (this.transform.rotation.eulerAngles.x < 270f)
        {
            speed += gravityAcceleration;
        }
        else
        {
            if (speed > minSpeed)
            {
                speed -= gravityDeceleration;
            }
        }
    }

    private void UpdateFire()
    {
        if (isAttacking && !enemyFireControl.IsFiring())
        {
            if (Vector3.Distance(this.transform.position, currentTargetPosition) < enemyFireControl.DistanceFromTargetTrigger())
            {
                enemyFireControl.SetFire(true);
            }
        }
        else if (!isAttacking && enemyFireControl.IsFiring())
        {
            enemyFireControl.SetFire(false);
        }
    }

    private IEnumerator Yaw(float time)
    {
        if (isAttacking)
        {
            up = Vector3.back;
        }
        else
        {
            up = Vector3.forward;
        }
        yield return new WaitForSeconds(time);
        up = Vector3.up;
    }

    private IEnumerator SpinRight(float waitTime, float time = 1000f)
    {
        float elapsedTime = 0f;
        up = Vector3.back;
        yield return new WaitForSeconds(waitTime);
        while (elapsedTime < time)
        {
            if (path.GetOrientation() == 1)
            {
                if (this.transform.rotation.eulerAngles.z > 180f && this.transform.rotation.eulerAngles.z <= 270f)
                {
                    up = Vector3.down;
                }
                else if (this.transform.rotation.eulerAngles.z > 90f && this.transform.rotation.eulerAngles.z <= 180f)
                {
                    up = Vector3.forward;
                }
                else if (this.transform.rotation.eulerAngles.z > 0f && this.transform.rotation.eulerAngles.z <= 90f)
                {
                    up = Vector3.up;
                    elapsedTime = 1000f;
                }
            }
            else
            {
                if (this.transform.rotation.eulerAngles.z > 0f && this.transform.rotation.eulerAngles.z <= 90f)
                {
                    up = Vector3.down;
                }
                else if (this.transform.rotation.eulerAngles.z > 90f && this.transform.rotation.eulerAngles.z <= 180f)
                {
                    up = Vector3.forward;
                }
                else if (this.transform.rotation.eulerAngles.z > 180f && this.transform.rotation.eulerAngles.z <= 360f)
                {
                    up = Vector3.up;
                    elapsedTime = 1000f;
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (up != Vector3.up)
        {
            up = Vector3.up;
        }
    }

    private IEnumerator SpinLeft(float waitTime, float time = 1000f)
    {
        float elapsedTime = 0f;
        up = Vector3.forward;
        yield return new WaitForSeconds(waitTime);
        while (elapsedTime < time)
        {
            if (path.GetOrientation() == 1)
            {
                if (this.transform.rotation.eulerAngles.z > 0f && this.transform.rotation.eulerAngles.z <= 90f)
                {
                    up = Vector3.down;
                }
                else if (this.transform.rotation.eulerAngles.z > 90f && this.transform.rotation.eulerAngles.z <= 180f)
                {
                    up = Vector3.back;
                }
                else if (this.transform.rotation.eulerAngles.z > 180f && this.transform.rotation.eulerAngles.z <= 360f)
                {
                    up = Vector3.up;
                    elapsedTime = 1000f;
                }
            }
            else
            {
                if (this.transform.rotation.eulerAngles.z > 180f && this.transform.rotation.eulerAngles.z <= 275f)
                {
                    up = Vector3.down;
                }
                else if (this.transform.rotation.eulerAngles.z > 95f && this.transform.rotation.eulerAngles.z <= 180f)
                {
                    up = Vector3.back;
                }
                else if (this.transform.rotation.eulerAngles.z > 0f && this.transform.rotation.eulerAngles.z <= 93f)
                {
                    up = Vector3.up;
                    elapsedTime = 1000f;
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (up != Vector3.up)
        {
            up = Vector3.up;
        }
    }

    private Vector3 GetNewTarget()
    {
        return targets[Random.Range(0, targets.Length - 1)].transform.position;
    }

    private IEnumerator Destroy()
    {
        visualEffect.Play(0);
        enemyFireControl.SetFire(false);
        rb.isKinematic = false;
        trailRenderer.enabled = false;
        rb.AddForce(4f * speed * (path.GetPathPoint(indexCount) - this.transform.position).normalized);
        float elapsedTime = 0f;
        float time = 10f;
        float xAxis = Random.Range(0f, 15f);
        float yAxis = Random.Range(0f, 15f);
        float zAxis = Random.Range(0f, 15f);
        while (this.transform.position.y > 0f && elapsedTime < time)
        {
            this.transform.Rotate(xAxis * Time.fixedDeltaTime, yAxis * Time.fixedDeltaTime, zAxis * Time.fixedDeltaTime);
            yield return null;
        }
        visualEffect.Stop(0);
        rb.isKinematic = true;
        meshRenderer.enabled = false;
        visualEffect.Play(1);
        GameManager.Instance.CreatePostExplosionSmoke(this.transform.position);
        while (visualEffect.IsActive())
        {
            yield return null;
        }
        ObjectPooling.Instance.ReturnObject(this.gameObject);
    }
}
