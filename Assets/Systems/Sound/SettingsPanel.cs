using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    [Header("UI Toggles")]
    public Toggle soundsToggle;
    public Toggle musicToggle;

    [Header("Audio")]
    public AudioMixer audioMixer;

    private const string KEY_SOUNDS = "Settings_Sounds";
    
    private const string KEY_MUSIC = "Settings_Music";

    private void Start()
    {
        soundsToggle.onValueChanged.AddListener(OnSoundsToggleChanged);
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        SetTogglesValue();
        gameObject.SetActive(false);
    } 

    private void OnEnable()
    {
        SetTogglesValue();
    }

    private void SetTogglesValue()
    {
        bool soundsOn = PlayerPrefs.GetInt(KEY_SOUNDS, 1) == 1;
        bool musicOn = PlayerPrefs.GetInt(KEY_MUSIC, 1) == 1;

        soundsToggle.isOn = soundsOn;
        musicToggle.isOn = musicOn;
        SetSoundsVolume(soundsOn);
        SetMusicVolume(musicOn);
    }

    private void OnDestroy()
    {
        soundsToggle.onValueChanged.RemoveListener(OnSoundsToggleChanged);
        musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
    }

    private void OnSoundsToggleChanged(bool isOn)
    {
        SetSoundsVolume(isOn);
        PlayerPrefs.SetInt(KEY_SOUNDS, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        SetMusicVolume(isOn);
        PlayerPrefs.SetInt(KEY_MUSIC, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetSoundsVolume(bool isOn)
    {
        audioMixer.SetFloat("Sounds", isOn ? 0f : -80f);
    }

    private void SetMusicVolume(bool isOn)
    {
        audioMixer.SetFloat("Music", isOn ? 0f : -80f);
    }
}
