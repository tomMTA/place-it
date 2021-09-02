using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void LoadLevel(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	// Start is called before the first frame update
	public void Exit()
	{
		Debug.Log("EXIT");
		Application.Quit();
	}
}
