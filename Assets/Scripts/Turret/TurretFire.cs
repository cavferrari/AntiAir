using UnityEngine;

public class TurretFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] muzzles;
    public GameObject muzzleSoundPrefab;
    public float intervalBetweenBarrels = 0.5f;

    private float[] timers;
    private AudioSource[] audioSources;

    void Start()
    {
        timers = new float[muzzles.Length];
        audioSources = new AudioSource[muzzles.Length];
        for (int i = 0; i < timers.Length; i++)
        {
            timers[i] = intervalBetweenBarrels * i * timers.Length;
            audioSources[i] = Instantiate(muzzleSoundPrefab, this.transform).GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            for (int i = 0; i < muzzles.Length; i++)
            {
                if (timers[i] <= 0f)
                {
                    GameObject newBullet = ObjectPooling.Instance.Get(bulletPrefab.name + "Pool",
                                                                      muzzles[i].position,
                                                                      muzzles[i].rotation);
                    newBullet.GetComponent<Ballistics>().Initialize(muzzles[i].position, muzzles[i].forward, muzzles[i].position.z);
                    if (i + 1 < timers.Length)
                    {
                        timers[i] = timers[i + 1] + intervalBetweenBarrels;
                    }
                    else
                    {
                        timers[i] = timers[0] + intervalBetweenBarrels;
                    }
                    audioSources[i].PlayOneShot(audioSources[i].clip);
                }
                else
                {
                    timers[i] -= Time.deltaTime;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < timers.Length; i++)
            {
                timers[i] = timers[i] * intervalBetweenBarrels * timers.Length;
            }
        }
    }
}
