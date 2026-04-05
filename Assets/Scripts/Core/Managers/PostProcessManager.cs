using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PostProcessManager : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private Volume globalVolume;

    public PlayerEventsManager events;

    private ChromaticAberration _chromaticAberration;
    private float _defaultCAIntensity = 0f;

    [Header("PerfectDodge Properties")]
    [SerializeField] private float perfectDodgeFXIntensity = 1.0f;
    [SerializeField] private float perfectDodgeFXDuration = 0.2f;

    void Awake()
    {
        globalVolume = GetComponent<Volume>();

        if(globalVolume.profile.TryGet(out _chromaticAberration))
        {
            _defaultCAIntensity = _chromaticAberration.intensity.value;
        }
    }
    void OnEnable()
    {
        events.OnPerfectDodge += TriggerDodge;
    }
    void OnDisable()
    {
        events.OnPerfectDodge -= TriggerDodge;
    }

    void TriggerDodge()
    {
        //StopAllCoroutines();
        StartCoroutine(DodgeSequence(perfectDodgeFXIntensity, perfectDodgeFXDuration));
    }
    IEnumerator DodgeSequence(float targetIntensity, float duration)
    {
        // 1. 瞬間爆發 (爆點)
        _chromaticAberration.intensity.value = targetIntensity;

        // 2. 平滑回彈 (消散)
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // 核心公式：Mathf.Lerp(起始值, 終點值, 進度百分比)
            _chromaticAberration.intensity.value = Mathf.Lerp(targetIntensity, _defaultCAIntensity, elapsed / duration);
            yield return null;
        }
        // 3. 確保最後歸零
        _chromaticAberration.intensity.value = _defaultCAIntensity;
    }
}
