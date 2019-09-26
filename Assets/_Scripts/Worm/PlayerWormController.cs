using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;

public class PlayerWormController : IWormController
{
    private int controllerNum;

    public PlayerWormController(int controllerNum)
    {
        this.controllerNum = controllerNum;
    }
    
    public Vector2 GetMoveDirection()
    {
        var vertical = Input.GetAxis($"Joystick{controllerNum}Y");
        var horizontal = Input.GetAxis($"Joystick{controllerNum}X");
        
        var direction = new Vector2(vertical, horizontal);
        if (direction.sqrMagnitude > 1)
        {
            direction.Normalize();
        }
        
        return direction;
    }

    public bool GetBoosting()
    {
        return false;
    }

    public bool GetEating()
    {
        return true;
    }
}
