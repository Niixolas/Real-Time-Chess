using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text startText;

    private void Start()
    {
        StartCoroutine("pressStartTextFlash");
    }

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

    // Flash the game over text
    IEnumerator pressStartTextFlash()
    {
        while (true)
        {
            if (startText.enabled)
            {
                startText.enabled = false;
            }
            else
            {
                startText.enabled = true;
            }

            yield return new WaitForSeconds(0.75f);
        }
    }

}
