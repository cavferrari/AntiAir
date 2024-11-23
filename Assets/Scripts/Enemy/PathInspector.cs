using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(FlyPath), true)]
[InitializeOnLoad]
public class PathInspector : Editor
{
    static PathInspector()
    {
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(FlyPath flyPath, GizmoType gizmoType)
    {
        Handles.color = Color.green;
        for (int i = 0; i < flyPath.GetPathCount() - 1; i++)
        {
            Handles.DrawLine(flyPath.GetPathPoint(i), flyPath.GetPathPoint(i + 1));
        }

        Handles.color = Color.yellow;
        Handles.DrawLine(flyPath.GetPathPoint(0), flyPath.GetEntryRunPosition());
        Handles.DrawLine(flyPath.GetEntryRunPosition(), flyPath.GetEndRunPosition());
        Handles.DrawLine(flyPath.GetEndRunPosition(), flyPath.GetEscapePosition());
        Handles.DrawLine(flyPath.GetEscapePosition(), flyPath.GetReturnPosition());
        Handles.DrawLine(flyPath.GetReturnPosition(), flyPath.GetPathPoint(flyPath.GetPathCount() - 1));

        Gizmos.color = Color.green;
        for (int i = 0; i < flyPath.GetPathCount(); i++)
        {
            Gizmos.DrawSphere(flyPath.GetPathPoint(i), 0.5f);
        }

        if (flyPath.GetPathCount() != 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(flyPath.GetEntryRunRollPosition(), 1f);
            Gizmos.DrawSphere(flyPath.GetEndRunRollPosition(), 1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(flyPath.GetPathPoint(0), 1f);
            Gizmos.DrawSphere(flyPath.GetEntryRunPosition(), 1f);
            Gizmos.DrawSphere(flyPath.GetEndRunPosition(), 1f);
            Gizmos.DrawSphere(flyPath.GetEscapePosition(), 1f);
            Gizmos.DrawSphere(flyPath.GetReturnPosition(), 1f);
            Gizmos.DrawSphere(flyPath.GetPathPoint(flyPath.GetPathCount() - 1), 1f);
            Handles.Label(flyPath.GetPathPoint(0), "Start Position");
            Handles.Label(flyPath.GetEntryRunPosition(), "Entry Run Position");
            Handles.Label(flyPath.GetEndRunPosition(), "End Run Position");
            Handles.Label(flyPath.GetEscapePosition(), "Escape Position");
            Handles.Label(flyPath.GetReturnPosition(), "Return Position");
            Handles.Label(flyPath.GetPathPoint(flyPath.GetPathCount() - 1), "Finish Position");
        }
    }
}
#endif