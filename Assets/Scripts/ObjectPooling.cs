using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjectPooling : Singleton<ObjectPooling>
{
  private List<Pool[]> groupPools;

  void Awake()
  {
    DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/ObjectPooling");
    DirectoryInfo[] directories = directoryInfo.GetDirectories();
    List<string> folders = new List<string>(directories.Length);
    foreach (DirectoryInfo info in directories)
    {
      folders.Add(info.Name);
    }
    groupPools = new List<Pool[]>(folders.Count);
    GameObject poolGroup;
    GameObject[] resources;
    Pool[] pools;
    GameObject poolParent;
    for (int i = 0; i < folders.Count; i++)
    {
      poolGroup = new GameObject(folders[i]);
      poolGroup.transform.position = Vector3.zero;
      poolGroup.transform.rotation = Quaternion.identity;
      poolGroup.transform.parent = this.transform;
      resources = Resources.LoadAll<GameObject>("ObjectPooling/" + folders[i]);
      pools = new Pool[resources.Length];
      for (int j = 0; j < resources.Length; j++)
      {
        pools[j] = new Pool(resources[j], GetPoolSize(resources[j].name));
        poolParent = new GameObject(pools[j].prefab.name + "Pool");
        poolParent.transform.position = Vector3.zero;
        poolParent.transform.rotation = Quaternion.identity;
        poolParent.transform.parent = poolGroup.transform;
        pools[j].Initialize(poolParent.transform);
      }
      groupPools.Add(pools);
    }
  }

  public GameObject Get(string poolName, Vector3 position, Quaternion rotation)
  {
    for (int i = 0; i < groupPools.Count; i++)
    {
      for (int j = 0; j < groupPools[i].Length; j++)
      {
        if (groupPools[i][j].GetName().Equals(poolName))
        {
          return groupPools[i][j].Get(position, rotation);
        }
      }
    }
    return null;
  }

  public int GetPoolFreeListSize(string poolName)
  {
    for (int i = 0; i < groupPools.Count; i++)
    {
      for (int j = 0; j < groupPools[i].Length; j++)
      {
        if (groupPools[i][j].GetName().Equals(poolName))
        {
          return groupPools[i][j].GetFreeListSize();
        }
      }
    }
    return 0;
  }

  public void ReturnObject(GameObject pooledObject)
  {
    ReturnObject(GetPoolName(pooledObject), pooledObject);
  }

  public void ReturnObject(string poolName, GameObject pooledObject)
  {
    for (int i = 0; i < groupPools.Count; i++)
    {
      for (int j = 0; j < groupPools[i].Length; j++)
      {
        if (groupPools[i][j].GetName().Equals(poolName))
        {
          groupPools[i][j].ReturnObject(pooledObject);
        }
      }
    }
  }

  public string GetPoolName(GameObject pooledObject)
  {
    int index = pooledObject.name.IndexOf("_");
    int length = pooledObject.name.Length - index;
    return pooledObject.name.Remove(index, length).Insert(index, "Pool");
  }

  [System.Serializable]
  public class Pool
  {
    public GameObject prefab;
    public int size;

    private string name;
    private List<GameObject> freeList;
    private List<GameObject> usedList;

    public Pool(GameObject prefab, int size)
    {
      this.prefab = prefab;
      this.size = size;
    }

    public string GetName()
    {
      return name;
    }

    public int GetFreeListSize()
    {
      return freeList.Count;
    }

    public void Initialize(Transform parent)
    {
      name = prefab.name + "Pool";
      freeList = new List<GameObject>(size);
      usedList = new List<GameObject>(size);
      for (int i = 0; i < size; i++)
      {
        GameObject pooledObject = Instantiate(prefab, parent);
        pooledObject.name = pooledObject.name.Replace("(Clone)", "_" + i);
        pooledObject.gameObject.SetActive(false);
        freeList.Add(pooledObject);
      }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
      if (freeList.Count == 0)
      {
        ReturnObject(usedList[0]);
      }
      GameObject pooledObject = freeList[freeList.Count - 1];
      freeList.RemoveAt(freeList.Count - 1);
      pooledObject.transform.position = position;
      pooledObject.transform.rotation = rotation;
      pooledObject.gameObject.SetActive(true);
      usedList.Add(pooledObject);
      return pooledObject;
    }

    public void ReturnObject(GameObject pooledObject)
    {
      usedList.Remove(pooledObject);
      pooledObject.transform.localPosition = Vector3.zero;
      pooledObject.transform.rotation = Quaternion.identity;
      pooledObject.gameObject.SetActive(false);
      freeList.Add(pooledObject);
    }
  }

  private int GetPoolSize(string name)
  {
    return name switch
    {
      "TurretBullet" => 1000,
      "EnemyBullet" => 1000,
      "Rocket" => 100,
      "PlaceHolderExplosionBullet" => 1000,
      "TurretBulletMuzzleAudio" => 1000,
      "TurretBulletExplosionAirAudio" => 100,
      _ => 10,
    };
  }
}
