using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Controller
{
    public static int selectionX = -1;
    public static int selectionY = -1;

    public static Vector2 getMovement(int joyNumber)
    {
        int xDir = 0;
        int yDir = 0;
        string leftHorizontal = "LeftStick_Horizontal";
        string leftVertical = "LeftStick_Vertical";
        if (joyNumber == 2)
        {
            leftHorizontal = "P2_LeftStick_Horizontal";
            leftVertical = "P2_LeftStick_Vertical";
        }

        if (Input.GetAxis(leftHorizontal) != 0)
        {
            xDir = Input.GetAxis(leftHorizontal) > 0 ? 1 : -1;
        }
        if (Input.GetAxis(leftVertical) != 0)
        {
            yDir = Input.GetAxis(leftVertical) > 0 ? 1 : -1;
        }

        return new Vector2(xDir, yDir);
    }

    public static Vector2 getAim()
    {
        int xDir = 0;
        int yDir = 0;
        if (Input.GetAxis("RightStick_Horizontal") != 0)
        {
            xDir = Input.GetAxis("RightStick_Horizontal") > 0 ? 1 : -1;
        }
        if (Input.GetAxis("RightStick_Vertical") != 0)
        {
            yDir = Input.GetAxis("RightStick_Vertical") > 0 ? 1 : -1;
        }

        return new Vector2(xDir, yDir);
    }

    public static bool getPressed(int joyNumber)
    {

        bool Pressed = false;
        if (joyNumber == 1)
        {
            Pressed = Input.GetButtonDown("Submit");
        }
        if (joyNumber == 2)
        {
            Pressed = Input.GetButtonDown("P2_Submit");
        }        

        return Pressed;
    }

    public static bool getCancel()
    {

        bool Pressed = Input.GetButtonDown("Cancel");

        return Pressed;
    }

    public static bool getFire()
    {
        bool firing = false;
        if(Input.GetAxis("Shoot") > 0.5)
        {
            firing = true;
        }

        return firing;
       
        
    }

    /*
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
        */
}
