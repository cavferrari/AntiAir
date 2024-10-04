using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotFireControl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public GameObject[] muzzleFlashes;
    public GameObject[] weaponsPrefabs;
    public Transform[] weaponsRails;
    public int rateOfFire = 1;
    public float distanceFromTarget = 50f;

    private BallisticData ballisticData;
    private float timer = 0f;
    private float muzzleFlashTimer = 0f;
    private bool fireBullet = false;
    private bool fireWeapon = false;
    private int currentMuzzleFlashIndex = -1;
    private Vector3 muzzleFlashEuler;
    private List<GameObject> weapons = new List<GameObject>();

    void Start()
    {
        ballisticData = bulletPrefab.GetComponent<BallisticData>();
        for (int i = 0; i < weaponsPrefabs.Length; i++)
        {
            for (int j = 0; j < weaponsRails.Length; j++)
            {
                GameObject weapon = ObjectPooling.Instance.Get(weaponsPrefabs[i].name + "Pool",
                                                               weaponsRails[j].position,
                                                               weaponsRails[j].rotation);
                weapon.transform.parent = weaponsRails[j].transform;
                weapon.SetActive(true);
                weapons.Add(weapon);
            }
        }
    }

    void Update()
    {
        FireWeapon();
        FireBullet();
        UpdateMuzzleFlash();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;
        }
    }

    public bool IsFiring()
    {
        return fireWeapon || fireBullet;
    }

    public void SetFire(bool set)
    {
        if (set)
        {
            if (weapons.Count == 0)
            {
                fireWeapon = false;
                fireBullet = true;
            }
            else
            {
                fireWeapon = true;
                fireBullet = false;
            }
        }
        else
        {
            fireWeapon = set;
            fireBullet = set;
        }
    }

    public bool HasWeapon()
    {
        return weapons.Count != 0;
    }

    private void FireWeapon()
    {
        if (fireWeapon && weapons.Count > 0 && timer <= 0f)
        {
            GameObject weapon = weapons.Last();
            weapon.GetComponent<Ballistics>().Initialize(weapon.transform.parent.position, weapon.GetComponent<BallisticData>().muzzleVelocity * weapon.transform.parent.forward);
            timer = 0.5f;
            weapons.Remove(weapon);
        }
    }

    private void FireBullet()
    {
        if (fireBullet && timer <= 0f)
        {
            GameObject newBullet = ObjectPooling.Instance.Get(bulletPrefab.name + "Pool",
                                                              muzzle.position,
                                                              muzzle.rotation);
            newBullet.GetComponent<Ballistics>().Initialize(muzzle.position, ballisticData.muzzleVelocity * muzzle.forward);
            timer = 1f / rateOfFire;
            CreateMuzzleFlash();
        }
    }

    private void CreateMuzzleFlash()
    {
        currentMuzzleFlashIndex = Random.Range(0, muzzleFlashes.Length - 1);
        muzzleFlashes[currentMuzzleFlashIndex].transform.localScale = Vector3.one * (Random.value * 2f);
        muzzleFlashEuler = muzzleFlashes[currentMuzzleFlashIndex].transform.eulerAngles;
        muzzleFlashEuler.z = Random.Range(0f, 360f);
        muzzleFlashes[currentMuzzleFlashIndex].transform.eulerAngles = muzzleFlashEuler;
        muzzleFlashes[currentMuzzleFlashIndex].SetActive(true);
        muzzleFlashTimer = (1f / rateOfFire) / 2f;
    }

    private void UpdateMuzzleFlash()
    {
        if (currentMuzzleFlashIndex != -1 && muzzleFlashTimer <= 0f)
        {
            muzzleFlashes[currentMuzzleFlashIndex].SetActive(false);
            currentMuzzleFlashIndex = -1;
        }
    }
}
