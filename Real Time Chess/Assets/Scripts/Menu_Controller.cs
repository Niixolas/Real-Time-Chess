using System.Collections;
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

    public Canvas selectButtonHint;
    public Canvas leftStickHint;
    public Canvas rightStickHint;

    public AudioSource backgroundMusic;

    bool gameOver = false;

    bool showLeftStickHint = false;
    bool showSelectHint = false;
    bool showRightStickHint = false;

    float timeUntilShowLeftStickHint = 5.0f;    
    float timeUntilShowSelectHint = 10.0f;
    float timeUntilShowRightStickHint = 10.0f;
    
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

        DetermineHints();
    }

    private void DetermineHints()
    {
        // Countdown timer if player hasn't used the left stick
        if (!InputController.Instance.playerHasUsedLeftStick)
        {
            timeUntilShowLeftStickHint -= Time.deltaTime;
            if (timeUntilShowLeftStickHint <= 0.0f)
            {
                leftStickHint.enabled = true;
                showLeftStickHint = true;
                timeUntilShowLeftStickHint = 20.0f;
            }
        }

        // Countdown timer if player hasn't selected a piece
        if (InputController.Instance.playerHasUsedLeftStick && !InputController.Instance.playerHasSelectedPiece)
        {
            timeUntilShowSelectHint -= Time.deltaTime;
            if (timeUntilShowSelectHint <= 0.0f)
            {
                selectButtonHint.enabled = true;
                showSelectHint = true;
                timeUntilShowSelectHint = 20.0f;
            }
        }

        // Countdown timer if player hasn't selected a piece
        if (InputController.Instance.playerHasSelectedPiece && !InputController.Instance.playerHasUsedRightStick)
        {
            timeUntilShowRightStickHint -= Time.deltaTime;
            if (timeUntilShowRightStickHint <= 0.0f)
            {
                rightStickHint.enabled = true;
                showRightStickHint = true;
                timeUntilShowRightStickHint = 20.0f;
            }
        }

        ShowAndFadeHint(ref showLeftStickHint, leftStickHint);
        ShowAndFadeHint(ref showSelectHint, selectButtonHint);
        ShowAndFadeHint(ref showRightStickHint, rightStickHint);
    }

    private void ShowAndFadeHint(ref bool showHint, Canvas canvasHint)
    {
        // Show and fade hint
        if (canvasHint.enabled == true)
        {
            if (!showHint)
            {
                float newAlpha = canvasHint.GetComponent<CanvasGroup>().alpha - 0.3f * Time.deltaTime;
                canvasHint.GetComponent<CanvasGroup>().alpha = newAlpha;
                if (newAlpha <= 0.0f)
                {
                    canvasHint.enabled = false;
                }
            }
            else if (showHint && canvasHint.GetComponent<CanvasGroup>().alpha <= 1.0f)
            {
                float newAlpha = canvasHint.GetComponent<CanvasGroup>().alpha + 0.3f * Time.deltaTime;
                canvasHint.GetComponent<CanvasGroup>().alpha = newAlpha;
                if (newAlpha >= 1.0f)
                {
                    showHint = false;
                }
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
