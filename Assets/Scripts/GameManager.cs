using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2f;
    public float minSpawnVerticalValue = 100f;
    public GameObject[] enemiesPrefabs;
    public GameObject postExplosionSmokePrefab;
    public float postExplosionSmokeTime = 10f;

    private float horizontalBorderLeft;
    private float horizontalBorderRight;
    private float topBorder;
    private float timer = 0f;
    private float spawnYRotation;
    private GameObject newEnemy;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Vector2 screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
        horizontalBorderLeft = -screenBounds.x;
        horizontalBorderRight = screenBounds.x;
        topBorder = screenBounds.y;
        Cursor.visible = false;
    }

    void Update()
    {
        if (timer <= 0f)
        {
            newEnemy = ObjectPooling.Instance.Get(enemiesPrefabs[0].name + "Pool",
                                                  GenerateRandonSpawnPosition(),
                                                  Quaternion.Euler(new Vector3(0f, spawnYRotation, 0f)));
            newEnemy.GetComponent<Enemy>().Initialize();
            timer = Random.Range(minSpawnTime, maxSpawnTime);
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public float HorizontalBorderLeft()
    {
        return horizontalBorderLeft;
    }

    public float HorizontalBorderRight()
    {
        return horizontalBorderRight;
    }

    public float TopBorder()
    {
        return topBorder;
    }

    private Vector3 GenerateRandonSpawnPosition()
    {
        float x, y, z;
        if (Random.Range(0, 2) == 0)
        {
            x = horizontalBorderLeft - 10f;
            spawnYRotation = 90f;
        }
        else
        {
            x = horizontalBorderRight - 10f;
            spawnYRotation = -90f;
        }
        y = Random.Range(minSpawnVerticalValue, topBorder - 10f);
        z = 0f;
        return new Vector3(x, y, z);
    }
}
