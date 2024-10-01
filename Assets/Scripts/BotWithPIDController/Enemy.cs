using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform xAxis;
    public Transform zAxis;
    public Transform input;
    public Transform aircraftTransform;

    public float powerVertical = 100f;
    public float powerHorizontal = 50f;
    public float pitchSpeed = 1f;
    public float yawSpeed = 1f;
    public float yawPrecision = 7f;

    private Rigidbody aircraftRigidBody;
    private PidController pidController;
    private EnemyInput enemyInput;
    private Vector3 inputPosition;
    private Vector3 direction;
    private bool isAttacking = false;

    void Start()
    {
        aircraftRigidBody = aircraftTransform.GetComponent<Rigidbody>();
        pidController = this.GetComponent<PidController>();
        enemyInput = this.GetComponentInChildren<EnemyInput>();
    }

    void Update()
    {
        UpdateYaw();
        UpdatePitch();
    }

    void FixedUpdate()
    {
        inputPosition = input.position;
        float throttleVerctical = pidController.CalculateVertical(Time.fixedDeltaTime, aircraftRigidBody.position.y, inputPosition.y);
        float throttleHorizontal = pidController.CalculateHorizontal(Time.fixedDeltaTime, aircraftRigidBody.position.x, inputPosition.x);
        aircraftRigidBody.AddForce(new Vector3(throttleHorizontal * powerHorizontal, throttleVerctical * powerVertical, 0f));
    }

    private void UpdateYaw()
    {
        if (!isAttacking && Math.Abs(enemyInput.GetEntryRunPosition().x - aircraftTransform.position.x) < 13f)
        {
            isAttacking = true;
            StartCoroutine(Yaw(180.1f, Vector3.right));
        }
        else if (isAttacking && aircraftTransform.position.x > enemyInput.GetEndRunPosition().x)
        {
            isAttacking = false;
            StartCoroutine(Yaw(0f, Vector3.left));
        }
    }

    private void UpdatePitch()
    {
        direction = inputPosition - aircraftTransform.position;
        float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angleZ != 0f)
        {
            zAxis.rotation = Quaternion.Slerp(zAxis.rotation, Quaternion.Euler(0f, 0f, angleZ), Time.deltaTime * pitchSpeed);
        }
    }

    IEnumerator Yaw(float angle, Vector3 axis)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        while (Mathf.Abs(xAxis.rotation.eulerAngles.x - rotation.eulerAngles.x) > yawPrecision)
        //while (Mathf.Abs(this.transform.rotation.eulerAngles.x - rotation.eulerAngles.x) > yawPrecision)
        {
            //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * yawSpeed);
            xAxis.rotation = Quaternion.Slerp(xAxis.rotation, rotation, Time.deltaTime * yawSpeed);
            yield return null;
        }
        xAxis.rotation = rotation;
        //this.transform.rotation = rotation;
    }
}