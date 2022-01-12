using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public int speed;
    public GameObject mainCamera;
    public GameObject canvasCamera;
    public bool isMenuMode;
    // Start is called before the first frame update
    void Start()
    {
        isMenuMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleCameras();
        }
        if (Input.GetKey("z"))
        {
            mainCamera.transform.position += new Vector3(0.1f*speed/100, 0, 0.1f * speed / 100);
        }
        if (Input.GetKey("s"))
        {
            mainCamera.transform.position += new Vector3(-0.1f * speed / 100, 0, -0.1f * speed / 100);
        }
        if (Input.GetKey("q"))
        {
            mainCamera.transform.position += new Vector3(-0.1f * speed / 100, 0, 0.1f * speed / 100);
        }
        if (Input.GetKey("d"))
        {
            mainCamera.transform.position += new Vector3(0.1f * speed / 100, 0, -0.1f * speed / 100);
        }
        if (Input.GetKey("space"))
        {
            mainCamera.transform.position += new Vector3(0, 0.1f * speed / 100, 0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            mainCamera.transform.position += new Vector3(
                Input.GetAxis("Mouse ScrollWheel") / Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")),
                -Input.GetAxis("Mouse ScrollWheel") / Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")),
                Input.GetAxis("Mouse ScrollWheel") / Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")));
        }
    }

    public void ToggleCameras()
    {
        mainCamera.SetActive(isMenuMode);
        canvasCamera.SetActive(!isMenuMode);
        isMenuMode = !isMenuMode;
    }
}
