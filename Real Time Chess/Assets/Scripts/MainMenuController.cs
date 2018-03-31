using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("StartButton"))
        {
            SceneManager.LoadScene("_Main");
        }
        if (Input.GetButtonDown("SelectButton"))
        {
            Quit();
        }
    }

	public void Quit()
	{
        Application.Quit();
	}
}
