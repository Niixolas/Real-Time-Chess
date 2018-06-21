using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

static public class Controller
{
    public static int greenSelectionX = -1;
    public static int greenSelectionY = -1;
    public static int redSelectionX = -1;
    public static int redSelectionY = -1;

    public static Vector2 getMovement(int joyNumber)
    {
        // Establish input devices
        InputDevice playerOne = null;
        InputDevice playerTwo = null;

        // Check for connected devices
        if (InputManager.Devices.Count > 0)
        {
            playerOne = InputManager.Devices[0];

            if (InputManager.Devices.Count > 1)
            {
                playerTwo = InputManager.Devices[1];
            }
        }

        // Establish movement vector2 based on left stick input
        Vector2 movement = new Vector2(0, 0);

        if (joyNumber == 1 && playerOne != null)
        {
            movement = playerOne.LeftStick;
        } else if (joyNumber == 2 && playerTwo != null)
        {
            movement = playerTwo.LeftStick;
        }

        return movement;
    }


    public static Vector2 getKnightMovement(int joyNumber)
    {
        // Establish input devices
        InputDevice playerOne = null;
        InputDevice playerTwo = null;

        // Check for connected devices
        if (InputManager.Devices.Count > 0)
        {
            playerOne = InputManager.Devices[0];

            if (InputManager.Devices.Count > 1)
            {
                playerTwo = InputManager.Devices[1];
            }
        }

        // Establish movement vector2 based on left stick input
        Vector2 movement = new Vector2(0, 0);

        if (joyNumber == 1 && playerOne != null)
        {
            movement = playerOne.LeftStick.Vector;
        }
        else if (joyNumber == 2 && playerTwo != null)
        {
            movement = playerTwo.LeftStick.Vector;
        }

        float rot = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;

        if (rot > 0 && rot < 45)
        {
            movement = new Vector2(1, 2);
        }
        else if (rot > 45 && rot < 90)
        {
            movement = new Vector2(2, 1);
        }
        else if (rot > 90 && rot < 135)
        {
            movement = new Vector2(2, -1);
        }
        else if (rot > 135 && rot <= 180)
        {
            movement = new Vector2(1, -2);
        }
        else if (rot < -135 && rot > -180)
        {
            movement = new Vector2(-1, -2);
        }
        else if (rot < -90 && rot > -135)
        {
            movement = new Vector2(-2, -1);
        }
        else if (rot < -45 && rot > -90)
        {
            movement = new Vector2(-2, 1);
        }
        else if (rot > -45 && rot < 0)
        {
            movement = new Vector2(-1, 2);
        }
        else
        {
            movement = new Vector2(0, 0);
        }
        return movement;
    }

    public static Vector2 getKnightAim(int joyNumber)
    {
        // Establish input devices
        InputDevice playerOne = null;
        InputDevice playerTwo = null;

        // Check for connected devices
        if (InputManager.Devices.Count > 0)
        {
            playerOne = InputManager.Devices[0];

            if (InputManager.Devices.Count > 1)
            {
                playerTwo = InputManager.Devices[1];
            }
        }

        // Establish movement vector2 based on left stick input
        Vector2 movement = new Vector2(0, 0);

        if (joyNumber == 1 && playerOne != null)
        {
            movement = playerOne.RightStick;
        }
        else if (joyNumber == 2 && playerTwo != null)
        {
            movement = playerTwo.RightStick;
        }

        float rot = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;

        if (rot > 0 && rot < 45)
        {
            movement = new Vector2(1, 2);
        }
        else if (rot > 45 && rot < 90)
        {
            movement = new Vector2(2, 1);
        }
        else if (rot > 90 && rot < 135)
        {
            movement = new Vector2(2, -1);
        }
        else if (rot > 135 && rot <= 180)
        {
            movement = new Vector2(1, -2);
        }
        else if (rot < -135 && rot > -180)
        {
            movement = new Vector2(-1, -2);
        }
        else if (rot < -90 && rot > -135)
        {
            movement = new Vector2(-2, -1);
        }
        else if (rot < -45 && rot > -90)
        {
            movement = new Vector2(-2, 1);
        }
        else if (rot > -45 && rot < 0)
        {
            movement = new Vector2(-1, 2);
        }
        else
        {
            movement = new Vector2(0, 0);
        }
        return movement;
    }

    public static Vector2 getAim(int joyNumber)
    {
        // Establish input devices
        InputDevice playerOne = null;
        InputDevice playerTwo = null;

        // Check for connected devices
        if (InputManager.Devices.Count > 0)
        {
            playerOne = InputManager.Devices[0];

            if (InputManager.Devices.Count > 1)
            {
                playerTwo = InputManager.Devices[1];
            }
        }

        // Establish movement vector2 based on left stick input
        Vector2 movement = new Vector2(0, 0);

        if (joyNumber == 1 && playerOne != null)
        {
            movement = playerOne.RightStick;
        }
        else if (joyNumber == 2 && playerTwo != null)
        {
            movement = playerTwo.RightStick;
        }        

        // Correct for deadzone and check for vertical/horizontal aim
        if (Mathf.Abs(movement.x) - 0.1 > 0)
        {
            movement.x = movement.x < 0 ? -1 : 1;
        }
        else
        {
            movement.x = 0;
        }

        if (Mathf.Abs(movement.y) - 0.1 > 0)
        {
            movement.y = movement.y < 0 ? -1 : 1;
        }
        else
        {
            movement.y = 0;
        }


        
        return movement;
    }

    public static bool getPressed(int joyNumber)
    {
        // Establish input devices
        InputDevice playerOne = null;
        InputDevice playerTwo = null;

        // Check for connected devices
        if (InputManager.Devices.Count > 0)
        {
            playerOne = InputManager.Devices[0];

            if (InputManager.Devices.Count > 1)
            {
                playerTwo = InputManager.Devices[1];
            }
        }

        // Check for button press
        bool pressed = false;

        if (joyNumber == 1 && playerOne != null && playerOne.Action1.LastState != playerOne.Action1.State)
        {
            pressed = playerOne.Action1.IsPressed;
        }
        else if (joyNumber == 2 && playerTwo != null && playerTwo.Action1.LastState != playerTwo.Action1.State)
        {
            pressed = playerTwo.Action1.IsPressed;
        }

        return pressed;
    }
    
}
