using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Controller
{



    // Update is called once per frame
    public static Vector2 getMovment()
    {

        int xDir = 0;
        int yDir = 0;
        if (Input.GetAxisRaw("LeftStick_Horizontal") != 0)
        {
            xDir = Input.GetAxisRaw("LeftStick_Horizontal") > 0 ? 1 : -1;
        }
        if (Input.GetAxisRaw("LeftStick_Horizontal") != 0)
        {
            yDir = Input.GetAxisRaw("LeftStick_Horizontal") > 0 ? 1 : -1;
        }
      

        return new Vector2(xDir, yDir);
    }

    public static Vector2 getAim()
    {
        int xDir = 0;
        int yDir = 0;

        if (Input.GetAxis("RightStick_Vertical") != 0)
        {
            xDir = Input.GetAxisRaw("RightStick_Vertical") > 0 ? 1 : -1;
        }
        if (Input.GetAxisRaw("RightStick_Vertical") != 0)
        {
            yDir = Input.GetAxisRaw("RightStick_Vertical") > 0 ? 1 : -1;
        }

        return new Vector2(xDir, yDir);
    }

    public static bool getSelectPress()
    {
        bool selectPressed = Input.GetButtonDown("Select");
        return selectPressed;
    }

    public static bool getDeselectPress()
    {
        bool deselectPress = Input.GetButtonDown("Deselect");
        return deselectPress;
    } 


        // Select button (A) on controller.
       /* bool Select_down = Input.GetButtonDown("Select");
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
        */
}

