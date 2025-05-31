using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    ObjectPool<AudioSource> audioSourcePool;
    public AudioSource audioSourcePrefab;

    [Header("Audio Volume Settings")]
    public float effectVolume = 0.5f; // 기본 볼륨
    public float bgmVolume = 0.3f; // 배경 음악 볼륨

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogWarning("동일한 AudioManager 인스턴스가 이미 존재합니다. 중복 생성 방지.");
            Destroy(gameObject); // 이미 존재하는 인스턴스가 있다면 현재 오브젝트 삭제
            return;
        }
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 AudioManager 유지
    }


    void Start()
    {
        audioSourcePool = new ObjectPool<AudioSource>(
            createFunc: () =>
            {
                AudioSource source = Instantiate(audioSourcePrefab, transform);
                source.playOnAwake = false; // 자동 재생 방지
                return source;
            },
            actionOnGet: audioSource => audioSource.gameObject.SetActive(true),
            actionOnRelease: audioSource =>
            {
                audioSource.Stop(); // 반환 시 오디오 정지
                audioSource.clip = null; // 클립 참조 제거
                audioSource.gameObject.SetActive(false);
            },
            actionOnDestroy: audioSource => Destroy(audioSource.gameObject),
            maxSize: 100
        );
    }

    /// <summary>
    /// 들어온 오디오 클립을 재생합니다.
    /// <para>오디오 소스는 풀에서 가져와 사용 후 반환됩니다.</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    public static void PlaySound(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null. Cannot play sound.");
            return;
        }

        AudioSource source = Instance.audioSourcePool.Get();
        source.clip = clip;
        source.volume = Instance.effectVolume * volume;
        source.pitch = pitch;
        source.Play();
        Instance.StartCoroutine(Instance.ReturnAudioSourceWhenFinished(source));
    }

    private IEnumerator ReturnAudioSourceWhenFinished(AudioSource source)
    {
        // source.isPlaying이 false가 될 때까지 대기 (재생이 끝날 때까지)
        // 또는 source.clip이 null이 아니고, source.time이 source.clip.length보다 크거나 같을 때까지 대기
        // source.loop를 고려하지 않음
        yield return new WaitWhile(() => source.isPlaying);

        // 재생이 끝나면 풀에 반환
        if (source.gameObject.activeInHierarchy) // 아직 활성화 상태인지 확인 (중간에 비활성화될 경우 대비)
        {
            audioSourcePool.Release(source);
        }
    }
}
