using UnityEngine;

public class TurretFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] muzzles;
    public GameObject muzzleSoundPrefab;

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
                newBullet.GetComponent<Ballistics>().Initialize(muzzles[i].position, muzzles[i].forward, muzzles[i].position.z);
                timer = 1f / newBullet.GetComponent<Ballistics>().rateFire;
                newBullet = ObjectPooling.Instance.Get(muzzleSoundPrefab.name + "Pool",
                                                       muzzles[i].position,
                                                       Quaternion.identity);
                newBullet.GetComponent<SoundEffect>().Play();
            }
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }
}
