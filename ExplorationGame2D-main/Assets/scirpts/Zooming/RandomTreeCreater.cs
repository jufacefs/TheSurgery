using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TreeEditor;
using UnityEngine;

public class RandomTreeCreater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    ZoomingController.TreeNode GenerateRootNode()
    {
        return null;
    }

    public void generateTree(string fileName)
    {
        string jsonText = LoadRandomDatatoString(fileName);

    }

    public string LoadRandomDatatoString(string fileName)
    {
        // 从 StreamingAssets 目录中读取 JSON 文件内容
        string jsonPath = ZoomingController.jsonPath;
        string path = Path.Combine(Application.dataPath, jsonPath);
        string _jsonPath = Path.Combine(path, $"{fileName}");
        if (File.Exists(_jsonPath))
        {
            string jsonText = File.ReadAllText(_jsonPath);
            //rootNode = JsonUtility.FromJson<TreeNodeWrapper>(jsonText).ToTreeNode();
            Debug.Log("Tree loaded successfully.");
            return jsonText;
        }
        else
        {
            Debug.LogWarning("Failed to load JSON file from StreamingAssets.");
            Debug.Log(_jsonPath);
            return  null;
        }
    }

    List<string> PickRandomName(List<string> nameList,List<float> chanceList,int cnt)
    {
        List<string> res = new List<string>();
        // 检查输入的列表是否有效
        if (nameList.Count != chanceList.Count)
        {
            throw new ArgumentException("nameList and chanceList must have the same length.");
        }

        // 计算总概率，确保它的和为 1（可能存在浮动，可以调整或标准化）
        float totalChance = 0f;
        foreach (float chance in chanceList)
        {
            totalChance += chance;
        }

        // 如果总概率不等于1，标准化
        if (Math.Abs(totalChance - 1f) > 0.01f)
        {
            Console.WriteLine("Warning: Total chance is not 1. Normalizing...");
            for (int i = 0; i < chanceList.Count; i++)
            {
                chanceList[i] /= totalChance;  // 归一化概率
            }
        }

        System.Random random = new System.Random();
        for (int i = 0; i < cnt; i++)
        {
            float randomValue = (float)random.NextDouble();
            float cumulativeChance = 0f;

            for (int j = 0; j < nameList.Count; j++)
            {
                cumulativeChance += chanceList[j];

                if (randomValue <= cumulativeChance)
                {
                    res.Add(nameList[j]);
                    break;
                }
            }
        }

        return res;
    }
}
