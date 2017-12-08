using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour 
{
	

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);

	}
	public void Quit()
	{
		Application.Quit ();
	}

	public void Pause(Transform canvas)
	{
		//if (Input.GetKeyDown (KeyCode.Escape)) 
		//{
			if (canvas.GetComponentInChildren<Image>().gameObject.activeInHierarchy == false) {
				canvas.GetComponentInChildren<Image>().gameObject.SetActive (true);
				Debug.Log ("pause");
			} else {
				canvas.GetComponentInChildren<Image>().gameObject.SetActive (false);
			}
		//}
	}




}
