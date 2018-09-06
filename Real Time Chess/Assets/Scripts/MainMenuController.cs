using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuLocation
{
    mainMenu,
    credits
};

public class MainMenuController : MonoBehaviour
{
    public Text startText;

    public GameObject menuPawn;

    public GameObject shotObject;

    public Canvas mainCanvas;
    public Animator fadePanel;

    public Canvas creditsCanvas;
    public Animator creditsFadePanel;

    private bool canMove = true;

    private MenuLocation menuLocation;

    private void Start()
    {
        Cursor.visible = false;
        menuLocation = MenuLocation.mainMenu;
    }

    private void Update()
    {
        if (menuLocation == MenuLocation.mainMenu)
        {
            CheckMainMenuInputs();
        }
        else if (menuLocation == MenuLocation.credits)
        {
            CheckCreditsMenuInputs();
        }
    }

    /// <summary>
    /// Allows the cursor to move after being frozen
    /// </summary>
    private void SetMove()
    {
        canMove = true;
    }

    /// <summary>
    /// Stop all coroutines and load the main game scene
    /// </summary>
    private void LoadGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("_Main");
    }

    /// <summary>
    /// Check for controller inputs when on the main menu
    /// </summary>
    private void CheckMainMenuInputs()
    {
        if ((InputController.Instance.startPressed || InputController.Instance.p1Pressed) && canMove)
        {
            GameObject shot = Instantiate(shotObject, menuPawn.transform);

            if (menuPawn.transform.localPosition.y == -55.0f)
            {                
                shot.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);
                fadePanel.SetTrigger("fade");

                Invoke("LoadGame", 2.0f);
            }
            else if (menuPawn.transform.localPosition.y == -105.0f)
            {
                shot.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);

                //StartCoroutine(FadeOutFadeIn(fadePanel, mainCanvas, creditsFadePanel, creditsCanvas, MenuLocation.credits));
                Invoke("SetMove", 1.5f);
            }
            else if (menuPawn.transform.localPosition.y == -155.0f)
            {
                shot.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);

                StartCoroutine(FadeOutFadeIn(fadePanel, mainCanvas, creditsFadePanel, creditsCanvas, MenuLocation.credits));

                Invoke("SetMove", 3.0f);
            }
            else if (menuPawn.transform.localPosition.y == -205.0f)
            {
                shot.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);

                fadePanel.SetTrigger("fade");

                Invoke("Quit", 1.5f);
            }

            shot.GetComponent<AudioSource>().Play();
            Destroy(shot, 1.4f);
            canMove = false;
        }

        if (InputController.Instance.p1Move.y != 0 && canMove)
        {
            if (InputController.Instance.p1Move.y < 0 && menuPawn.transform.localPosition.y != -205.0f)
            {
                menuPawn.transform.localPosition = new Vector2(menuPawn.transform.localPosition.x, menuPawn.transform.localPosition.y - 50.0f);
                menuPawn.GetComponent<AudioSource>().Play();
                canMove = false;
                Invoke("SetMove", 0.2f);
            }
            else if (InputController.Instance.p1Move.y > 0 && menuPawn.transform.localPosition.y != -55.0f)
            {
                menuPawn.transform.localPosition = new Vector2(menuPawn.transform.localPosition.x, menuPawn.transform.localPosition.y + 50.0f);
                menuPawn.GetComponent<AudioSource>().Play();
                canMove = false;
                Invoke("SetMove", 0.2f);
            }
        }
    }

    /// <summary>
    /// Check for controller inputs when on the credits menu
    /// </summary>
    private void CheckCreditsMenuInputs()
    {
        if (InputController.Instance.startPressed && canMove)
        {
            StartCoroutine(FadeOutFadeIn(creditsFadePanel, creditsCanvas, fadePanel, mainCanvas, MenuLocation.mainMenu));

            canMove = false;
            Invoke("SetMove", 1.5f);
        }
    }

    /// <summary>
    /// Fade between menu screens
    /// </summary>
    /// <param name="animatorToFadeOut"></param>
    /// <param name="canvasToDisable"></param>
    /// <param name="animatorToFadeIn"></param>
    /// <param name="canvasToEnable"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public IEnumerator FadeOutFadeIn(Animator animatorToFadeOut, Canvas canvasToDisable, Animator animatorToFadeIn, Canvas canvasToEnable, MenuLocation destination)
    {
        animatorToFadeOut.SetTrigger("fade");

        yield return new WaitForSeconds(1.5f);

        canvasToDisable.gameObject.SetActive(false);
        canvasToEnable.gameObject.SetActive(true);

        animatorToFadeIn.SetTrigger("fadeIn");

        menuLocation = destination;

        yield break;
    }

    /// <summary>
    /// Stop all coroutines and quit the game
    /// </summary>
	public void Quit()
	{
        StopAllCoroutines();
        Application.Quit();
	}

}
