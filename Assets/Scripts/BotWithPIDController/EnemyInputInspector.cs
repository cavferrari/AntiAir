using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyInput), true)]
[InitializeOnLoad]
public class EnemyInputInspector : Editor
{
    static EnemyInputInspector()
    {
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(EnemyInput enemyInput, GizmoType gizmoType)
    {
        Handles.color = Color.red;
        for (int i = 0; i < enemyInput.GetPathCount() - 1; i++)
        {
            Handles.DrawLine(enemyInput.GetPathPoint(i), enemyInput.GetPathPoint(i + 1));
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < enemyInput.GetPathCount(); i++)
        {
            Gizmos.DrawSphere(enemyInput.GetPathPoint(i), 0.1f);
        }
    }
}
