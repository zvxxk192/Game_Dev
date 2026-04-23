using UnityEngine;
using UnityEditor;

public class BatchRenameToolWindow : EditorWindow
{
    string batchName = "";
    string batchNumber = "";
    bool showOptions = true;

    [MenuItem("Window/Batch Rename")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(BatchRenameToolWindow));
        window.maxSize = new Vector2(500, 150);
        window.minSize = window.maxSize;
        GUIContent guiContent = new GUIContent();
        guiContent.text = "Batch Rename";
        window.titleContent = guiContent;
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Step 1 : Select objects in the hierarchy", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        GUIStyle guiStyle = new GUIStyle(EditorStyles.foldout);
        guiStyle.fontStyle = FontStyle.Bold;
        showOptions = EditorGUILayout.Foldout(showOptions, "Step 2 : Enter rename info", guiStyle);
        if (showOptions)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("\tEnter name for batch");
            batchName = EditorGUILayout.TextField(batchName);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("\tEnter starting number");
            batchNumber = EditorGUILayout.TextField(batchNumber);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Step 3 : Click the rename button", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("Rename"))
        {
            int numberAsInt = int.Parse(batchNumber);
            foreach (GameObject obj in Selection.objects)
            {
                obj.name = batchName + "_" + numberAsInt.ToString();
                numberAsInt++;
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
        Repaint();
    }
}
