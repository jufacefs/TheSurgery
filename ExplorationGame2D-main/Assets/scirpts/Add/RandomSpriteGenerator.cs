using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteGenerator : MonoBehaviour
{
    public GameObject itemPrefab; // 用于生成的子GameObject的预制件
    public string[] spritePaths; // 存放Sprite路径的数组
    public int maxItems = 5; // 最多生成的项目数量
    public Transform parentObject; // 父对象，新生成的对象将作为这个对象的子对象

    void Start()
    {
        GenerateRandomSprites();
    }

    void GenerateRandomSprites()
    {
        for (int i = 0; i < maxItems; i++)
        {
            // 创建新的GameObject
            GameObject newItem = Instantiate(itemPrefab, parentObject);

            // 随机位置（这里简单地在父对象的局部坐标系内随机）
            newItem.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            // 随机选择一个Sprite
            Sprite newSprite = Resources.Load<Sprite>(spritePaths[Random.Range(0, spritePaths.Length)]);
            newItem.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
}
