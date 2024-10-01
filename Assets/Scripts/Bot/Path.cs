using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public float curveRadius = 50f;
    public int curveNumberPoints = 15;
    public float distanceEntryRun = 150f;
    public float distanceEndRun = 50f;
    public float distanceEscape = 200f;
    public int orientation;

    private float horizontalBorder, topBorder;
    private Vector3 startPosition, returnPosition, finishPosition, entryRunPosition, endRunPosition, escapePosition;
    private Vector3 entryRunRollPosition, endRunRollPosition;
    private Vector3 lineBA, lineCB;
    private Vector3 curveStartPoint, curveEndPoint;
    private Vector3 m1, m2;
    private List<Vector3> path = new List<Vector3>();
    private int turnCounter;

    void Awake()
    {
        Vector2 screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
        horizontalBorder = screenBounds.x;
        topBorder = screenBounds.y;
    }

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

    public Vector3 GetEntryRunRollPosition()
    {
        return entryRunRollPosition;
    }

    public Vector3 GetEndRunRollPosition()
    {
        return endRunRollPosition;
    }

    public Vector3 GetEscapePosition()
    {
        return escapePosition;
    }

    public Vector3 GetReturnPosition()
    {
        return returnPosition;
    }

    public Vector3 GetFinishPosition()
    {
        return finishPosition;
    }

    public bool IsTurning(Vector3 playerPosition)
    {
        bool isTurning = false;
        if (orientation == 1)
        {
            if (playerPosition.x >= entryRunRollPosition.x && playerPosition.x < endRunRollPosition.x && turnCounter == 0)
            {
                isTurning = true;
                turnCounter += 1;
            }
            else if (playerPosition.x >= endRunRollPosition.x && turnCounter == 1)
            {
                isTurning = true;
                turnCounter += 1;
            }
        }
        else
        {
            if (playerPosition.x <= entryRunRollPosition.x && playerPosition.x > endRunRollPosition.x && turnCounter == 0)
            {
                isTurning = true;
                turnCounter += 1;
            }
            else if (playerPosition.x <= endRunRollPosition.x && turnCounter == 1)
            {
                isTurning = true;
                turnCounter += 1;
            }
        }
        return isTurning;
    }

    public void GeneratePath(Vector3 playerPosition, Vector3 targetPosition, float rollEntryDistance, float rollEndDistance)
    {
        path.Clear();
        turnCounter = 0;

        if (playerPosition.x > targetPosition.x)
        {
            orientation = -1;
        }
        else
        {
            orientation = 1;
        }

        startPosition = playerPosition;
        entryRunPosition = CalculateEntryRunPosition(startPosition, targetPosition, distanceEntryRun * orientation);

        path.Add(startPosition);

        curveStartPoint = GetCurveStartPoint(startPosition, entryRunPosition, targetPosition, 1f);
        curveEndPoint = GetCurveEndPoint(startPosition, entryRunPosition, targetPosition, 1f);
        AddCurvePoints(curveStartPoint, curveEndPoint, entryRunPosition);
        entryRunPosition = curveStartPoint;
        entryRunRollPosition = entryRunPosition - rollEntryDistance * Vector3.right * orientation;

        endRunPosition = CalculateEndRunPosition(curveEndPoint, targetPosition, distanceEndRun);
        escapePosition = CalculateEscapePosition(startPosition, targetPosition, distanceEscape);

        curveStartPoint = GetCurveStartPoint(curveEndPoint, endRunPosition, escapePosition, 2f);
        curveEndPoint = GetCurveEndPoint(curveEndPoint, endRunPosition, escapePosition, 2f);
        AddCurvePoints(curveStartPoint, curveEndPoint, endRunPosition);
        endRunPosition = curveStartPoint;
        endRunRollPosition = endRunPosition + rollEndDistance * Vector3.right * orientation;

        returnPosition = new Vector3((horizontalBorder * orientation) - (10f * orientation), escapePosition.y + curveRadius * 2f, startPosition.z);

        curveStartPoint = GetCurveStartPoint(curveEndPoint, escapePosition, returnPosition, 1);
        curveEndPoint = GetCurveEndPoint(curveEndPoint, escapePosition, returnPosition, 1);
        AddCurvePoints(curveStartPoint, curveEndPoint, escapePosition);

        finishPosition = new Vector3(curveEndPoint.x - (40f * orientation), returnPosition.y + 5f, startPosition.z);

        curveStartPoint = GetCurveStartPoint(curveEndPoint, returnPosition, finishPosition, 1);
        curveEndPoint = GetCurveEndPoint(curveEndPoint, returnPosition, finishPosition, 1);
        AddCurvePoints(curveStartPoint, curveEndPoint, returnPosition);

        finishPosition = new Vector3(curveEndPoint.x - (10f * orientation), curveEndPoint.y, startPosition.z);

        path.Add(finishPosition);
    }

    private Vector3 CalculateEntryRunPosition(Vector3 startPosition, Vector3 targetPosition, float distance)
    {
        return new Vector3(targetPosition.x - distance, startPosition.y, startPosition.z);
    }

    private Vector3 CalculateEndRunPosition(Vector3 startPosition, Vector3 targetPosition, float distance)
    {
        return targetPosition - distance * (targetPosition - startPosition).normalized;
    }

    private Vector3 CalculateEscapePosition(Vector3 startPosition, Vector3 targetPosition, float distance)
    {
        return new Vector3(horizontalBorder * orientation - (10f * orientation), targetPosition.y + distance, startPosition.z);
    }

    private Vector3 GetCurveStartPoint(Vector3 pointA, Vector3 pointB, Vector3 pointC, float radiusMultiplier)
    {
        lineBA = pointB - pointA;
        lineCB = pointC - pointB;
        float distanceTangent = curveRadius * radiusMultiplier;
        return pointB - distanceTangent * lineBA.normalized;
    }

    private Vector3 GetCurveEndPoint(Vector3 pointA, Vector3 pointB, Vector3 pointC, float radiusMultiplier)
    {
        lineBA = pointB - pointA;
        lineCB = pointC - pointB;
        float distanceTangent = curveRadius * radiusMultiplier;
        return pointB + distanceTangent * lineCB.normalized;
    }

    private void AddCurvePoints(Vector3 startPosition, Vector3 endPosition, Vector3 controlPoint)
    {
        path.Add(startPosition);
        float count = 0.0f;
        for (int i = 1; i < curveNumberPoints; i++)
        {
            count += 1f / curveNumberPoints;
            m1 = Vector3.Lerp(startPosition, controlPoint, count);
            m2 = Vector3.Lerp(controlPoint, endPosition, count);
            Vector3 newPosition = Vector3.Lerp(m1, m2, count);
            path.Add(newPosition);
        }
        path.Add(endPosition);
    }
}