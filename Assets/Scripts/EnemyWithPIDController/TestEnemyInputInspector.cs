using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TestEnemyInput), true)]
[InitializeOnLoad]
public class TestEnemyInputInspector : Editor
{
    static TestEnemyInputInspector()
    {
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawHandles(TestEnemyInput enemyInput, GizmoType gizmoType)
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
