using UnityEngine;

public class Bread : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public string jamType; 
    public int nutsCount; 
    public string crustType; 

    public void Initialize(string spritePath, string jamType, int nutsCount, string crustType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        //load a sprite and sprite setting
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
