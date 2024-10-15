using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    public static float BGMVolume;
    public static float EffectsVolume;
    public static int isFullScreen;
    public static string curLanguage;
    public static int isMute;
    public static string curResolution;

    private static Dictionary<string, (int, int)> resMap = new Dictionary<string, (int, int)> { 
        {"1920*1080",(1920, 1080)},
        {"1920*1080",(1920, 1080)},
        {"1920*1080",(1920, 1080)},
        {"1920*1080",(1920, 1080)},
        {"1920*1080",(1920, 1080)}};
    public GameObject languagePannel;
    public GameObject generalPanel;
    public GameObject controlsPanel;

    public Locale SimplifiedChinese;
    public Locale EnglishLocale;
    public Locale TraditionalChinese;
    


    void Start()
    {
        curResolution = "1920*1080";
        isFullScreen = 1;
        curLanguage = "en";

        
        updateScreen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateScreen()
    {
        Screen.SetResolution(resMap[curResolution].Item1, resMap[curResolution].Item2, isFullScreen == 1 ? true:false);
    }

    public void setEnglish()
    {
        curLanguage = "en";
        LocalizationSettings.SelectedLocale = EnglishLocale;
    }

    public void setTraditionalChinese()
    {
        curLanguage = "hant";
        LocalizationSettings.SelectedLocale = TraditionalChinese;
    }

    public void setSimplifiedChinese()
    {
        curLanguage = "hans";
        LocalizationSettings.SelectedLocale = SimplifiedChinese;
    }

    public void showLanguagePannel()
    {
        languagePannel.SetActive(true);
        generalPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void showGeneralPannel()
    {
        languagePannel.SetActive(false);
        generalPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void showControlsPannel()
    {
        languagePannel.SetActive(false);
        generalPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void closeSetting()
    {
        
        showGeneralPannel();
        gameObject.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void changeFullScreen(bool fullScreen)
    {

    }
    public void changeFullScreen()
    {

    }
    /// <summary>
    /// …Ë÷√”Ô—‘
    /// </summary>
    private void savePref()
    {
        PlayerPrefs.SetInt("mute", isMute);
        PlayerPrefs.SetFloat("BGM",BGMVolume);
        PlayerPrefs.SetFloat("effects", EffectsVolume);
        PlayerPrefs.SetInt("fullScreen", isFullScreen);
        PlayerPrefs.SetString("resolution", curResolution);
        PlayerPrefs.SetString("language", curLanguage);
        PlayerPrefs.
    }

    private void loadPref()
    {

    }
}
