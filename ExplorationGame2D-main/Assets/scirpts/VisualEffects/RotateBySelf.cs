using UnityEngine;

public class RotateBySelf : MonoBehaviour
{

    public float speed = 60.0f;

    // Use this for initialization

    void Start()
    {


    }


    // Update is called once per frame

    void Update()
    {

        transform.Rotate(Vector3.up, Time.deltaTime * speed);

    }

}