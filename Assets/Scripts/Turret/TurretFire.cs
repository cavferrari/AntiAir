using UnityEngine;

public class TurretFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] muzzles;

    private float timer = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && timer <= 0f)
        {
            for (int i = 0; i < muzzles.Length; i++)
            {
                GameObject newBullet = ObjectPooling.Instance.Get(bulletPrefab.name + "Pool",
                                                                  muzzles[i].position,
                                                                  muzzles[i].rotation);
                newBullet.GetComponent<Ballistics>().Initialize(muzzles[i].position, muzzles[i].forward);
                timer = 1f / newBullet.GetComponent<Ballistics>().rateFire;
            }
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
