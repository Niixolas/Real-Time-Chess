using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Text startText;

    public GameObject menuPawn;

    private bool canMove = true;

    private void Start()
    {
        Cursor.visible = false;
        //StartCoroutine("PressStartTextFlash");
    }

    private void SetMove()
    {
        canMove = true;
    }

    private void Update()
    {
        if (InputController.Instance.startPressed || InputController.Instance.p1Pressed)
        {
            if (menuPawn.transform.localPosition.y == -55.0f)
            {
                StopAllCoroutines();
                SceneManager.LoadScene("_Main");
            }
            else if (menuPawn.transform.localPosition.y == -155.0f)
            {
                Quit();
            }
            
        }

        if (InputController.Instance.p1Move.y < 0 && menuPawn.transform.localPosition.y != -155.0f && canMove)
        {
            menuPawn.transform.localPosition = new Vector2(menuPawn.transform.localPosition.x, menuPawn.transform.localPosition.y - 50.0f);
            canMove = false;
            Invoke("SetMove", 0.2f);
        }
        else if (InputController.Instance.p1Move.y > 0 && menuPawn.transform.localPosition.y != -55.0f && canMove)
        {
            menuPawn.transform.localPosition = new Vector2(menuPawn.transform.localPosition.x, menuPawn.transform.localPosition.y + 50.0f);
            canMove = false;
            Invoke("SetMove", 0.2f);
        }
    }

	public void Quit()
	{
        StopAllCoroutines();
        Application.Quit();
	}

    //IEnumerator PressStartTextFlash()
    //{
    //    while (true)
    //    {
    //        if (startText.enabled)
    //        {
    //            startText.enabled = false;
    //        }
    //        else
    //        {
    //            startText.enabled = true;
    //        }

    //        yield return new WaitForSeconds(0.75f);
    //    }
    //}

}
