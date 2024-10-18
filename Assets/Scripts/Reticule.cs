using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    public static Reticule Instance { get; private set; }
    public float moveSpeed = 0.1f;

    private Vector3 mousePosition;
    private List<GameObject> activeEnemies;
    private int closestEnemyIndex;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        this.transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }

    public Transform GetClosestEnemy()
    {
        activeEnemies = ObjectPooling.Instance.UsedList("Mirage2000Pool");
        float previousDistance = 10000;
        float distance = 10000;
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            distance = Vector3.Distance(this.transform.position, activeEnemies[i].transform.position);
            if (distance < previousDistance)
            {
                previousDistance = distance;
                closestEnemyIndex = i;
            }
        }
        if (previousDistance != 10000)
        {
            return activeEnemies[closestEnemyIndex].transform;
        }
        else
        {
            Debug.Log("No target found.");
            return null;
        }
    }
}
