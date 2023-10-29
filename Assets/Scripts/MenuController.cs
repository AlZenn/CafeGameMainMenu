using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour // hierarchy bölümünde "MenuController"in içinde yer alýyor
{
    // Ses ayarlarýný barýndýrýr
    [Header("Volume Settings")] 
    [SerializeField] private TMP_Text volumeTextValue = null;         // Ses slider'ýnýn deðerini içinde tutar
    [SerializeField] private Slider volumeSlider = null;              // ses slider'ý
    [SerializeField] private float defaultVolume = 1.0f;              // varsayýlan ses

    // oyun oynayýþ ayarlarýný barýndýrýr
    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;  // fare hýzý deðerini içinde tutar
    [SerializeField] private Slider controllerSenSlider = null;       // fare hýzý slider'ý
    [SerializeField] private int defaultSen = 4;                      // varsayýlan fare hýzý
    public int mainControllerSen = 4;                                 

    // fareyi tersine çevirme ayarlarýný barýndýrýr
    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;              // fareyi tersine çevirme butonunu içinde barýndýrýr

    // grafik ayarlarýný barýndýrýr
    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;           // parlaklýk slider'ýný içinde tutar
    [SerializeField] private TMP_Text brightnessTextValue = null;      // parlaklýk slider'ý deðerini içinde tutar
    [SerializeField] private float defaultBrightness = 1;              // varsayýlan parlaklýk ayarý

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;             // kalite ayarý
    [SerializeField] private Toggle fullScreenToggle;                  // tam ekran açýk mý kapalý mý ayarý

    // alttaki 3 bölümde otomatik kalite ve tam ekran ayarý için gerekli deðerler var.
    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    // oyunumuzun ayarýný kaydettikten sonra 2 saniyelik bir image çýkartýr ve kapatýr
    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPromt = null;

    // oyuna baþlama ayarlarýný barýndýrýr
    [Header("Levels To Load")]
    public string _newGameLevel;                                       // oyun baþlangýç ekranýmýzýn adý
    private string levelToLoad;  
    [SerializeField] private GameObject noSavedGameDialog = null;      // "noSavedGameDialogPanel" adlý gameobject'in açýlmasýný saðlýyor

    // grafik ayarlarýný barýndýrýr
    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionsDropdown;                            // oyundaki resolationlarý otomatik olarak oyuna aktarýyor (el ile deðil kod ile yapýldý.)
    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();                      // ekran boyutlarýný ayarlamak yerine koddan atadým

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


    public void NewGameDialogYes()                                      // new game olarak oyuna baþlamayý saðlayan kod.
    {
        SceneManager.LoadScene(_newGameLevel);
    }
    public void LoadGameDialogYes()                                     // load game olarak oyuna devam etmeyi saðlayan kod, oyunda kayýtlý dosya varsa baþlar yoksa hata dialogunu aktif eder
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

    public void VolumeApply()                                               // ayarladýðýmýz oyun sesini kaydeder, prefsde deðiþtirir
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetControllerSen(float sensitivity)                         // fare hýzýný ayarlamaya yarar
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");

    }

    public void GameplayApply()                                              // fare kontrollerini kaydeder, prefsde deðiþtirir
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

    public void SetBrightness(float brightness)                                 // parlaklýk ayarýný yapmamýza yarayan kod
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)                                // fullscreen aktif mi olacak deaktif mi deðiþiklik yapmamýza yarayan kod
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)                                    // oyun grafik kalitesini deðiþtirmeye yarayan kod
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()                                                 // oyun grafik ayarýný kaydeder (parlaklýk, kalite, tam ekran)
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        // change your brightness with your proccesing

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string menuType)                                    // oyunu default ayarlara geri çevirir
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

            // oyunun kalite ve grafik ayarýný yapar
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width,currentResolution.height, Screen.fullScreen);
            resolutionsDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (menuType == "Audio") // varsayýlan ses ayarlarý
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (menuType=="Gameplay") // varsayýlan gameplay ayarlarý
        {
            controllerSenTextValue.text = defaultSen.ToString("0");
            controllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }

    }

    public IEnumerator ConfirmationBox()                                // sol altta oyunumuzu kaydettiðimizde çýkan image'ý kontrol eder
    {
        comfirmationPromt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPromt.SetActive(false);
    }



}
