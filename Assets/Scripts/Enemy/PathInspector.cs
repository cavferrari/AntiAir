using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path), true)]
[InitializeOnLoad]
public class PathInspector : Editor
{
    static PathInspector()
    {
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(Path path, GizmoType gizmoType)
    {
        Handles.color = Color.green;
        for (int i = 0; i < path.GetPathCount() - 1; i++)
        {
            Handles.DrawLine(path.GetPathPoint(i), path.GetPathPoint(i + 1));
        }

        Handles.color = Color.yellow;
        Handles.DrawLine(path.GetPathPoint(0), path.GetEntryRunPosition());
        Handles.DrawLine(path.GetEntryRunPosition(), path.GetEndRunPosition());
        Handles.DrawLine(path.GetEndRunPosition(), path.GetEscapePosition());
        Handles.DrawLine(path.GetEscapePosition(), path.GetReturnPosition());
        Handles.DrawLine(path.GetReturnPosition(), path.GetPathPoint(path.GetPathCount() - 1));

        Gizmos.color = Color.green;
        for (int i = 0; i < path.GetPathCount(); i++)
        {
            Gizmos.DrawSphere(path.GetPathPoint(i), 0.5f);
        }

        if (path.GetPathCount() != 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(path.GetEntryRunRollPosition(), 1f);
            Gizmos.DrawSphere(path.GetEndRunRollPosition(), 1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(path.GetPathPoint(0), 1f);
            Gizmos.DrawSphere(path.GetEntryRunPosition(), 1f);
            Gizmos.DrawSphere(path.GetEndRunPosition(), 1f);
            Gizmos.DrawSphere(path.GetEscapePosition(), 1f);
            Gizmos.DrawSphere(path.GetReturnPosition(), 1f);
            Gizmos.DrawSphere(path.GetPathPoint(path.GetPathCount() - 1), 1f);
            Handles.Label(path.GetPathPoint(0), "Start Position");
            Handles.Label(path.GetEntryRunPosition(), "Entry Run Position");
            Handles.Label(path.GetEndRunPosition(), "End Run Position");
            Handles.Label(path.GetEscapePosition(), "Escape Position");
            Handles.Label(path.GetReturnPosition(), "Return Position");
            Handles.Label(path.GetPathPoint(path.GetPathCount() - 1), "Finish Position");
        }
    }
}