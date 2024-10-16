using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public static string up1;
    public static string up2;
    public static string down1;
    public static string down2;
    public static string left1;
    public static string left2;
    public static string right1;
    public static string right2;
    public static string jump1;
    public static string jump2;
    public static string interact1;
    public static string interact2;
    public static string menuButton;

    public static Dictionary<string, string> controlKeys = new Dictionary<string, string> { 
        { "up1",""},
        { "up2",""},
        { "down1",""},
        { "down2",""},
        { "left1",""},
        { "left2",""},
        { "right1",""},
        { "right2",""},
        { "jump1",""},
        { "jump2",""},
        { "interact1",""},
        { "interact2",""},
        { "menuButton",""}
    };

    public bool isSettingKey = false;
    public string curSettingKey;
    public GameObject curKeyButton;

    private static Dictionary<string, (int, int)> resMap = new Dictionary<string, (int, int)> { 
        //{"1920*1080",(1920, 1080)},
        //{"1920*1080",(1920, 1080)},
        //{"1920*1080",(1920, 1080)},
        //{"1920*1080",(1920, 1080)},
        {"1920*1080",(1920, 1080)}};
    public GameObject languagePannel;
    public GameObject generalPanel;
    public GameObject controlsPanel;

    public Locale SimplifiedChinese;
    public Locale EnglishLocale;
    public Locale TraditionalChinese;
    


    void Start()
    {
        loadPref();

        
        updateScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSettingKey)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("the key is clicked while setting controls");
                foreach(KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if(Input.GetMouseButton(0) || Input.GetMouseButton(1)|| Input.GetMouseButton(2))
                    {
                        continue;
                    }
                    if (Input.GetKeyDown(keyCode))
                    {
                        if (controlKeys.ContainsValue(keyCode.ToString()) && controlKeys[curSettingKey] != keyCode.ToString())
                        {
                            continue;
                        }
                        controlKeys[curSettingKey] = keyCode.ToString();
                        setButtonText(curKeyButton, keyCode.ToString());
                        curKeyButton = null;
                        curSettingKey = "";
                        isSettingKey = false;
                        return;
                    }

                }
            }
        }
    }

    public static void setButtonText(GameObject button, string text)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"UpArrow","↑"},
            {"DownArrow","↓"},
            {"LeftArrow","←"},
            {"RightArrow","→"},
            {"Escape","Esc"},
            {"Return","Enter"},
            {"Comma",","},
            {"Minus","-"},
            {"Equals","="},
            {"LeftBracket","["},
            {"RightBracket","]"},
            {"Backslash","\\"},
            {"Semicolon",";"},
            {"Quote","\'"},
            {"Period","."},
            {"Slash","/"},
            {"BackQuote","`"},
            {"KeypadPlus","+"},
            {"KeypadMinus","Keypad-"},
            {"KeypadDivide","Keypad/"},
            {"KeypadMultiply","*"},
            {"KeypadPeriod","Keypad."},
            {"Delete","DEL"},
            {"Numlock","NUM"},
            {"PageDown","PGDN"},
            {"PageUp","PGUP"},

        };
        if(dict.ContainsKey(text))
        {
            button.GetComponentInChildren<TMP_Text>().text = dict[text];
        }
        else
        {
            button.GetComponentInChildren<TMP_Text>().text = text;
        }
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
        if (!isSettingKey)
        {
            savePref();
            applyPref();
            showGeneralPannel();
            gameObject.SetActive(false);
        }
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
    /// 设置语言
    /// </summary>
    private void savePref()
    {
        PlayerPrefs.SetInt("mute", isMute);
        PlayerPrefs.SetFloat("BGM",BGMVolume);
        PlayerPrefs.SetFloat("effects", EffectsVolume);
        PlayerPrefs.SetInt("fullScreen", isFullScreen);
        PlayerPrefs.SetString("resolution", curResolution);
        PlayerPrefs.SetString("language", curLanguage);
        PlayerPrefs.SetString("up1", controlKeys["up1"]);
        PlayerPrefs.SetString("up2", controlKeys["up2"]);
        PlayerPrefs.SetString("down1", controlKeys["down1"]);
        PlayerPrefs.SetString("down2", controlKeys["down2"]);
        PlayerPrefs.SetString("left1", controlKeys["left1"]);
        PlayerPrefs.SetString("left2", controlKeys["left2"]);
        PlayerPrefs.SetString("right1", controlKeys["right1"]);
        PlayerPrefs.SetString("right2", controlKeys["right2"]);
        PlayerPrefs.SetString("interact1", controlKeys["interact1"]);
        PlayerPrefs.SetString("interact2", controlKeys["interact2"]);
        PlayerPrefs.SetString("jump1", controlKeys["jump1"]);
        PlayerPrefs.SetString("jump2", controlKeys["jump2"]);
        PlayerPrefs.SetString("menuButton", controlKeys["menuButton"]);
    }

    private void applyPref()
    {
        //PlayerPrefs.SetInt("mute", isMute);
        //PlayerPrefs.SetFloat("BGM", BGMVolume);
        //PlayerPrefs.SetFloat("effects", EffectsVolume);
        //PlayerPrefs.SetInt("fullScreen", isFullScreen);
        //PlayerPrefs.SetString("resolution", curResolution);
    }

    private void loadPref()
    {
        isMute = PlayerPrefs.GetInt("mute", 0);
        BGMVolume = PlayerPrefs.GetFloat("BGM", 1);
        EffectsVolume = PlayerPrefs.GetFloat("effects", 1);
        isFullScreen = PlayerPrefs.GetInt("fullScreen", 1);
        curResolution = PlayerPrefs.GetString("resolution", "1920*1080");
        curLanguage = PlayerPrefs.GetString("language", "en");
        controlKeys["up1"] = PlayerPrefs.GetString("up1", "UpArrow");
        controlKeys["up2"] = PlayerPrefs.GetString("up2", "W");
        controlKeys["down1"] = PlayerPrefs.GetString("down1", "DownArrow");
        controlKeys["down2"] = PlayerPrefs.GetString("down2", "S");
        controlKeys["left1"] = PlayerPrefs.GetString("left1", "LeftArrow");
        controlKeys["left2"] = PlayerPrefs.GetString("left2", "A");
        controlKeys["right1"] = PlayerPrefs.GetString("right1", "RightArrow");
        controlKeys["right2"] = PlayerPrefs.GetString("right2", "D");
        controlKeys["interact1"] = PlayerPrefs.GetString("interact1", "F");
        controlKeys["interact2"] = PlayerPrefs.GetString("interact2", "E");
        controlKeys["jump1"] = PlayerPrefs.GetString("jump1", "X");
        controlKeys["jump2"] = PlayerPrefs.GetString("jump2", "Space");
        controlKeys["menuButton"] = PlayerPrefs.GetString("menuButton", "Escape");
    }

    public KeyCode s2code(string s)
    {
        if (System.Enum.TryParse(s, out KeyCode keyCode))
        {
            Debug.Log("转换成功，"+ s + "的KeyCode 为：" + keyCode);
            return keyCode;
        }
        else
        {
            Debug.LogWarning("转换失败，无效的 KeyCode 字符串"+s);
            return KeyCode.None;
        }
    }
    public void beginSetKey(GameObject button)
    {
        if(curKeyButton!=null && button == curKeyButton)
        {
            return;
        }
        if(isSettingKey)
        {
            setButtonText(curKeyButton, controlKeys[curSettingKey]);
        }
        setButtonText(button, "");
        string keyName = button.name;
        curKeyButton = button;
        curSettingKey = keyName;
        isSettingKey = true;
    }
}
