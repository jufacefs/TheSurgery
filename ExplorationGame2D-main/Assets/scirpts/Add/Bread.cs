using UnityEngine;

public class Bread : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public string jamType; // 酱料类型
    public int nutsCount; // 坚果数量
    public string crustType; // 皮类型

    // 初始化Bread对象
    public void Initialize(string spritePath, string jamType, int nutsCount, string crustType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // 加载并设置Sprite
        Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
        if (loadedSprite != null)
        {
            spriteRenderer.sprite = loadedSprite;
        }
        else
        {
            Debug.LogError("Failed to load sprite at path: " + spritePath);
        }

        this.jamType = jamType;
        this.nutsCount = nutsCount;
        this.crustType = crustType;
    }

}
