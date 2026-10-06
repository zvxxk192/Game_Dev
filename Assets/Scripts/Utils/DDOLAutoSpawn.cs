using UnityEngine;

public static class DDOLAutoSpawn
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeDDOLPrefabs()
    {
        DDOLConfig config = Resources.Load<DDOLConfig>("DDOLConfig");

        if (config == null)
        { 
            Debug.LogWarning("當前項目沒有配置 DDOLConfig");
            return;
        }

        foreach(GameObject prefab in config.DdolPrefabs)
        {
            if(prefab != null)
            {
                GameObject instance = Object.Instantiate(prefab);
                instance.name = prefab.name;
                Object.DontDestroyOnLoad(instance);
            }
        }
    }
}
