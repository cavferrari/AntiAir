using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public float entryRunCurveRadius = 50f;
    public float endRunCurveRadius = 100f;
    public float returnCurveRadius = 100f;
    public int curveNumberPoints = 15;
    public float distanceEntryRunMax = 200f;
    public float distanceEntryRunMin = 100f;
    public float distanceEndRunMax = 50f;
    public float distanceEndRunMin = -45f;
    public float distanceEscapeFromBorder = 10f;
    public float distanceEscapeVerticalMax = 190f;
    public float distanceEscapeVerticalMin = 100f;

    private float horizontalBorder;
    private Vector3 returnPosition, finishPosition, entryRunPosition, endRunPosition, escapePosition;
    private Vector3 entryRunRollPosition, endRunRollPosition;
    private List<Vector3> path = new List<Vector3>();
    private int turnCounter;
    private int orientation;


    void Awake()
    {
        Vector2 screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.position.z));
        horizontalBorder = screenBounds.x;
        //topBorder = screenBounds.y;
    }

    public int GetPathCount()
    {
        return path.Count;
    }

    public Vector3 GetPathPoint(int index)
    {
        if (path.Count != 0)
        {
            return path[index];
        }
        else
        {
            return Vector3.zero;
        }
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

    public int GetOrientation()
    {
        return orientation;
    }

    public void Reset()
    {
        path.Clear();
        returnPosition = Vector3.zero;
        finishPosition = Vector3.zero;
        entryRunPosition = Vector3.zero;
        endRunPosition = Vector3.zero;
        escapePosition = Vector3.zero;
        entryRunRollPosition = Vector3.zero;
        endRunRollPosition = Vector3.zero;
        turnCounter = 0;
    }

    public bool IsTurningDown(Vector3 playerPosition)
    {
        bool isTurning = false;
        if (playerPosition.x * orientation >= entryRunRollPosition.x * orientation && playerPosition.x * orientation < endRunRollPosition.x * orientation && turnCounter == 0)
        {
            isTurning = true;
            turnCounter += 1;
        }
        return isTurning;
    }

    public bool IsTurningUp(Vector3 playerPosition)
    {
        bool isTurning = false;
        if (playerPosition.x * orientation >= endRunRollPosition.x * orientation && turnCounter == 1)
        {
            isTurning = true;
            turnCounter += 1;
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

        path.Add(playerPosition);

        float distanceEntryRun = Random.Range(distanceEntryRunMin, distanceEntryRunMax) * orientation;
        //Create attack curve
        entryRunPosition = CreateCurve(playerPosition,
                                       new Vector3(targetPosition.x - distanceEntryRun, playerPosition.y, playerPosition.z),
                                       targetPosition,
                                       entryRunCurveRadius);
        entryRunRollPosition = entryRunPosition - orientation * rollEntryDistance * Vector3.right;

        float distanceEscapeVertical = Random.Range(distanceEscapeVerticalMin, distanceEscapeVerticalMax);
        escapePosition = new Vector3(horizontalBorder * orientation - (distanceEscapeFromBorder * orientation), targetPosition.y + distanceEscapeVertical, playerPosition.z);
        float distanceEndRun = Random.Range(distanceEndRunMin, distanceEndRunMax);
        //create escape curve
        endRunPosition = CreateCurve(path.Last(),
                                     targetPosition - distanceEndRun * (targetPosition - path.Last()).normalized,
                                     escapePosition,
                                     endRunCurveRadius);
        endRunRollPosition = endRunPosition + orientation * rollEndDistance * Vector3.right;

        //create first return curve
        returnPosition = new Vector3((horizontalBorder * orientation) - (distanceEscapeFromBorder * orientation), escapePosition.y + returnCurveRadius * 2f, playerPosition.z);
        CreateCurve(path.Last(),
                    escapePosition,
                    returnPosition,
                    returnCurveRadius);
        //create second return curve
        CreateCurve(path.Last(),
                    returnPosition,
                    new Vector3(path.Last().x - (distanceEscapeFromBorder * 4f * orientation), returnPosition.y + returnCurveRadius / 10f, playerPosition.z),
                    returnCurveRadius);

        finishPosition = new Vector3(path.Last().x - (10f * orientation), path.Last().y, playerPosition.z);

        path.Add(finishPosition);
    }

    private Vector3 CreateCurve(Vector3 start, Vector3 middle, Vector3 end, float radius)
    {
        Vector3 curveStartPoint = GetCurveStartEndPoint(start, middle, end, radius, "Start");
        Vector3 curveEndPoint = GetCurveStartEndPoint(start, middle, end, radius, "End");
        AddCurvePoints(curveStartPoint, curveEndPoint, middle);
        return curveStartPoint;
    }

    private Vector3 GetCurveStartEndPoint(Vector3 pointA, Vector3 pointB, Vector3 pointC, float curveRadius, string type)
    {
        if (type.Equals("Start"))
        {
            Vector3 lineBA = pointB - pointA;
            return pointB - curveRadius * lineBA.normalized;
        }
        else if (type.Equals("End"))
        {
            Vector3 lineCB = pointC - pointB;
            return pointB + curveRadius * lineCB.normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void AddCurvePoints(Vector3 start, Vector3 end, Vector3 controlPoint)
    {
        path.Add(start);
        float count = 0.0f;
        Vector3 newPosition, m1, m2;
        for (int i = 1; i < curveNumberPoints; i++)
        {
            count += 1f / curveNumberPoints;
            m1 = Vector3.Lerp(start, controlPoint, count);
            m2 = Vector3.Lerp(controlPoint, end, count);
            newPosition = Vector3.Lerp(m1, m2, count);
            path.Add(newPosition);
        }
        path.Add(end);
    }
}