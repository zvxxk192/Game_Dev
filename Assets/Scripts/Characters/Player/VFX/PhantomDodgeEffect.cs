using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhantomDodgeEffect : MonoBehaviour
{
    [Header("Phamtom Settings")]
    [SerializeField] private SkinnedMeshRenderer[] playerSMRList;
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private float fadeDuration = 2f;

    private PlayerEventsManager events;

    void Awake()
    {
        events = GetComponent<PlayerEventsManager>();
    }
    void OnEnable()
    {
        events.OnPerfectDodge += TriggerPhantom;
    }
    void OnDisable()
    {
        events.OnPerfectDodge -= TriggerPhantom;
    }

    void TriggerPhantom()
    {
        StartCoroutine(SpawnAndFadeGhost());
    }
    IEnumerator SpawnAndFadeGhost()
    {
        // 建立一個空的殘影物件，位置和旋轉與當前主角一模一樣
        GameObject ghostRoot = new GameObject("PhantomGhost_Root");
        ghostRoot.transform.position = transform.position;
        ghostRoot.transform.rotation = transform.rotation;

        List<Material> matInstances = new List<Material>();

        foreach(var smr in playerSMRList)
        {
            if(smr == null) continue;

            GameObject partObj = new GameObject(smr.gameObject.name + "_Ghost");
            partObj.transform.SetParent(ghostRoot.transform);

            partObj.transform.position = smr.transform.position;
            partObj.transform.rotation = smr.transform.rotation;

            // 加入網格和渲染組件
            MeshRenderer meshRenderer = partObj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = partObj.AddComponent<MeshFilter>();

            // 烘焙網格
            Mesh bakedMesh = new Mesh();
            smr.BakeMesh(bakedMesh);
            meshFilter.mesh = bakedMesh;

            // 實例化材質
            Material matInstance = new Material(ghostMaterial);
            meshRenderer.material = matInstance;
            matInstances.Add(matInstance);

            Destroy(bakedMesh, fadeDuration + 0.1f);
        }

        // 漸隱動畫
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            foreach(var mat in matInstances)
            {
                if (mat != null) mat.SetFloat("_Alpha", currentAlpha);
            }
            yield return null;
        }

        // 清理記憶體(目前先用 Destroy，未來階段 4 我們會無縫換成 Object Pool 回收)
        foreach (var mat in matInstances)
        {
            Destroy(mat);
        }
        Destroy(ghostRoot);
    }
}




