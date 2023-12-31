using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    public Toggle toggleFullScreen;

    private bool activeLocalSelector = false;
    private void Start()
    {
        StartResolution();
        StartFullscreen();

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    #region Resolution
    private void StartResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        //int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            //if (resolutions[i].width == Screen.currentResolution.width &&
            //    resolutions[i].height == Screen.currentResolution.height)
            //{
            //    currentResolutionIndex = i;
            //}
        }
        resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.value = PlayerPrefs.GetInt("currentResolutionIndex");
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution()
    {
        Resolution resolution = resolutions[resolutionDropdown.value];
        PlayerPrefs.SetInt("currentResolutionIndex", resolutionDropdown.value);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    #endregion

    #region Fullscreen
    public void StartFullscreen()
    {
        Screen.fullScreen = (PlayerPrefs.GetInt("fullscreenValue") > 0 ? true : false);
    }
    public void SetFullscreen()
    {
        Screen.fullScreen = toggleFullScreen.isOn;
        PlayerPrefs.SetInt("fullscreenValue", (toggleFullScreen.isOn ? 1 : 0));
    }
    #endregion

    #region Sound
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
    #endregion
}
