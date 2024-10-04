using UnityEngine;

public class BallisticData : MonoBehaviour
{
    //Data belonging to this bullet type
    //The initial speed [m/s]
    public float muzzleVelocity = 10f;
    //Mass [kg]
    public float mass = 0.2f;
    //Radius [m]
    public float radius = 0.05f;
    //Coefficients, which is a value you can't calculate - you have to simulate it in a wind tunnel
    //and they also depends on the speed, so we pick some average value
    //Drag coefficient (Tesla Model S has the drag coefficient 0.24)
    public float dragCoefficient = 0.5f;
    //The density of the medium the bullet is travelling in, which in this case is air at 15 degrees [kg/m^3]
    public float mediumDensity = 1.225f;

    private Vector3 dragVec;

    public Vector3 CalculateDrag(Vector3 velocityVec)
    {
        float A = Mathf.PI * radius * radius; // m^2
        float k = 0.5f * dragCoefficient * mediumDensity * A;
        float vSqr = velocityVec.sqrMagnitude;
        float aDrag = (k * vSqr) / mass;
        //Has to be in a direction opposite of the bullet's velocity vector
        dragVec = -1f * aDrag * velocityVec.normalized;
        dragVec.z = 0f;
        return dragVec;
    }
}
