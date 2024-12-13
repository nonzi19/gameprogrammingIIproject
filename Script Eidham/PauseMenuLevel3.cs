using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLevel3 : MonoBehaviour
{
    public string levelSelect, mainMenu, levelrestart;
    public GameObject pauseScreen;
    public static PauseMenuLevel3 instance;
    public bool isPaused;


    private void Awake()
    {
        instance = this;
    }
  
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if(isPaused)
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        } else
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    /*public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        pauseScreen.SetActive(true);
        Time.timeScale = 1f;
    }

    */

        public void LevelRestart()
    {
        SceneManager.LoadScene("Level3");
        pauseScreen.SetActive(true);
        Time.timeScale = 1f;
    }
    


}
