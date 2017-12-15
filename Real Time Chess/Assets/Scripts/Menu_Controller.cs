using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour 
{
	

	public void LoadScene(string sceneName)
	{
		Time.timeScale = 1;
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
				Time.timeScale = 0;
				GameObject.FindGameObjectWithTag ("boardManager").GetComponent<BoxCollider2D> ().enabled = false;
				Debug.Log ("Pause");
			} else {
				canvas.GetComponentInChildren<Image>().gameObject.SetActive (false);
			}
		//}
	}

	public void Resume(Transform canvas)
	{
		
		if (canvas.GetComponentInChildren<Image>().gameObject.activeInHierarchy == true) {
			canvas.GetComponentInChildren<Image>().gameObject.SetActive (false);
			Time.timeScale=1;
			GameObject.FindGameObjectWithTag ("boardManager").GetComponent<BoxCollider2D> ().enabled = true;
			Debug.Log ("Resume");
	} 
	}

	public void Setting(Transform canvas)
	{
		
		if (canvas.GetComponentInChildren<Image>().gameObject.activeInHierarchy == false) {
			canvas.GetComponentInChildren<Image>().gameObject.SetActive (true);
		} else {
			canvas.GetComponentInChildren<Image>().gameObject.SetActive (false);
		}
	}


}
