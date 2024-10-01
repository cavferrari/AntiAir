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
        Handles.color = Color.yellow;
        for (int i = 0; i < path.GetPathCount() - 1; i++)
        {
            Handles.DrawLine(path.GetPathPoint(i), path.GetPathPoint(i + 1));
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < path.GetPathCount(); i++)
        {
            Gizmos.DrawSphere(path.GetPathPoint(i), 0.5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(path.GetEntryRunRollPosition(), 1f);
        Gizmos.DrawSphere(path.GetEndRunRollPosition(), 1f);

        Handles.Label(path.GetEntryRunPosition(), "Entry Run Position");
        Handles.Label(path.GetEndRunPosition(), "End Run Position");
        Handles.Label(path.GetEscapePosition(), "Escape Position");
        Handles.Label(path.GetReturnPosition(), "Return Position");
        Handles.Label(path.GetFinishPosition(), "Finish Position");
    }
}