using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Select button (A) on controller.
        bool Select_down = Input.GetButtonDown("Select");
        bool Select_up = Input.GetButtonUp("Select");
        bool Select_held = Input.GetButton("Select");

        // Deselect button (B) on controller. 
        bool Deselect_down = Input.GetButtonDown("Deselect"); 
        bool Deselect_up = Input.GetButtonUp("Deselect");
        bool Deselect_held = Input.GetButton("Deselect");
        
        
        // Trigger to Fire.
        float Right_Trigger = Input.GetAxis("Shoot");

        // Joystick to move.
        float LeftStick_H = Input.GetAxisRaw("LeftStick_Horizontal");
        float LeftStick_V = Input.GetAxisRaw("LeftStick_Vertical");

        // Joystick to aim.
        float RightStick_V = Input.GetAxis("RightStick_Vertical");
        float RightStick_H = Input.GetAxis("RightStick_Horizontal"); 


    }
}
