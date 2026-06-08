using UnityEditor;
using UnityEngine;
using System;

[InitializeOnLoad]
public class CustomHierarchyOptions
{
    static CustomHierarchyOptions()
    {
        EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowItemOnGUI;
    }

    static void hierarchyWindowItemOnGUI(int id, Rect rect)
    {
        DrawActiveToggleButton(id, rect);
    }

    static Rect DrawRect(float x, float y, float size)
    {
        return new Rect(x, y, size, size);
    }

    static void DrawButtonWithToggle(int id, float x, float y, float size)
    {
        GameObject gameObject = EditorUtility.EntityIdToObject(id) as GameObject;
        if (gameObject != null)
        {
            Rect r = DrawRect(x, y, size);
            gameObject.SetActive(GUI.Toggle(r, gameObject.activeSelf, string.Empty));
        }
    }

    static void DrawActiveToggleButton(int id, Rect rect)
    {
        DrawButtonWithToggle(id, rect.x - 28, rect.y + 3, 10);
    }
}