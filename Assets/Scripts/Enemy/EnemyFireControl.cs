using System.Collections;
using UnityEngine;

public class EnemyFireControl : MonoBehaviour
{
    public GameObject[] muzzleFlashes;

    private Ordenance ordenance;
    private Enemy enemy;
    private float timer = 0f;
    private Vector3 muzzleFlashEuler;
    private bool fire;
    private bool isFiringWeapons = false;

    void Start()
    {
        ordenance = this.GetComponent<Ordenance>();
        enemy = this.GetComponent<Enemy>();
    }

    void Update()
    {
        UpdateFire();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public bool IsFiring()
    {
        return fire;
    }

    public void SetFire(bool value)
    {
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
                ordenance.Fire();
                StartCoroutine(CreateMuzzleFlash((1f / ordenance.ActiveWeaponBallistics().rateFire) / 2f));
            }
            else
            {
                ordenance.Fire(enemy.Velocity());
                isFiringWeapons = true;
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