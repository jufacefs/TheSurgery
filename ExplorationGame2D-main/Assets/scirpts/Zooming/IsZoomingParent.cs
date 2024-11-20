using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsZoomingParent : MonoBehaviour
{
    // Start is called before the first frame update
    public string jsonFileName;
    public bool isRandomOrInfinite;
    public bool isClickable = true;
    public bool isParent = false;
    public string spriteName = "";
    void Start()
    {
        InitializeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClickable && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == GetComponent<PolygonCollider2D>())
            {
                detection();
            }
        }

    }

    private void detection()
    {
        Debug.Log(transform.name + "is clicked");
        //isClickable = false;
        if (isParent)
        {
            if (!jsonFileName.EndsWith(".json"))
            {
                jsonFileName = jsonFileName + ".json";
            }
            if (isRandomOrInfinite)
            {
                ZoomingController.Instance.RandomTreeClicked(jsonFileName, transform.gameObject, spriteName);
            }
            else
            {
                ZoomingController.Instance.ParentClicked(jsonFileName, transform.gameObject);
            }

        }
        else
        {
            ZoomingController.Instance.ChildrenClicked(transform.gameObject);
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
