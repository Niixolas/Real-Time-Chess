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
        StartCoroutine("PressStartTextFlash");
    }

    private void Update()
    {
        if (InputController.Instance.startPressed)
        {
            StopAllCoroutines();
            SceneManager.LoadScene("_Main");
        }

        if (InputController.Instance.selectPressed)
        {
            Quit();
        }
    }

	public void Quit()
	{
        StopAllCoroutines();
        Application.Quit();
	}

    IEnumerator PressStartTextFlash()
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
