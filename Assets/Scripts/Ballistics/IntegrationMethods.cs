using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods
{
    private static Vector3 acceleartionFactorEuler;
    private static Vector3 acceleartionFactorHeun;
    private static Vector3 pos_E;
    private static Vector3 vel_E;
    private static Vector3 velocityFactor;
    private static Vector3 pos_H;
    private static Vector3 vel_H;

    //Heun's method - one iteration
    //Will give a better result than Euler forward, but will not match Unity's physics engine
    //so the bullets also have to use Heuns method
    public static void Heuns(float timeStep,
                             Vector3 currentPosition,
                             Vector3 currentVelocity,
                             BallisticData ballisticData,
                             out Vector3 newPosition,
                             out Vector3 newVelocity)
    {
        //Init acceleration
        //Gravity
        acceleartionFactorEuler = Physics.gravity;
        acceleartionFactorHeun = Physics.gravity;
        //Init velocity
        //Current velocity
        velocityFactor = currentVelocity;
        //
        //Main algorithm
        //
        //Euler forward
        pos_E = currentPosition + timeStep * velocityFactor;
        acceleartionFactorEuler += ballisticData.CalculateDrag(currentVelocity);
        vel_E = currentVelocity + timeStep * acceleartionFactorEuler;
        //Heuns method
        pos_H = currentPosition + timeStep * 0.5f * (velocityFactor + vel_E);
        acceleartionFactorHeun += ballisticData.CalculateDrag(vel_E);
        vel_H = currentVelocity + timeStep * 0.5f * (acceleartionFactorEuler + acceleartionFactorHeun);
        newPosition = pos_H;
        newVelocity = vel_H;
    }

    public static void CurrentIntegrationMethod(float timeStep,
                                                Vector3 currentPosition,
                                                Vector3 currentVelocity,
                                                BallisticData ballisticData,
                                                out Vector3 newPosition,
                                                out Vector3 newVelocity)
    {
        Heuns(timeStep, currentPosition, currentVelocity, ballisticData, out newPosition, out newVelocity);
    }
}
