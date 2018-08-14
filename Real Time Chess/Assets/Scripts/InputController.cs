using UnityEngine;
using UnityEngine.Experimental.Input;

public class InputController : MonoBehaviour
{
    // Holds the gamepads for each player
    [HideInInspector]
    private Gamepad gamepad1, gamepad2;

    // The variables used for game inputs
    [HideInInspector]
    public Vector2 p1Aim, p1Move, p1MoveFloat, p1KnightAim, p1KnightMove;

    [HideInInspector]
    public Vector2 p2Aim, p2Move, p2MoveFloat, p2KnightAim, p2KnightMove;

    [HideInInspector]
    public bool p1Pressed, p2Pressed;

    // Set the devices up
    private void Awake()
    {
        // Initialize variables
        p1Move = Vector2.zero;
        p2Move = Vector2.zero;
        p1Aim = Vector2.zero;
        p2Aim = Vector2.zero;
        p1Pressed = false;
        p2Pressed = false;

        // Check for and assign gamepads
        var allGamepads = Gamepad.all;
        if (allGamepads.Count > 0)
        {
            gamepad1 = allGamepads[0];
        }
        
        if (allGamepads.Count > 1)
        {
            gamepad2 = allGamepads[1];
        }

        // Assign delegate functions for when controllers are added or removed
        InputSystem.onDeviceChange +=
        (device, change) =>
        {
            if (change == InputDeviceChange.Added)
            {
                var allPads = Gamepad.all;

                if (allPads.Count == 2)
                {
                    if (gamepad1 == null)
                    {
                        // If a gamepad is connected, and there are 2 gamepads, and gamepad1 is missing:
                        // Check to see which slot gamepad2 is occupying and put gamepad1 in the other one.
                        if (gamepad2.device == allPads[0].device)
                        {
                            gamepad1 = allPads[1];
                        }
                        else
                        {
                            gamepad1 = allPads[0];
                        }
                    }
                    else if (gamepad2 == null)
                    {
                        // If a gamepad is connected, and there are 2 gamepads, and gamepad2 is missing:
                        // Check to see which slot gamepad1 is occupying and put gamepad2 in the other one.
                        if (gamepad1.device == allPads[0].device)
                        {
                            gamepad2 = allPads[1];
                        }
                        else
                        {
                            gamepad2 = allPads[0];
                        }
                    }
                }
                else if (allPads.Count == 1)
                {
                    // If a gamepad is connected, and there is only one gamepad, give it the first slot.
                    gamepad1 = allPads[0];
                }

                Debug.Log("Device Added");
            }
            else if (change == InputDeviceChange.Removed)
            {
                if (Gamepad.all.Count == 0)
                {
                    // If a gamepad is removed and there are no gamepads, clear the slots.
                    gamepad1 = null;
                    gamepad2 = null;
                }
                if (Gamepad.all.Count == 1)
                {
                    // If a gamepad is removed and there is only one gamepad connected,
                    // Check to see which gamepad is connected and clear the slot for the other one.
                    if (gamepad1.device == Gamepad.all[0].device)
                    {
                        gamepad2 = null;
                    }
                    else
                    {
                        gamepad1 = null;
                    }
                }

                Debug.Log("Device Removed");
            }
        };
    }
    
    // Update the variables based on device input
    private void Update()
    {
        // Update the input variables for gamepad1 if it is connected
        if (gamepad1 != null)
        {
            p1Move = new Vector2(gamepad1.leftStick.x.ReadValue(), gamepad1.leftStick.y.ReadValue());
            p1MoveFloat = p1Move;
            p1KnightMove = GetKnightAim(p1Move);
            p1Move = NormalizeMove(p1Move);

            p1Aim = new Vector2(gamepad1.rightStick.x.ReadValue(), gamepad1.rightStick.y.ReadValue());
            p1KnightAim = GetKnightAim(p1Aim);
            p1Aim = NormalizeMove(p1Aim);
            
            p1Pressed = gamepad1.buttonSouth.wasJustPressed;
        }

        // Update the input variables for gamepad2 if it is connected
        if (gamepad2 != null)
        {
            p2Move = new Vector2(gamepad2.leftStick.x.ReadValue(), gamepad2.leftStick.y.ReadValue());
            p2MoveFloat = p2Move;
            p2KnightMove = GetKnightAim(p2Move);
            p2Move = NormalizeMove(p2Move);

            p2Aim = new Vector2(gamepad2.rightStick.x.ReadValue(), gamepad2.rightStick.y.ReadValue());
            p2KnightAim = GetKnightAim(p2Aim);
            p2Aim = NormalizeMove(p2Aim);
            
            p2Pressed = gamepad2.buttonSouth.wasJustPressed;
        }        
    }

    /// <summary>
    /// Normalize an input move vector so its components are -1, 0, or 1
    /// </summary>
    /// <param name="move">A vector2 movement input</param>
    /// <returns></returns>
    private Vector2 NormalizeMove(Vector2 move)
    {
        Vector2 newMove = Vector2.zero;

        if (Mathf.Abs(move.x) - 0.3 > 0)
        {
            move.x = move.x < 0 ? -1 : 1;
        }
        else
        {
            move.x = 0;
        }

        if (Mathf.Abs(move.y) - 0.3 > 0)
        {
            move.y = move.y < 0 ? -1 : 1;
        }
        else
        {
            move.y = 0;
        }

        return move;
    }

    /// <summary>
    /// Get the appropriate aim position based on the Knight's pattern
    /// </summary>
    /// <param name="aim">The Vector2 input for the player's aim</param>
    /// <returns></returns>
    private Vector2 GetKnightAim(Vector2 aim)
    {
        Vector2 movement = Vector2.zero;

        // Convert the Vector2 to a rotational position
        float rot = Mathf.Atan2(aim.x, aim.y) * Mathf.Rad2Deg;

        // Set the movement vector based on the rotational position
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

}
