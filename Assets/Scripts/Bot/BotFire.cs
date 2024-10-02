using UnityEngine;

public class BotFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public GameObject[] muzzleFlashes;
    public int rateOfFire = 1;
    public float distanceFromTarget = 50f;

    private BulletData bulletData;
    private float timer = 0f;
    private float muzzleFlashtimer = 0f;
    private bool fire = false;
    private int currentMuzzleFlashIndex = 0;
    private Vector3 muzzleFlashEuler;

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
            currentMuzzleFlashIndex = Random.Range(0, muzzleFlashes.Length - 1);
            muzzleFlashes[currentMuzzleFlashIndex].transform.localScale = Vector3.one * (Random.value * 2f);
            muzzleFlashEuler = muzzleFlashes[currentMuzzleFlashIndex].transform.eulerAngles;
            muzzleFlashEuler.z = Random.Range(0f, 360f);
            muzzleFlashes[currentMuzzleFlashIndex].transform.eulerAngles = muzzleFlashEuler;
            muzzleFlashes[currentMuzzleFlashIndex].SetActive(true);
            muzzleFlashtimer = timer / 2f;
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if (muzzleFlashtimer <= 0f)
        {
            muzzleFlashes[currentMuzzleFlashIndex].SetActive(false);
        }
        if (muzzleFlashtimer > 0f)
        {
            muzzleFlashtimer -= Time.deltaTime;
        }
    }

    public void SetFire(bool set)
    {
        fire = set;
    }
}
