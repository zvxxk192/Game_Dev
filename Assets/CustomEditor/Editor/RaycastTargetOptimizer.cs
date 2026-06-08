using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RaycastTargetOptimizer
{
    [MenuItem("Custom Tools/關閉選中物件及其子節點的所有 Raycast Target", false, 1)]
    static void DisableRaycasts(MenuCommand menuCommand)
    {
        GameObject root = Selection.activeGameObject;
        if (root == null) return;

        // 記錄 Undo，方便 Ctrl+Z 復原
        Undo.RegisterCompleteObjectUndo(root.GetComponentsInChildren<Graphic>(true), "Disable Raycast Targets");

        int count = 0;
        Graphic[] graphics = root.GetComponentsInChildren<Graphic>(true);
        foreach (Graphic g in graphics)
        {
            // 如果它身上沒有掛 Button、Toggle 這種需要點擊的組件，就把它關掉
            if (g.raycastTarget && g.GetComponent<Selectable>() == null)
            {
                g.raycastTarget = false;
                count++;
            }
        }

        Debug.Log($"[優化完成] 已將 {count} 個純裝飾性 UI 的 Raycast Target 關閉，效能提升！");
    }
}