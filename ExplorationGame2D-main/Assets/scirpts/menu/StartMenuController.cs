using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isDarkMode = false;
    static List<GameObject> buttons;
    static string[] buttonNames;
    void Start()
    {
        // ["Play", "Load", "Options", "Credits", "Exit"],2
        if (buttons == null) buttons = new List<GameObject>();
        if(buttonNames == null) buttonNames = new string[] { "Play", "Load", "Options", "Credits", "Exit" };

        if (buttonNames.Contains(transform.name)) 
            buttons.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quitGame()
    {
        Application.Quit();
    }


}
