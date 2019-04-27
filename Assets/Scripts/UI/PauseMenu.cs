using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
     public static bool GameIsPaused = false;
     public GameObject pauseMenuUI;
 
     // Update is called once per frame
     public void Update()
     {
 
         if (Input.GetKeyDown(KeyCode.Escape))
         {
             if (GameIsPaused) {
                 Resume();
                 
             } else {
                 Pause();
             }
         }
     }
 
    public void Resume()
     {
         pauseMenuUI.SetActive(false);
         TimeManagerSingleton.Instance.SetTimeScale(1f);
         GameIsPaused = false;
         
     }
     public void Pause()
     
     {
         pauseMenuUI.SetActive(true);
         TimeManagerSingleton.Instance.SetTimeScale(0f);
         GameIsPaused = true;
     }
 }
