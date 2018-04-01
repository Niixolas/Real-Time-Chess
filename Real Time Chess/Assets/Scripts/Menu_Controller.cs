using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour 
{
    public Canvas pauseCanvas;

    private void Update()
    {
        if (Input.GetButtonDown("StartButton"))
        {
            Pause();
        }
        if (Input.GetButtonDown("SelectButton") && pauseCanvas.isActiveAndEnabled == true)
        {
            Quit();
        }
    }

    public void LoadScene(string sceneName)
	{
		Time.timeScale = 1;
		SceneManager.LoadScene (sceneName);


	}

	public void Quit()
	{
        Time.timeScale = 1;
        SceneManager.LoadScene("_Menu");
	}

	public void Pause()
	{
		if (pauseCanvas.isActiveAndEnabled == false)
        {
            //canvas.enabled = true;
            pauseCanvas.gameObject.SetActive(true);
			Time.timeScale = 0;
			Debug.Log ("Pause");
		} else
        {
            pauseCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
	}
}
