using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TreeEditor;
using UnityEngine;

public class RandomTreeCreater : MonoBehaviour
{
    // Start is called before the first frame update
    public static ZoomingController.TreeNode curNode = null;
    public static Root root;
    public static int depthLimit = 100;
    public static int depth = 0;
    public static List<(float, float)> randomPos = new List<(float, float)> ()
    {
        (0f,0f),

        (-0.3f,0.3f),
        (0.3f,0.3f),
        (-0.3f,-0.3f),
        (0.3f,-0.3f),

        (0.5f,-0.2f),
        (-0.5f,-0.2f),
        (0.5f,0.2f),
        (-0.5f,0.2f),

        (0.2f,-0.5f),
        (-0.2f,-0.5f),
        (0.2f,0.5f),
        (-0.2f,0.5f),
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ZoomingController.TreeNode generateTree(string fileName,string parentName)
    {
        string jsonText = LoadRandomDatatoString(fileName);
        root = JsonUtility.FromJson<Root>(jsonText);
        curNode = null;
        ZoomingController.TreeNode rootNode = null;
        for (int i = 0; i < root.myGroups.Count; i += 1)
        {
            if (root.myGroups[i].name == parentName)
            {
                Child c = root.myGroups[i].children[0];
                List<(float, float)> RootUsedPos = new List<(float, float)> ();
                depth = 0;
                rootNode = GenerateTreeFromChild(c,RootUsedPos);
                break;
            }
        }
        return rootNode;
    }

    static ZoomingController.TreeNode GenerateTreeFromChild(Child child, List<(float, float)> ParentUsedPos)
    {
        Debug.Log("processing Child: " + child.name);
        int remainingChildCnt = child.childrenCnt;
        
        ZoomingController.TreeNode node = getTreeNodeFromChild(child,ParentUsedPos);
        if(child.childrenCnt == 0 || depth > depthLimit)
        {
            return node;
        }
        Group ChildGroup = getGroup(child.childrenGroup);
        List<Child>ChildList = new List<Child>();
        List<float> chanceList = new List<float>();
        List<(float, float)> MyUsedPos = new List<(float, float)>();
        for (int i = 0; i < ChildGroup.children.Count; i += 1)
        {
            if (ChildGroup.children[i].isPermanent)
            {
                remainingChildCnt -= 1;
                curNode = node;
                depth++;
                node.children.Add(GenerateTreeFromChild(ChildGroup.children[i],MyUsedPos));
                depth--;
            }
            else
            {
                ChildList.Add(ChildGroup.children[i]);
                chanceList.Add(ChildGroup.children[i].chance);
            }
        }
        if(remainingChildCnt < 0)
        {
            throw new ArgumentException(ChildGroup.name+" group Permanent child count is larger than Child "+child.name+"'s childrenCnt.");
        }
        List<Child> randomChildren = PickRandomName(ChildList,chanceList, remainingChildCnt);
        foreach(Child c in randomChildren)
        {
            curNode = node;
            depth += 1;
            node.children.Add(GenerateTreeFromChild(c,MyUsedPos));
            depth -= 1;
        }
        return node;
    }

    public static Group getGroup(string groupName)
    {
        for (int i = 0; i < root.myGroups.Count; i += 1)
        {
            if (root.myGroups[i].name == groupName)
            {
                return root.myGroups[i];
            }
        }
        Debug.Log("fail to get group,groupName is " + groupName);
        return null;
    }
    public static ZoomingController.TreeNode getTreeNodeFromChild(Child child,List<(float, float)>usedPos)
    {
        float posx,posy;
        if (child.isRandomPos) {

            var position = GetRandomPosition(usedPos);
            posx = position.Item1;
            posy = position.Item2;

        }
        else
        {
            posx = child.posx;
            posy = child.posy;
        }
        ZoomingController.TreeNode node = new ZoomingController.TreeNode()
        {
            name = child.name,
            posx = posx,
            posy = posy,
            sprite = ZoomingController.getSprite(child.name),
            text = child.text,
            parent = curNode,
        };
        return node;
    }

    public static (float, float) GetRandomPosition(List<(float, float)> usedPositions)
    {
        var availablePositions = randomPos
            .Where(pos => !usedPositions.Contains(pos))  // 过滤已使用的位置
            .Where(pos => IsFarEnough(pos, usedPositions))  // 过滤距离过近的位置
            .ToList();

        if (availablePositions.Count == 0)
        {
            Debug.LogWarning("No available positions!");
            return (0, 0); // 无可用位置
        }

        // 随机选择一个位置
        var randomIndex = UnityEngine.Random.Range(0, availablePositions.Count);
        var selectedPos = availablePositions[randomIndex];

        // 标记为已使用
        usedPositions.Add(selectedPos);

        return selectedPos;
    }
    private static bool IsFarEnough((float x, float y) position, List<(float, float)> existingPositions, float minDistance = 0.1f)
    {
        foreach (var existingPos in existingPositions)
        {
            float dx = position.x - existingPos.Item1;
            float dy = position.y - existingPos.Item2;
            float distance = Mathf.Sqrt(dx * dx + dy * dy);

            if (distance < minDistance)
            {
                return false; // 距离过近
            }
        }

        return true;
    }
    public static string LoadRandomDatatoString(string fileName)
    {
        // 从 StreamingAssets 目录中读取 JSON 文件内容
        string jsonPath = ZoomingController.jsonPath;
        string path = Path.Combine(Application.dataPath, jsonPath);
        string _jsonPath = Path.Combine(path, $"{fileName}");
        if (!_jsonPath.EndsWith(".json"))
        {
            _jsonPath = _jsonPath + ".json";
        }
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

    static List<Child> PickRandomName(List<Child> nameList,List<float> chanceList,int cnt)
    {
        List<Child> res = new List<Child>();
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

    

    [System.Serializable]
    public class Root
    {
        [HideInInspector] public string _comment;  // 可选，评论内容可以忽略
        public List<Group> myGroups;
    }
    [System.Serializable]
    public class Group
    {
        public string name;
        public List<Child> children;
    }

    [System.Serializable]
    public class Child
    {
        [HideInInspector] public string _comment;  // 可选，评论内容可以忽略
        public string name;
        public string text;
        public bool isPermanent;
        public bool isRandomPos;
        public int childrenCnt;
        public string childrenGroup;
        public float chance;
        public float posx;
        public float posy;
    }
}
