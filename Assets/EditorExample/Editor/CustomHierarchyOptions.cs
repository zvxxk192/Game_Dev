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
        AddInfoScriptToGameObject(id);
        DrawInfoButton(id, rect, "");
        DrawZoomInButton(id, rect, "Frame this gmae object");
        DrawPrefabButton(id, rect, "Save as prefab");
        //DrawDeleteButton(id, rect, "Delete this object");
    } 

    static Rect DrawRect(float x, float y, float size)
    {
        return new Rect(x, y, size, size);
    }

    static void DrawButtonWithToggle(int id, float x, float y, float size)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
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

    static void DrawButtonWithTexture(float x, float y, float size, string name, Action action, 
        GameObject gameObject, string tooltip)
    {
        if (gameObject != null)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fixedHeight = 0;
            guiStyle.fixedWidth = 0;
            guiStyle.stretchHeight = true;
            guiStyle.stretchWidth = true;
            Rect r = DrawRect(x, y, size);
            Texture t = Resources.Load(name) as Texture;
            GUIContent guiContent = new GUIContent();
            guiContent.image = t;
            guiContent.text = "";
            guiContent.tooltip = tooltip;
            bool isClicked = GUI.Button(r, guiContent, guiStyle);
            if (isClicked)
            {
                action.Invoke();
            }
        }
    }

    static void DrawInfoButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject != null)
        {
            bool hasInfoScriptComponent = gameObject.GetComponent<Info>();
            if (hasInfoScriptComponent)
            {
                Info infoScript = gameObject.GetComponent<Info>();
                if (infoScript != null)
                {
                    tooltip = infoScript.info;
                }
            }
        }
        DrawButtonWithTexture(rect.x + 150, rect.y - 1, 18, "info", () => { }, gameObject, tooltip);
    }

    static void AddInfoScriptToGameObject(int id)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject != null)
        {
            bool hasInfoScriptComponent = gameObject.GetComponent<Info>();
            if (!hasInfoScriptComponent)
            {
                gameObject.AddComponent<Info>();
            }
        }

    }

    static void DrawZoomInButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject != null)
        {
            DrawButtonWithTexture(rect.x + 175, rect.y + 2, 14, "zoom_in", () =>
            {
                Selection.activeGameObject = gameObject;
                SceneView.FrameLastActiveSceneView();
            }, gameObject, tooltip);
        }
    }

    static void DrawPrefabButton(int id, Rect rect, string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject != null)
        {
            DrawButtonWithTexture(rect.x + 198, rect.y, 18, "prefab", () =>
            {
                const string pathToPrefabsFolder = "Assets/Prefabs";
                bool doesPrefabsFolderExist = AssetDatabase.IsValidFolder(pathToPrefabsFolder);
                if (!doesPrefabsFolderExist)
                {
                    AssetDatabase.CreateFolder("Assets", "Prefabs");
                }
                string prefabName = gameObject.name + ".prefab";
                string prefabPath = pathToPrefabsFolder + "/" + prefabName;
                AssetDatabase.DeleteAsset(prefabName);
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
                EditorGUIUtility.PingObject(prefab);
            }, gameObject, tooltip);
        }
    }

    //static void DrawDeleteButton(int id, Rect rect, string tooltip)
    //{
    //    GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
    //    if (gameObject != null)
    //    {
    //        DrawButtonWithTexture(rect.x + 225, rect.y + 2, 14, "delete", () =>
    //        {
    //            UnityEngine.Object.DestroyImmediate(gameObject);
    //        }, gameObject, tooltip);
    //    }
    //}
}
