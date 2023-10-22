using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour // hierarchy bölümünde "MenuController"in içinde yer alýyor
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;


    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPromt = null;

    [Header("Levels To Load")]
    public string _newGameLevel; // oyun baþlangýç ekranýmýzýn adý
    private string levelToLoad; 
    [SerializeField] private GameObject noSavedGameDialog = null; // "noSavedGameDialogPanel" adlý gameobject'in açýlmasýný saðlýyor
    public void NewGameDialogYes() // new game olarak oyuna baþlamayý saðlayan kod.
    {
        SceneManager.LoadScene(_newGameLevel);

    }
    public void LoadGameDialogYes() // load game olarak oyuna devam etmeyi saðlayan kod.
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

    public void ExitButton() // oyunu kapatma kodu
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string menuType)
    {
        if (menuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

    }

    public IEnumerator ConfirmationBox()
    {
        comfirmationPromt.SetActive(true);
        yield return new WaitForSeconds(2);
        comfirmationPromt.SetActive(false);
    }



}
