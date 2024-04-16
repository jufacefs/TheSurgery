using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    

    //public GameObject ThisObject;
    public SpriteOutline ThisOutline;

    public void Start()
    {
        ThisOutline = GetComponent<SpriteOutline>();

        if(ThisOutline!=null)
        ThisOutline.enabled=false;
    }



    public void OnMouseOver()
    {
        //Debug.Log("mouseover");
        ThisOutline.enabled = true;
    }


    public void OnMouseExit()
    {


        ThisOutline.enabled = false;
    }





}








