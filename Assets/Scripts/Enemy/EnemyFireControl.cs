using System.Collections;
using UnityEngine;

public class EnemyFireControl : MonoBehaviour
{
    public GameObject[] muzzleFlashes;
    public AudioClip[] muzzleAudioClips;

    private AudioSource audioSource;
    private Ordenance ordenance;
    private Enemy enemy;
    private float timer = 0f;
    private Vector3 muzzleFlashEuler;
    private bool fire;
    private bool previousFire;
    private bool isFiringWeapons = false;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        ordenance = this.GetComponent<Ordenance>();
        enemy = this.GetComponent<Enemy>();
    }

    void Update()
    {
        UpdateFire();
        UpdateMuzzleAudio();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Reset()
    {
        ordenance.Reset();
        timer = 0;
        fire = false;
        isFiringWeapons = false;
        for (int i = 0; i < muzzleFlashes.Length; i++)
        {
            muzzleFlashes[i].SetActive(false);
        }
    }

    public bool IsFiring()
    {
        return fire;
    }

    public void SetFire(bool value)
    {
        previousFire = fire;
        fire = value;
        if (!fire)
        {
            isFiringWeapons = false;
        }
    }

    public float DistanceFromTargetTrigger()
    {
        return ordenance.ActiveWeaponBallistics().distanceFromTargetTrigger;
    }

    private void UpdateFire()
    {
        if (fire && timer <= 0f)
        {
            timer = 1f / ordenance.ActiveWeaponBallistics().rateFire;
            if (!isFiringWeapons && ordenance.IsMainWeaponActive())
            {
                ordenance.Fire(this.transform.position.z);
                StartCoroutine(CreateMuzzleFlash((1f / ordenance.ActiveWeaponBallistics().rateFire) / 2f));
            }
            else
            {
                ordenance.Fire(enemy.Velocity(), this.transform.position.z);
                isFiringWeapons = true;
            }
        }
    }

    private void UpdateMuzzleAudio()
    {
        if (!isFiringWeapons && ordenance.IsMainWeaponActive())
        {
            if (fire)
            {
                if (!previousFire)
                {
                    audioSource.loop = false;
                    audioSource.clip = muzzleAudioClips[0];
                    audioSource.Play();
                    previousFire = fire;
                }
                else
                {
                    if (audioSource.clip == muzzleAudioClips[0] && !audioSource.isPlaying)
                    {
                        audioSource.loop = true;
                        audioSource.clip = muzzleAudioClips[1];
                        audioSource.Play();
                    }
                }
            }
            else
            {
                if (previousFire && audioSource.clip == muzzleAudioClips[1])
                {
                    audioSource.loop = false;
                    audioSource.clip = muzzleAudioClips[2];
                    audioSource.Play();
                    previousFire = fire;
                }
            }
        }
    }

    private IEnumerator CreateMuzzleFlash(float waitTime)
    {
        int index = Random.Range(0, muzzleFlashes.Length - 1);
        muzzleFlashes[index].transform.localScale = Vector3.one * (Random.value * 2f);
        muzzleFlashEuler = muzzleFlashes[index].transform.eulerAngles;
        muzzleFlashEuler.z = Random.Range(0f, 360f);
        muzzleFlashes[index].transform.eulerAngles = muzzleFlashEuler;
        muzzleFlashes[index].SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        muzzleFlashes[index].SetActive(false);
    }
}