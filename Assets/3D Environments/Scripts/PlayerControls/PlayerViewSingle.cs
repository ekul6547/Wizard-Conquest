using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewSingle : MonoBehaviour {

    private bool isPaused = false;
    private bool inMenu = true;
    
    void Start ()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                inMenu = true;
            }
            else
            {
                inMenu = false;
            }
        }

        if (inMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
