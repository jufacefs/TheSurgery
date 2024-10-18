using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveFileButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject emptyPanel;
    public GameObject savePanel;
    private bool isEmpty;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void savePlayerData(saveFlie f)
    {
        f.saveTime = System.DateTime.Now.ToString("o");
    }
}
