using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class saveController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void saveByJson(string saveFileName,object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            File.WriteAllText(path, json);

            Debug.Log("successfully saved data to " + path);
        } catch(System.Exception e)
        {
            Debug.LogError($"fail to saved date to {path}.\n{e}"); 
        }
    }

    public static T loadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception)
        {
            return default(T);
            throw;   
        }   
    }

    public static void deleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath,saveFileName);
        try
        {
            File.Delete(path);
        }catch(System.Exception e)
        {
            Debug.LogError($"fail to delete {path}.\n{e}");
        }
    }

    public saveFlie parsePlayerData()
    {
        saveFlie saveFlie = new saveFlie();


        return saveFlie;
    }
}
