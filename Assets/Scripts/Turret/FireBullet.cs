using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] muzzles;
    public int rateOfFire = 1;

    private BulletData bulletData;
    private float timer = 0f;

    void Start()
    {
        bulletData = bulletPrefab.GetComponent<BulletData>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && timer <= 0f)
        {
            for (int i = 0; i < muzzles.Length; i++)
            {
                GameObject newBullet = ObjectPooling.Instance.Get(bulletPrefab.name + "Pool",
                                                                  muzzles[i].position,
                                                                  muzzles[i].rotation);
                newBullet.GetComponent<Bullet>().Initialize(muzzles[i].position, bulletData.muzzleVelocity * muzzles[i].forward);
            }
            timer = 1f / rateOfFire;
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
