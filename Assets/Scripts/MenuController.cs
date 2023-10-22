using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour // hierarchy b�l�m�nde "MenuController"in i�inde yer al�yor
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;


    [Header("Confirmation")]
    [SerializeField] private GameObject comfirmationPromt = null;

    [Header("Levels To Load")]
    public string _newGameLevel; // oyun ba�lang�� ekran�m�z�n ad�
    private string levelToLoad; 
    [SerializeField] private GameObject noSavedGameDialog = null; // "noSavedGameDialogPanel" adl� gameobject'in a��lmas�n� sa�l�yor
    public void NewGameDialogYes() // new game olarak oyuna ba�lamay� sa�layan kod.
    {
        SceneManager.LoadScene(_newGameLevel);

    }
    public void LoadGameDialogYes() // load game olarak oyuna devam etmeyi sa�layan kod.
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
