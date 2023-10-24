using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;

    [Header("Quality level Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Settings")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensivity Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider contollerSenSlider = null;


    [Header("Invert Y Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }
            if (PlayerPrefs.HasKey("MasterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);

            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");
                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;

                }
                else
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }

                if (PlayerPrefs.HasKey("masterBrightness"))
                {
                    float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                    brightnessSlider.value = localBrightness;
                    brightnessTextValue.text = localBrightness.ToString("0.0");
                }
                if (PlayerPrefs.HasKey("masterSen"))
                {
                    float localSensivity = PlayerPrefs.GetFloat("masterSen");
                    contollerSenSlider.value = localSensivity;
                    controllerSenTextValue.text = localSensivity.ToString("0");
                    menuController.mainControllerSen = Mathf.RoundToInt(localSensivity);
                }
                if (PlayerPrefs.HasKey("masterInvertY"))
                {
                    if (PlayerPrefs.GetInt("masterInverY")==1) 
                    {
                        invertYToggle.isOn = true;
                    }
                    else
                    {
                        invertYToggle.isOn = false;
                    }
                }

            }
        }
    }


}