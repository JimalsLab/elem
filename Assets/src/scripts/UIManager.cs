using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvas;
    public bool isMenuActive;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("/Canvas");
        isMenuActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        canvas.SetActive(isMenuActive);
    }
}
