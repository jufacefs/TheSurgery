using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Transform[] childs;
    public Vector3 pivotOffset = new Vector3(0, -1, 0);

    void Start()
    {
        childs = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].rotation = Camera.main.transform.rotation;
        }
        transform.rotation = Camera.main.transform.rotation;


    }
}
