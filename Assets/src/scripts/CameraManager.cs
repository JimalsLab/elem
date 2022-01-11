using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public int speed;
    public GameObject focusPoint;
    public new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("z"))
        {
            focusPoint.transform.position += new Vector3(0.1f*speed/100, 0, 0.1f * speed / 100);
        }
        if (Input.GetKey("s"))
        {
            focusPoint.transform.position += new Vector3(-0.1f * speed / 100, 0, -0.1f * speed / 100);
        }
        if (Input.GetKey("q"))
        {
            focusPoint.transform.position += new Vector3(-0.1f * speed / 100, 0, 0.1f * speed / 100);
        }
        if (Input.GetKey("d"))
        {
            focusPoint.transform.position += new Vector3(0.1f * speed / 100, 0, -0.1f * speed / 100);
        }
        if (Input.GetKey("space"))
        {
            focusPoint.transform.position += new Vector3(0, 0.1f * speed / 100, 0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            camera.transform.position += new Vector3(
                Input.GetAxis("Mouse ScrollWheel") / Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")),
                -Input.GetAxis("Mouse ScrollWheel") / Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")),
                Input.GetAxis("Mouse ScrollWheel") / Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")));
        }
    }
}
