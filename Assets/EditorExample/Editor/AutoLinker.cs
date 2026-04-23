using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoLinker : Editor
{
    static Dictionary<string, GameObject> hierarchyNameToGameObjectMap;
    static Dictionary<string, SerializedObject> inspectorFieldNameToSerializedPropertyMap;

    [InitializeOnLoadMethod]
    static void Setup()
    {
        hierarchyNameToGameObjectMap = new Dictionary<string, GameObject>();
        inspectorFieldNameToSerializedPropertyMap = new Dictionary<string, SerializedObject>();
        SetupHierarchyMap();
        SetupInspectorMap();
        HandleAutoLinking();
    }

    static void SetupHierarchyMap()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            string key = gameObject.name.ToLower().Replace(" ", "");
            if (hierarchyNameToGameObjectMap.ContainsKey(key))
                hierarchyNameToGameObjectMap.Add(key, gameObject);
        }
    }

    static void SetupInspectorMap()
    {
        foreach(GameObject gameObject in hierarchyNameToGameObjectMap.Values)
        {
            Component[] components = gameObject.GetComponents<Component>();
            foreach(Component component in components)
            {
                SerializedObject serializedObject = new SerializedObject(component);
                SerializedProperty serializedProperty = serializedObject.GetIterator();
                while (serializedProperty.NextVisible(true))
                {
                    string key = serializedProperty.displayName.ToLower().Replace(" ", "");
                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (!inspectorFieldNameToSerializedPropertyMap.ContainsKey(key))
                        {
                            inspectorFieldNameToSerializedPropertyMap.Add(key, serializedObject);
                        }
                    }

                }
            }
        }
    }

    static void HandleAutoLinking()
    {
        foreach(string name in inspectorFieldNameToSerializedPropertyMap.Keys)
        {
            string key = name.ToLower().Replace(" ", "");
            if (hierarchyNameToGameObjectMap.ContainsKey(key))
            {
                SerializedProperty serializedProperty = inspectorFieldNameToSerializedPropertyMap[key].FindProperty(key);
                serializedProperty.objectReferenceValue = hierarchyNameToGameObjectMap[key];
                serializedProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
