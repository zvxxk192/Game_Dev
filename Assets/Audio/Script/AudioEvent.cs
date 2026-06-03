using UnityEngine;

[CreateAssetMenu(fileName = "AudioEvent", menuName = "Scriptable Objects/Audio/AudioEvent")]
public class AudioEvent : ScriptableObject
{
    public AudioClip[] clips;
    public AudioCategory category;

    [SerializeField] private bool isRandomPitch = false;

    [Range(0f, 1f)] public float baseVolume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;

    public void Play(AudioSource source)
    {
        if (clips.Length == 0) return;

        source.spatialBlend = (category == AudioCategory.World_3D) ? 1 : 0;
        source.loop = (category == AudioCategory.BGM);

        if (isRandomPitch) pitch = Random.Range(0.8f, 1.2f);
        source.pitch = pitch;
        source.clip = (clips.Length > 1) ? clips[Random.Range(0, clips.Length)] : clips[0];


        if (category == AudioCategory.BGM)
        {
            source.volume = baseVolume * AudioManager.Instance.GlobalBgmVolume;
            source.Play();
        }
        else
        { 
            source.volume = baseVolume * AudioManager.Instance.GlobalSfxVolume;
            source.PlayOneShot(source.clip);
        }
    }
}
