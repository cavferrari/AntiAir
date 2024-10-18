using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;

public class TurretFireMissile : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform[] launcherRails;
    public float reloadTime = 1f;

    private GameObject newMissile;
    private List<Missile> missiles;
    private Missile missile;
    private int capacity;
    private Transform target;
    private float reloadTimer = 0f;

    void Start()
    {
        capacity = launcherRails.Length;
        missiles = new List<Missile>(capacity);
        Reload();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && missiles.Count > 0)
        {
            target = Reticule.Instance.GetClosestEnemy();
            if (target)
            {
                missile = missiles.Last();
                missile.Launch(target, (target.position - missile.transform.parent.position).normalized * 10f);
                missiles.RemoveAt(missiles.Count - 1);
            }
        }
        if (missiles.Count == 0)
        {
            if (reloadTimer >= reloadTime)
            {
                Reload();
                reloadTimer = 0f;
            }
            else
            {
                reloadTimer += Time.deltaTime;
            }
        }
    }

    private void Reload()
    {
        for (int i = 0; i < capacity; i++)
        {
            newMissile = ObjectPooling.Instance.Get(missilePrefab.name + "Pool",
                                                    launcherRails[i].position,
                                                    launcherRails[i].rotation);
            newMissile.transform.parent = launcherRails[i].transform;
            newMissile.SetActive(true);
            missiles.Add(newMissile.GetComponent<Missile>());
        }
    }
}
