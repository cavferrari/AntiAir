using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public int rateOfFire = 1;
    public float distanceFromTarget = 50f;

    private BulletData bulletData;
    private float timer = 0f;
    private bool fire = false;

    void Start()
    {
        bulletData = bulletPrefab.GetComponent<BulletData>();
    }

    void Update()
    {
        if (fire && timer <= 0f)
        {
            GameObject newBullet = ObjectPooling.Instance.Get(bulletPrefab.name + "Pool",
                                                              muzzle.position,
                                                              muzzle.rotation);
            newBullet.GetComponent<Bullet>().Initialize(muzzle.position, bulletData.muzzleVelocity * muzzle.forward);
            timer = 1f / rateOfFire;
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void SetFire(bool set)
    {
        fire = set;
    }
}
