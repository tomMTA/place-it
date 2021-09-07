using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static bool GameIsPaused = false;
	public GameObject PauseMenuUI;



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameIsPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
    }

	public void PauseGame()
	{
		GameIsPaused = true;
		PauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		ToggleBridgeActivity(false);
	}
	public void ResumeGame()
	{
		GameIsPaused = false;
		PauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		ToggleBridgeActivity(true);
	}

	private void ToggleBridgeActivity(bool toggle)
	{
		if(InentoryManager.activeBridge != null)
		{
			MonoBehaviour[] scripts = InentoryManager.activeBridge.GetComponents<MonoBehaviour>();

			foreach (MonoBehaviour script in scripts)
			{
				script.enabled = toggle;
			}
		}
	}
}
