using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour 
{
    public Canvas canvas;

    private void Update()
    {
        if (Input.GetButtonDown("StartButton"))
        {
            Pause();
        }
        if (Input.GetButtonDown("SelectButton") && canvas.isActiveAndEnabled == true)
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
		if (canvas.isActiveAndEnabled == false)
        {
            //canvas.enabled = true;
            canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
			Debug.Log ("Pause");
		} else
        {
            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
	}
}
