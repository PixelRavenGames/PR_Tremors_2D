using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWormController
{
    Vector2 GetMoveDirection();
    bool GetBoosting();
    bool GetEating();
}
