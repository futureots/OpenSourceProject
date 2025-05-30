using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SoundSlider는 UI 슬라이더와 같이 배치하여 오디오 볼륨을 조절할 수 있는 컴포넌트입니다.
/// 저장도 같이 합니다.
/// </summary>
[RequireComponent(typeof(Slider))]
public class SoundSlider : MonoBehaviour
{
    public enum AudioType
    {
        Effect,
        BGM,
    }

    private Slider soundSlider;
    
    [Header("Audio Type")]
    [SerializeField] private AudioType audioType = AudioType.Effect;

    void Awake()
    {
        soundSlider = GetComponent<Slider>();
        soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);

        float volume = PlayerPrefs.GetFloat(audioType.ToString(), 0.5f);
        soundSlider.value = volume;

    }

    void OnEnable()
    {
        if (audioType == AudioType.Effect)
        {
            soundSlider.value = AudioManager.Instance.effectVolume;
        }
        else if (audioType == AudioType.BGM)
        {
            soundSlider.value = AudioManager.Instance.bgmVolume;
        }
    }

    private void OnSoundVolumeChanged(float value)
    {
        if (audioType == AudioType.Effect)
        {
            AudioManager.Instance.effectVolume = value;
        }
        else if (audioType == AudioType.BGM)
        {
            AudioManager.Instance.bgmVolume = value;
        }
        PlayerPrefs.SetFloat(audioType.ToString(), value); // 배경 음악 볼륨 저장
        PlayerPrefs.Save(); // 변경 사항 저장
    }
}
