using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour
{
    public Transform inputTransform;
    public Transform aircraftTransform;
    public float speed = 2f;
    public float maxDistanceFromPlane = 7f;
    public float distanceEntryRun = 30f;
    public float distanceEndRun = 10f;
    public float distanceEscape = 50f;

    private GameObject[] targets;
    public List<Vector3> path = new List<Vector3>();
    private Vector3 currentTargetPosition;
    private Vector2 screenBounds;
    private Vector3 startPosition;
    private Vector3 entryRunPosition;
    private Vector3 endRunPosition;
    private Vector3 escapePosition;
    private Vector3 finishPosition;
    private float totalDistance;
    //private int indexCount = 0;

    void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
        currentTargetPosition = GetNewTarget();
        GeneratePath();
        StartCoroutine(LerpPosition());
    }

    /* void Update()
    {
        if (indexCount < path.Count)
        {
            inputTransform.position = Vector3.MoveTowards(inputTransform.position, path[indexCount], Time.deltaTime * speed);
            if (inputTransform.position == path[indexCount])
            {
                indexCount += 1;
            }
        }
    } */

    public int GetPathCount()
    {
        return path.Count;
    }

    public Vector3 GetPathPoint(int index)
    {
        return path[index];
    }

    public Vector3 GetEntryRunPosition()
    {
        return entryRunPosition;
    }

    public Vector3 GetEndRunPosition()
    {
        return endRunPosition;
    }

    private Vector3 GetNewTarget()
    {
        return targets[UnityEngine.Random.Range(0, targets.Length - 1)].transform.position;
    }

    private void GeneratePath()
    {
        startPosition = inputTransform.position;
        entryRunPosition = new Vector3(currentTargetPosition.x - distanceEntryRun, startPosition.y, startPosition.z);
        endRunPosition = currentTargetPosition - distanceEndRun * (currentTargetPosition - entryRunPosition).normalized;
        escapePosition = new Vector3(screenBounds.x, currentTargetPosition.y + distanceEscape, startPosition.z);
        finishPosition = new Vector3(screenBounds.x - 20f, screenBounds.y - 10f, startPosition.z);
        path.Add(startPosition);
        path.Add(entryRunPosition);
        path.Add(endRunPosition);
        path.Add(escapePosition);
        path.Add(finishPosition);
        totalDistance = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(path[i], path[i + 1]);
        }
    }

    IEnumerator LerpPosition()
    {
        for (int i = 1; i < path.Count; i++)
        {
            float time = 0;
            Vector3 startPosition = path[i - 1];
            while (time < speed)
            {
                if (inputTransform.position.x - aircraftTransform.position.x < maxDistanceFromPlane)
                {
                    inputTransform.position = Vector3.Lerp(startPosition, path[i], time / speed);
                    time += Time.deltaTime;
                }
                yield return null;
            }
            inputTransform.position = path[i];
        }
    }
}
