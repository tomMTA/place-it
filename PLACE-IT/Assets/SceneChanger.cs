using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public void LoadLevel(string sceneName)
	{
		Debug.Log("Loading " + sceneName);
		SceneManager.LoadScene(sceneName);
	}

	// Start is called before the first frame update
	public void Exit()
	{
		Debug.Log("EXIT");
		Application.Quit();
	}
}