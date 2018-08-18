﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour 
{
    public Canvas pauseCanvas;
    public GameObject checkmatePanel;

    public GameObject greenWinnerText;
    public GameObject redWinnerText;

    public AudioSource backgroundMusic;

    bool gameOver = false;

    private void Update()
    {
        if (!gameOver)
        {
            if (InputController.Instance.startPressed)
            {
                Pause();
            }
            if (InputController.Instance.selectPressed && pauseCanvas.isActiveAndEnabled == true)
            {
                Quit();
            }
        }
        else
        {
            if (InputController.Instance.startPressed)
            {
                LoadScene("_Main");
            }
            if (InputController.Instance.selectPressed)
            {
                Quit();
            }
        }

    }

    public void setWinner(bool isWhite)
    {
        Time.timeScale = 0;
        checkmatePanel.SetActive(true);
        gameOver = true;
        FindObjectOfType<BoardManager>().gameOver = true;
        backgroundMusic.Stop();
        GetComponent<AudioSource>().Play();

        if (isWhite)
        {
            greenWinnerText.SetActive(true);
        }
        else
        {
            redWinnerText.SetActive(true);
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
            pauseCanvas.gameObject.SetActive(true);
			Time.timeScale = 0;
		} else
        {
            pauseCanvas.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
	}
}
