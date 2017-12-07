using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GlowText_OnHover : MonoBehaviour {

    private void Start()
    {
        Cursor.visible = true;
    }

    private void OnMouseOver()
    {
		this.GetComponent<Text>().material.color = Color.white;
    }
    private void OnMouseExit()
    {
		this.GetComponent<Text>().material.color = Color.black;
    }

}