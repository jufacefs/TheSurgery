using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsZoomingParent : MonoBehaviour
{
    // Start is called before the first frame update
    public string jsonFileName;
    public bool isInfinite;
    private bool isClickable = true;
    public bool isParent = false;
    void Start()
    {
        InitializeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (isClickable)
        {
            Debug.Log(transform.name + "is clicked");
            //isClickable = false;
            if (isParent)
            {
                if (!jsonFileName.EndsWith(".json"))
                {
                    jsonFileName = jsonFileName + ".json";
                }
                ZoomingController.Instance.ParentClicked(jsonFileName, transform.gameObject);
            }
            else
            {
                ZoomingController.Instance.ChildrenClicked(transform.gameObject);
            }
        }
        
    }

    private void OnMouseOver()
    {
        if(isClickable)
        {
            transform.GetComponent<SpriteOutline>().enabled = true;
        }
    }

    private void OnMouseExit()
    {
        transform.GetComponent<SpriteOutline>().enabled = false;
    }

    public void beginZooming()
    {
        ZoomingController.Instance.LoadTree(jsonFileName);
    }

    public void InitializeSprite()
    {
        if(GetComponent<PolygonCollider2D>() == null)
        {
            transform.AddComponent<PolygonCollider2D>();
        }
        if(GetComponent<SpriteOutline>() == null)
        {
            transform.AddComponent<SpriteOutline>();
            transform.GetComponent<SpriteOutline>().enabled = false;
        }
    }

    public void setClickable(bool a)
    {
        isClickable=a;
    }
}
