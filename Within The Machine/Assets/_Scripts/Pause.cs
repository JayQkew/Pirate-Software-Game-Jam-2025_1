using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
        
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
        pauseMenu.SetActive(isPaused);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }
}
