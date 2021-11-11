using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitApplication : MonoBehaviour
{
	public void QuitApp()
	{
		Debug.Log("Application quit! (Funciona sólo en standalone.)");
		Application.Quit();
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
