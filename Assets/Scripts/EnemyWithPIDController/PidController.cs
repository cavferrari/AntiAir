using System;
using UnityEngine;

public class PidController : MonoBehaviour
{
    public enum DerivativeMeasurement
    {
        Velocity,
        ErrorRateOfChange
    }

    //PID coefficients
    public float proportionalGain = 1f;
    public float integralGain = 1f;
    public float derivativeGain = 1f;
    public float integralSaturation = 1f;
    public float outputMinVertical = -1f;
    public float outputMaxVertical = 1f;
    public float outputMinHorizontal = -1f;
    public float outputMaxHorizontal = 1f;
    public DerivativeMeasurement derivativeMeasurement;

    private float valueLastVertical;
    private float valueLastHorizontal;
    private float errorLastVertical;
    private float errorLastHorizontal;
    private float integrationStoredVertical;
    private float integrationStoredHorizontal;
    private float velocityVertical;
    private float velocityHorizontal;
    private bool derivativeInitializedVertical;
    private bool derivativeInitializedHorizontal;

    public void ResetVertical()
    {
        derivativeInitializedVertical = false;
    }

    public void ResetHorizontal()
    {
        derivativeInitializedHorizontal = false;
    }

    public float CalculateVertical(float dt, float currentValue, float targetValue)
    {
        return Calculate(dt,
                         currentValue,
                         targetValue,
                         ref integrationStoredVertical,
                         ref errorLastVertical,
                         ref valueLastVertical,
                         ref velocityVertical,
                         ref derivativeInitializedVertical,
                         outputMinVertical,
                         outputMaxVertical);
    }

    public float CalculateHorizontal(float dt, float currentValue, float targetValue)
    {
        return Calculate(dt,
                         currentValue,
                         targetValue,
                         ref integrationStoredHorizontal,
                         ref errorLastHorizontal,
                         ref valueLastHorizontal,
                         ref velocityHorizontal,
                         ref derivativeInitializedHorizontal,
                         outputMinHorizontal,
                         outputMaxHorizontal);
    }

    public float UpdateAngleVertical(float dt, float currentAngle, float targetAngle)
    {
        return UpdateAngle(dt,
                           currentAngle,
                           targetAngle,
                           ref errorLastVertical,
                           ref integrationStoredVertical,
                           ref valueLastVertical,
                           ref velocityVertical,
                           ref derivativeInitializedVertical,
                           outputMinVertical,
                           outputMaxVertical);
    }

    public float UpdateAngleHorizontal(float dt, float currentAngle, float targetAngle)
    {
        return UpdateAngle(dt,
                           currentAngle,
                           targetAngle,
                           ref errorLastHorizontal,
                           ref integrationStoredHorizontal,
                           ref valueLastHorizontal,
                           ref velocityHorizontal,
                           ref derivativeInitializedHorizontal,
                           outputMinHorizontal,
                           outputMaxHorizontal);
    }

    private float Calculate(float dt,
                            float currentValue,
                            float targetValue,
                            ref float integrationStored,
                            ref float errorLast,
                            ref float valueLast,
                            ref float velocity,
                            ref bool derivativeInitialized,
                            float outputMin,
                            float outputMax)
    {
        if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));
        float error = targetValue - currentValue;
        //calculate P term
        float P = proportionalGain * error;
        //calculate I term
        integrationStored = Mathf.Clamp(integrationStored + (error * dt), -integralSaturation, integralSaturation);
        float I = integralGain * integrationStored;
        //calculate both D terms
        float errorRateOfChange = (error - errorLast) / dt;
        errorLast = error;
        float valueRateOfChange = (currentValue - valueLast) / dt;
        valueLast = currentValue;
        velocity = valueRateOfChange;
        //choose D term to use
        float deriveMeasure = 0;
        if (derivativeInitialized)
        {
            if (derivativeMeasurement == DerivativeMeasurement.Velocity)
            {
                deriveMeasure = -valueRateOfChange;
            }
            else
            {
                deriveMeasure = errorRateOfChange;
            }
        }
        else
        {
            derivativeInitialized = true;
        }
        float D = derivativeGain * deriveMeasure;
        float result = P + I + D;
        return Mathf.Clamp(result, outputMin, outputMax);
    }

    private float UpdateAngle(float dt,
                              float currentAngle,
                              float targetAngle,
                              ref float errorLast,
                              ref float integrationStored,
                              ref float valueLast,
                              ref float velocity,
                              ref bool derivativeInitialized,
                              float outputMin,
                              float outputMax)
    {
        if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));
        float error = AngleDifference(targetAngle, currentAngle);
        errorLast = error;
        //calculate P term
        float P = proportionalGain * error;
        //calculate I term
        integrationStored = Mathf.Clamp(integrationStored + (error * dt), -integralSaturation, integralSaturation);
        float I = integralGain * integrationStored;
        //calculate both D terms
        float errorRateOfChange = AngleDifference(error, errorLast) / dt;
        errorLast = error;
        float valueRateOfChange = AngleDifference(currentAngle, valueLast) / dt;
        valueLast = currentAngle;
        velocity = valueRateOfChange;
        //choose D term to use
        float deriveMeasure = 0;
        if (derivativeInitialized)
        {
            if (derivativeMeasurement == DerivativeMeasurement.Velocity)
            {
                deriveMeasure = -valueRateOfChange;
            }
            else
            {
                deriveMeasure = errorRateOfChange;
            }
        }
        else
        {
            derivativeInitialized = true;
        }
        float D = derivativeGain * deriveMeasure;
        float result = P + I + D;
        return Mathf.Clamp(result, outputMin, outputMax);
    }

    private float AngleDifference(float a, float b)
    {
        return (a - b + 540) % 360 - 180;   //calculate modular difference, and remap to [-180, 180]
    }
}
