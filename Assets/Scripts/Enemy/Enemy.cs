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

    private Path path;
    private EnemyFireControl enemyFireControl;
    private Quaternion targetRotation;
    private Vector3 direction;
    private Vector3 currentTargetPosition;
    private int indexCount;
    private bool isAttacking = false;
    private Vector3 up = Vector3.up;
    private GameObject[] targets;

    void Start()
    {
        path = this.GetComponent<Path>();
        enemyFireControl = this.GetComponent<EnemyFireControl>();
        targets = GameObject.FindGameObjectsWithTag("Target");
    }

    void Update()
    {
        if (path.GetPathCount() != 0 && indexCount < path.GetPathCount())
        {
            UpdatePitch();
            UpdateYaw();
            this.transform.position = Vector3.MoveTowards(this.transform.position, path.GetPathPoint(indexCount), speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path.GetPathPoint(indexCount)) < 0.001f)
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

    void FixedUpdate()
    {
        UpdateSpeed();
    }

    public Vector3 Velocity()
    {
        return (path.GetPathPoint(indexCount) - this.transform.position) * speed;
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
        if (!isAttacking && path.IsTurning(this.transform.position))
        {
            isAttacking = true;
            StartCoroutine(SpinRight(rotationPauseTime));
        }
        if (isAttacking && path.IsTurning(this.transform.position))
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
                if (this.transform.rotation.eulerAngles.z > 180f && this.transform.rotation.eulerAngles.z <= 273f)
                {
                    up = Vector3.down;
                }
                else if (this.transform.rotation.eulerAngles.z > 93f && this.transform.rotation.eulerAngles.z <= 180f)
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
}
