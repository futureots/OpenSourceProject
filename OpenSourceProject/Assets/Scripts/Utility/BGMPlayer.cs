using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    /// <summary> 배경 음악을 재생하는 AudioSource입니다.</summary>
    private AudioSource bgmSource;
    /// <summary> 배경 음악 클립입니다.</summary>
    [SerializeField] private AudioClip bgmClip;

    void Start()
    {
        bgmSource = gameObject.GetComponent<AudioSource>();

        PlayBGM();
    }

    private void PlayBGM()
    {
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.volume = AudioManager.Instance.bgmVolume; // AudioManager에서 설정한 배경 음악 볼륨 사용
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
}