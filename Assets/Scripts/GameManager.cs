using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 2f;
    public float minSpawnVerticalValue = 100f;
    public GameObject[] enemiesPrefabs;

    public Transform sun;
    public float dayLenghtMinutes = 0f;
    public TextMeshProUGUI timeText;

    private float horizontalBorderLeft;
    private float horizontalBorderRight;
    private float topBorder;
    private float timer = 0f;
    private float spawnYRotation;
    private GameObject newEnemy;
    private float currenyTime;
    private float rotationSpeed;
    private float midDay;
    private float translatedTime;
    private string ampm = " AM";
    private string displayHours;
    private string displayMinutes;
    private string displaTime;
    private bool isNight = false;

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

        rotationSpeed = 360f / dayLenghtMinutes / 60f;
        midDay = dayLenghtMinutes * 60f / 2f;
        currenyTime = midDay;
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
        DayNightCycle();
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

    public bool IsNight()
    {
        return isNight;
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

    private void DayNightCycle()
    {
        currenyTime += 1 * Time.deltaTime;
        translatedTime = currenyTime / (midDay * 2);
        float t = translatedTime * 24f;
        float hours = Mathf.FloorToInt(t);
        displayHours = hours.ToString();
        if (hours == 0f)
        {
            displayHours = "12";
        }
        if (hours > 12f)
        {
            displayHours = (hours - 12f).ToString();
        }
        if (currenyTime >= midDay)
        {
            if (!ampm.Equals(" PM"))
            {
                ampm = " PM";
            }
        }
        if (currenyTime >= midDay * 2f)
        {
            currenyTime = 0f;
            if (!ampm.Equals(" AM"))
            {
                ampm = " AM";
            }
        }
        if (hours >= 18f || hours <= 5f)
        {
            isNight = true;
        }
        else
        {
            isNight = false;
        }
        t *= 60f;
        float minutes = Mathf.Floor(t % 60f);
        displayMinutes = minutes.ToString();
        if (minutes < 10f)
        {
            displayMinutes = "0" + minutes.ToString();
        }
        displaTime = displayHours + " : " + displayMinutes + ampm;
        timeText.text = displaTime;
        sun.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
