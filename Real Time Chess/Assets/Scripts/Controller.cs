using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Controller
{
    public static int greenSelectionX = -1;
    public static int greenSelectionY = -1;
    public static int redSelectionX = -1;
    public static int redSelectionY = -1;

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

    public static Vector2 getKnightMovement(int joyNumber)
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

        float rot = Mathf.Atan2(Input.GetAxis(leftVertical), Input.GetAxis(leftHorizontal)) * Mathf.Rad2Deg;

        if (rot > 0 && rot < 45)
        {
            xDir = 2;
            yDir = 1;
        }
        if (rot > 45 && rot < 90)
        {
            xDir = 1;
            yDir = 2;
        }
        if (rot > 90 && rot < 135)
        {
            xDir = -1;
            yDir = 2;
        }
        if (rot > 135 && rot < 180)
        {
            xDir = -2;
            yDir = 1;
        }
        if (rot < -135 && rot > -180)
        {
            xDir = -2;
            yDir = -1;
        }
        if (rot < -90 && rot > -135)
        {
            xDir = -1;
            yDir = -2;
        }
        if (rot < -45 && rot > -90)
        {
            xDir = 1;
            yDir = -2;
        }
        if (rot > -45 && rot < 0)
        {
            xDir = 2;
            yDir = -1;
        }
        return new Vector2(xDir, yDir);
    }

    public static Vector2 getKnightAim(int joyNumber)
    {
        int xDir = 0;
        int yDir = 0;
        string rightHorizontal = "RightStick_Horizontal";
        string rightVertical = "RightStick_Vertical";
        if (joyNumber == 2)
        {
            rightHorizontal = "P2_RightStick_Horizontal";
            rightVertical = "P2_RightStick_Vertical";
        }

        //bug.Log(Input.GetAxis(rightVertical) + " " + Input.GetAxis(rightHorizontal));
        float rot = Mathf.Atan2(Input.GetAxis(rightVertical), Input.GetAxis(rightHorizontal)) * Mathf.Rad2Deg;

        if (rot >= 0 && rot < 45)
        {
            xDir = 2;
            yDir = 1;
        }
        if (rot >= 45 && rot < 90)
        {
            xDir = 1;
            yDir = 2;
        }
        if (rot >= 90 && rot < 135)
        {
            xDir = -1;
            yDir = 2;
        }
        if (rot >= 135 && rot <= 180)
        {
            xDir = -2;
            yDir = 1;
        }
        if (rot < -135 && rot >= -180)
        {
            xDir = -2;
            yDir = -1;
        }
        if (rot < -90 && rot >= -135)
        {
            xDir = -1;
            yDir = -2;
        }
        if (rot < -45 && rot >= -90)
        {
            xDir = 1;
            yDir = -2;
        }
        if (rot >= -45 && rot < 0)
        {
            xDir = 2;
            yDir = -1;
        }

        return new Vector2(xDir, yDir);
    }

    public static Vector2 getAim(int joyNumber)
    {
        int xDir = 0;
        int yDir = 0;
        if (joyNumber == 1)
        {
            if (Input.GetAxis("RightStick_Horizontal") != 0)
            {
                xDir = Input.GetAxis("RightStick_Horizontal") > 0 ? 1 : -1;
            }
            if (Input.GetAxis("RightStick_Vertical") != 0)
            {
                yDir = Input.GetAxis("RightStick_Vertical") > 0 ? 1 : -1;
            }
        }

        if (joyNumber == 2)
        {
            if (Input.GetAxis("P2_RightStick_Horizontal") != 0)
            {
                xDir = Input.GetAxis("P2_RightStick_Horizontal") > 0 ? 1 : -1;
            }
            if (Input.GetAxis("P2_RightStick_Vertical") != 0)
            {
                yDir = Input.GetAxis("P2_RightStick_Vertical") > 0 ? 1 : -1;
            }
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

    public static bool getFire(int joyNumber)
    {
        bool firing = false;
        if (joyNumber == 1)
        {
            if (Input.GetAxis("Shoot") > 0.5)
            {
                firing = true;
            }
        }
        if (joyNumber == 2)
        {
            if (Input.GetAxis("P2_Shoot") > 0.5)
            {
                firing = true;
            }
        }

        return firing;        
    }

}
