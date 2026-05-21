using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ProjectOrganizerWindow : EditorWindow
{
    int selectedTabIndex = 0;
    string[] tabs = { "Orgainzer", "Asset Type Mappings" };
    int countOfAssetTypeRows;
    List<AssetTypeRow> assetTypeRows;
    int totalNumberOfFileExtensions;
    bool isDirty = false;
    string[] assetTypeNames;
    int countOfOrganizerRows;
    List<OrganizerRow> organizerRows;

    class OrganizerRow
    {
        public int selectedOptionIndex;
        public string folderPath;
        public Object obj;
    }

    class AssetTypeRow
    {
        public string name;
        public string fileExtension;
    }

    Dictionary<string, List<string>> assetTypes = new Dictionary<string, List<string>>()
    {
        { "Prefabs", new List<string>(){ ".prefabs" } },
        { "Animations", new List<string>(){ ".anim" } },
        { "Image", new List<string>(){ ".png", ".jpeg" } }
    };

    void Awake()
    {
        InitializeFields();
    }

    [MenuItem("Custom Tools/Project Organizer Tool")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(ProjectOrganizerWindow));
        GUIContent guiContent = new GUIContent();
        guiContent.text = "Project Organizer Tool";
        window.titleContent = guiContent;
        window.Show();
    }

    void OnGUI()
    {
        DrawToolbarTabs();
        EditorGUILayout.Space(20);
        if (selectedTabIndex == 0)
        {
            if (isDirty)
            {
                isDirty = false;
                UpdateAssetTypes(assetTypeNames.Length);
            }
            for (int i = 0; i < countOfOrganizerRows; i++)
            {
                DrawOrganizerRow(i);
            }

            DrawAddAndRemoveControls();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Organize"))
            {
                OrganizeFilesIntoFolders();
            }
        }
        else
        {
            for (int i = 0; i < countOfAssetTypeRows; i++)
            {
                DrawAssetTypeRow(i);
            }

            DrawAddAndRemoveControls();
        }
    }

    void DrawToolbarTabs()
    {
        GUILayout.BeginHorizontal();
        selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, tabs);
        GUILayout.EndHorizontal();
    }

    void DrawAssetTypeRow(int currentIndex)
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Name");
        EditorGUI.BeginChangeCheck();
        if (assetTypeRows != null)
        {
            assetTypeRows[currentIndex].name = EditorGUILayout.TextField(assetTypeRows[currentIndex].name);
        }
        if (EditorGUI.EndChangeCheck())
        {
            isDirty = true;
        }
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("File Extension");
        EditorGUI.BeginChangeCheck();
        if (assetTypeRows != null)
        {
            assetTypeRows[currentIndex].fileExtension = EditorGUILayout.TextField(assetTypeRows[currentIndex].fileExtension);
        }
        if (EditorGUI.EndChangeCheck() && assetTypes.ContainsKey(assetTypeRows[currentIndex].name))
        {
            isDirty = true;
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    void InitializeFields()
    {
        foreach (string key in assetTypes.Keys)
        {
            totalNumberOfFileExtensions += assetTypes[key].Count;
        }

        countOfAssetTypeRows = totalNumberOfFileExtensions;

        assetTypeRows = new List<AssetTypeRow>();

        assetTypeNames = new string[totalNumberOfFileExtensions];
        assetTypes.Keys.CopyTo(assetTypeNames, 0);

        for (int i = 0; i < totalNumberOfFileExtensions; i++)
        {
            string key = assetTypeNames[i];
            if (key != null)
            {
                int numberOfFileExtensionForAssetType = assetTypes[key].Count;
                for (int j = 0; j < numberOfFileExtensionForAssetType; j++)
                {
                    assetTypeRows.Add(new AssetTypeRow()
                    {
                        name = assetTypeNames[i],
                        fileExtension = assetTypes[assetTypeNames[i]][j]
                    });
                }
            }
        }

        countOfOrganizerRows = assetTypes.Keys.Count;
        organizerRows = new List<OrganizerRow>();
        for (int i = 0; i < countOfOrganizerRows; i++)
        {
            organizerRows.Add(new OrganizerRow()
            {
                selectedOptionIndex = i,
                folderPath = "Assets/" + assetTypeNames[i]
            });
        }
    }

    void UpdateAssetTypes(int currentIndex)
    {
        assetTypes.Add(assetTypeRows[currentIndex].name, new List<string>() { });
        assetTypes[assetTypeRows[currentIndex].name].Add(assetTypeRows[currentIndex].fileExtension);
        totalNumberOfFileExtensions = 0;
        foreach (string key in assetTypes.Keys)
        {
            totalNumberOfFileExtensions += assetTypes[key].Count;
        }
        assetTypeNames = new string[totalNumberOfFileExtensions - 1];
        assetTypes.Keys.CopyTo(assetTypeNames, 0);
    }

    void DrawOrganizerRow(int currentIndex)
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Asset Type");
        EditorGUI.BeginChangeCheck();
        organizerRows[currentIndex].selectedOptionIndex = EditorGUILayout.Popup(
            "",
            organizerRows[currentIndex].selectedOptionIndex,
            assetTypeNames
        );
        if (EditorGUI.EndChangeCheck())
        {
            organizerRows[currentIndex].folderPath = "Assets/" + assetTypeNames[organizerRows[currentIndex].selectedOptionIndex];
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path to Folder");
        organizerRows[currentIndex].folderPath = EditorGUILayout.TextField(
            organizerRows[currentIndex].folderPath
        );
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Select Folder");
        EditorGUI.BeginChangeCheck();
        organizerRows[currentIndex].obj = EditorGUILayout.ObjectField(
            organizerRows[currentIndex].obj,
            typeof(UnityEditor.DefaultAsset),
            true
        );
        if (EditorGUI.EndChangeCheck())
        {
            organizerRows[currentIndex].folderPath = "Assets/" + organizerRows[currentIndex].obj.name;
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    void DrawAddAndRemoveControls()
    {
        GUILayout.BeginHorizontal();
        for(int i = 0; i < 12; i++)
        {
            EditorGUILayout.Space();
        }
        GUIContent add = new GUIContent();
        add.text = "+";
        if (GUILayout.Button(add))
        {
            if (selectedTabIndex == 0)
            {
                countOfOrganizerRows++;
                organizerRows.Add(new OrganizerRow());
            }
            else
            {
                countOfAssetTypeRows++;
                assetTypeRows.Add(new AssetTypeRow());
            }
        }
        GUIContent remove = new GUIContent();
        remove.text = "-";
        if (GUILayout.Button(remove))
        {
            if (selectedTabIndex == 0)
            {
                countOfOrganizerRows--;
                organizerRows.RemoveAt(organizerRows.Count - 1);
            }
            else
            {
                countOfAssetTypeRows--;
                assetTypeRows.RemoveAt(assetTypeRows.Count - 1);
            }
        }
        GUILayout.EndHorizontal();
    }

    void OrganizeFilesIntoFolders()
    {
        Dictionary<string, string> fileExtensionsToFolderPathsMap = new Dictionary<string, string>();
        foreach(string assetTypeName in assetTypes.Keys)
        {
            for(int i =  0; i < assetTypes[assetTypeName].Count; i++)
            {
                string folderPath = "Assets/" + assetTypeName + "/";
                fileExtensionsToFolderPathsMap.Add(assetTypes[assetTypeName][i], folderPath);

            }
        }

        DirectoryInfo dir = new DirectoryInfo("Assets/");
        foreach(string fileExtension in fileExtensionsToFolderPathsMap.Keys)
        {
            string query = "*" + fileExtension;
            FileInfo[] info = dir.GetFiles(query);
            foreach(FileInfo file in info)
            {
                string filePath = fileExtensionsToFolderPathsMap[fileExtension] + file.Name;
                AssetDatabase.MoveAsset("Assets/" + file.Name, filePath);
            }
        }
    }
}