using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("SFX Audio Object Pool")]
    [SerializeField] private int sfxPoolSize = 10;

    public float GlobalBgmVolume { get; private set; } = 1.0f;
    public float GlobalSfxVolume { get; private set; } = 1.0f;

    private AudioSource bgmAudioSource;
    private List<AudioSource> sfxPool;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManager();
        }
        else Destroy(gameObject);
    }
    private void InitializeManager()
    {
        // 建立 bgm 專屬喇叭
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        bgmAudioSource.spatialBlend = 0f;

        // 建立 sfx 的物件池
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.spatialBlend = 0f;
            sfxPool.Add(sfxSource);
        }
    }

    #region 公開 API
    public void SetBgmVolume(float volume)
    {
        GlobalBgmVolume = Mathf.Clamp01(volume);
        // bgm需要即時更新
        bgmAudioSource.volume = Mathf.Clamp01(volume);
    }
    public void SetSfxVolume(float volume)
    {
        GlobalSfxVolume = Mathf.Clamp01(volume);
    }
    /// <summary> 播放音效 </summary>
    public void PlayAudioEvent(AudioEvent audioEvent, Vector3 position = default)
    {
        AudioSource source = GetAvailableSource();
        source.transform.position = position;

        audioEvent.Play(source);
    }
    #endregion
    
    private AudioSource GetAvailableSource()
    {
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying) return source;
        }

        // 若不夠用則動態生成
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.spatialBlend = 0f;
        sfxPool.Add(newSource);
        return newSource;
    }
}
