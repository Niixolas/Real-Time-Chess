using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text startText;

    public GameObject menuPawn;

    public GameObject shotObject;

    public Animator fadePanel;

    private bool canMove = true;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void SetMove()
    {
        canMove = true;
    }

    private void LoadGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("_Main");
    }

    private void Update()
    {
        if ( (InputController.Instance.startPressed || InputController.Instance.p1Pressed) && canMove)
        {
            if (menuPawn.transform.localPosition.y == -55.0f)
            {
                GameObject shot = Instantiate(shotObject, menuPawn.transform);
                shot.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);
                canMove = false;

                fadePanel.SetTrigger("fade");

                Invoke("LoadGame", 2.0f);                
            }
            else if (menuPawn.transform.localPosition.y == -155.0f)
            {
                GameObject shot = Instantiate(shotObject, menuPawn.transform);
                shot.GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 0.0f);
                canMove = false;

                fadePanel.SetTrigger("fade");

                Invoke("Quit", 1.5f);
            }
            
        }

        if (InputController.Instance.p1Move.y < 0 && menuPawn.transform.localPosition.y != -155.0f && canMove)
        {
            menuPawn.transform.localPosition = new Vector2(menuPawn.transform.localPosition.x, menuPawn.transform.localPosition.y - 100.0f);
            menuPawn.GetComponent<AudioSource>().Play();
            canMove = false;
            Invoke("SetMove", 0.2f);
        }
        else if (InputController.Instance.p1Move.y > 0 && menuPawn.transform.localPosition.y != -55.0f && canMove)
        {
            menuPawn.transform.localPosition = new Vector2(menuPawn.transform.localPosition.x, menuPawn.transform.localPosition.y + 100.0f);
            menuPawn.GetComponent<AudioSource>().Play();
            canMove = false;
            Invoke("SetMove", 0.2f);
        }
    }

	public void Quit()
	{
        StopAllCoroutines();
        Application.Quit();
	}

}
