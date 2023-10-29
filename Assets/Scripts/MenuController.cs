using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour // hierarchy b�l�m�nde "MenuController"in i�inde yer al�yor
{
    // Ses ayarlar�n� bar�nd�r�r
    [Header("Volume Settings")] 
    [SerializeField] private TMP_Text volumeTextValue = null;         // Ses slider'�n�n de�erini i�inde tutar
    [SerializeField] private Slider volumeSlider = null;              // ses slider'�
    [SerializeField] private float defaultVolume = 1.0f;              // varsay�lan ses

    // oyun oynay�� ayarlar�n� bar�nd�r�r
    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;  // fare h�z� de�erini i�inde tutar
    [SerializeField] private Slider controllerSenSlider = null;       // fare h�z� slider'�
    [SerializeField] private int defaultSen = 4;                      // varsay�lan fare h�z�
    public int mainControllerSen = 4;                                 

    // fareyi tersine �evirme ayarlar�n� bar�nd�r�r
    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;              // fareyi tersine �evirme butonunu i�inde bar�nd�r�r

    // grafik ayarlar�n� bar�nd�r�r
    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;           // parlakl�k slider'�n� i�inde tutar
    [SerializeField] private TMP_Text brightnessTextValue = null;      // parlakl�k slider'� de�erini i�inde tutar
    [SerializeField] private float defaultBrightness = 1;              // varsay�lan parlakl�k ayar�

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;             // kalite ayar�
    [SerializeField] private Toggle fullScreenToggle;                  // tam ekran a��k m� kapal� m� ayar�

    // alttaki 3 b�l�mde otomatik kalite ve tam ekran ayar� i�in gerekli de�erler var.
    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    // oyunumuzun ayar�n� kaydettikten sonra 2 saniyelik bir image ��kart�r ve kapat�r
    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPromt = null;

    // oyuna ba�lama ayarlar�n� bar�nd�r�r
    [Header("Levels To Load")]
    public string _newGameLevel;                                       // oyun ba�lang�� ekran�m�z�n ad�
    private string levelToLoad;  
    [SerializeField] private GameObject noSavedGameDialog = null;      // "noSavedGameDialogPanel" adl� gameobject'in a��lmas�n� sa�l�yor

    // grafik ayarlar�n� bar�nd�r�r
    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionsDropdown;                            // oyundaki resolationlar� otomatik olarak oyuna aktar�yor (el ile de�il kod ile yap�ld�.)
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();                      // ekran boyutlar�n� ayarlamak yerine koddan atad�m

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)                         
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    }


    public void NewGameDialogYes()                                      // new game olarak oyuna ba�lamay� sa�layan kod.
    {
        SceneManager.LoadScene(_newGameLevel);
    }
    public void LoadGameDialogYes()                                     // load game olarak oyuna devam etmeyi sa�layan kod, oyunda kay�tl� dosya varsa ba�lar yoksa hata dialogunu aktif eder
    {
        if (PlayerPrefs.HasKey("Savedlevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }

    }
     
    public void ExitButton()                                                // oyunu kapatma kodu
    {
        Application.Quit();
    }

    public void SetVolume(float volume)                                     // oyun sesini ayarlayan kod
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()                                               // ayarlad���m�z oyun sesini kaydeder, prefsde de�i�tirir
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)                         // fare h�z�n� ayarlamaya yarar
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");

    }

    public void GameplayApply()                                              // fare kontrollerini kaydeder, prefsde de�i�tirir
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            // invert y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            // not invert
        }

        PlayerPrefs.SetFloat("masterSen",mainControllerSen);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)                                 // parlakl�k ayar�n� yapmam�za yarayan kod
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)                                // fullscreen aktif mi olacak deaktif mi de�i�iklik yapmam�za yarayan kod
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)                                    // oyun grafik kalitesini de�i�tirmeye yarayan kod
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()                                                 // oyun grafik ayar�n� kaydeder (parlakl�k, kalite, tam ekran)
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        // change your brightness with your proccesing

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string menuType)                                    // oyunu default ayarlara geri �evirir
    {
        if (menuType =="Graphics")
        {
            // reset brightness value
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            // quality reset
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            // fullscreen reset
            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            // oyunun kalite ve grafik ayar�n� yapar
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width,currentResolution.height, Screen.fullScreen);
            resolutionsDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (menuType == "Audio") // varsay�lan ses ayarlar�
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (menuType=="Gameplay") // varsay�lan gameplay ayarlar�
        {
            controllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }

    }

    public IEnumerator ConfirmationBox()                                // sol altta oyunumuzu kaydetti�imizde ��kan image'� kontrol eder
    {
        comfirmationPromt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPromt.SetActive(false);
    }



}
