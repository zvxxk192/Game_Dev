using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseView : MonoBehaviour
{
    protected VisualElement Root;
    protected bool IsInitialized { get; private set; }

    [SerializeField] protected string containerName;

    protected virtual void OnEnable()
    {
        TryInitialize();
    }
    protected virtual void OnDisable()
    {
        if (IsInitialized)
        {
            OnUnregisterEvents();
            IsInitialized = false;
        }
    }
    protected virtual void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        if (IsInitialized) return;

        var uiDoc = GetComponentInParent<UIDocument>();
        if (uiDoc == null || uiDoc.rootVisualElement == null) return;

        // 如果有指定名稱就找特定 Container，否則用 Root
        Root = string.IsNullOrEmpty(containerName)
            ? uiDoc.rootVisualElement
            : uiDoc.rootVisualElement.Q<TemplateContainer>(containerName);

        if (Root != null)
        {
            OnBindElements();      // 1. 抓 DOM 節點
            OnRegisterEvents();    // 2. 綁定事件
            OnPostInitialize();    // 3. 初始化後的自定義邏輯
            IsInitialized = true;
        }
    }

    protected abstract void OnBindElements();
    protected abstract void OnRegisterEvents();
    protected abstract void OnUnregisterEvents();
    protected virtual void OnPostInitialize() { }
}
