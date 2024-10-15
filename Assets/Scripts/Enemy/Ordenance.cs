using UnityEngine;

[System.Serializable]
public class Ordenance : MonoBehaviour
{
    [System.Serializable]
    public class HardPoint
    {
        public Transform point;
        public GameObject weaponPrefab;
    }

    public HardPoint mainWeapon;
    public HardPoint[] hardPoints;

    private HardPoint activeHardPoint;
    private Ballistics activeWeaponBallistics;
    private int weaponsQuantity;

    public void Reset()
    {
        weaponsQuantity = 0;
        for (int i = 0; i < hardPoints.Length; i++)
        {
            if (hardPoints[i].point.childCount == 0)
            {
                GameObject weapon = ObjectPooling.Instance.Get(hardPoints[i].weaponPrefab.name + "Pool",
                                                               hardPoints[i].point.position,
                                                               hardPoints[i].point.rotation);
                weapon.transform.parent = hardPoints[i].point;
                weapon.SetActive(true);
                weaponsQuantity += 1;
                activeHardPoint = hardPoints[i];
                activeWeaponBallistics = hardPoints[i].point.GetComponentInChildren<Ballistics>();
            }
            else
            {
                weaponsQuantity += 1;
            }
        }
        if (activeHardPoint == null)
        {
            activeHardPoint = mainWeapon;
            activeWeaponBallistics = activeHardPoint.weaponPrefab.GetComponent<Ballistics>();
        }
    }

    public Ballistics ActiveWeaponBallistics()
    {
        return activeWeaponBallistics;
    }

    public int WeaponsQuantity()
    {
        return weaponsQuantity;
    }

    public void ReduceQuantity()
    {
        weaponsQuantity -= 1;
        if (weaponsQuantity == 0)
        {
            activeHardPoint = mainWeapon;
            activeWeaponBallistics = activeHardPoint.weaponPrefab.GetComponent<Ballistics>();
        }
        else
        {
            activeWeaponBallistics = hardPoints[weaponsQuantity - 1].point.GetComponentInChildren<Ballistics>();
        }
    }

    public bool IsMainWeaponActive()
    {
        return activeHardPoint == mainWeapon;
    }

    public void Fire(Vector3 direction, float targetZPlane)
    {
        if (weaponsQuantity > 0)
        {
            if (activeWeaponBallistics.ballisticsType == Ballistics.BallisticsType.BOMB)
            {
                activeWeaponBallistics.Initialize(activeHardPoint.point.position, direction, targetZPlane);
            }
            else if (activeWeaponBallistics.ballisticsType == Ballistics.BallisticsType.ROCKET)
            {
                activeWeaponBallistics.Initialize(activeHardPoint.point.position, direction, targetZPlane);
            }
            ReduceQuantity();
        }
    }

    public void Fire(float targetZPlane)
    {
        GameObject newBullet = ObjectPooling.Instance.Get(activeHardPoint.weaponPrefab.name + "Pool",
                                                          activeHardPoint.point.position,
                                                          activeHardPoint.point.rotation);
        newBullet.GetComponent<Ballistics>().Initialize(activeHardPoint.point.position, activeHardPoint.point.forward, targetZPlane);
    }
}
