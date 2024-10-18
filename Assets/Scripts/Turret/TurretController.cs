using UnityEngine;

public class TurretController : MonoBehaviour
{
    private TurretAim TurretAim;

    public Transform TargetPoint = null;

    private void Awake()
    {
        TurretAim = this.GetComponent<TurretAim>();
    }

    private void Update()
    {
        if (TurretAim == null)
            return;

        if (TargetPoint == null)
            TurretAim.IsIdle = TargetPoint == null;
        else
            TurretAim.AimPosition = TargetPoint.position;

        /* if (Input.GetMouseButtonDown(0))
            TurretAim.IsIdle = !TurretAim.IsIdle; */
    }
}
