using UnityEngine;
using UnityEditor;

public class MissingReferencesDetector : EditorWindow
{
    [MenuItem("Window/Find Missing References")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(MissingReferencesDetector));
        window.maxSize = new Vector2(250, 100);
        window.minSize = window.maxSize;
        GUIContent guiContent = new GUIContent();
        guiContent.text = "Find Missing References";
        window.titleContent = guiContent;
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Find Missing References"))
        {
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();
            foreach(GameObject gameObject in gameObjects)
            {
                Component[] components = gameObject.GetComponents<Component>();
                foreach(Component component in components)
                {
                    SerializedObject serializedObject = new SerializedObject(component);
                    SerializedProperty serializedProperty = serializedObject.GetIterator();
                    while (serializedProperty.NextVisible(true))
                    {
                        if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            if(serializedProperty.objectReferenceValue == null)
                            {
                                Debug.Log("<color=red><b>Missing refernce : </b></color>" + 
                                    serializedProperty.displayName + " on " + 
                                    gameObject.name,
                                    gameObject
                                );
                            }
                        }
                    }
                }
            }
            EditorGUILayout.Space();
            Repaint();
        }
    }
}
