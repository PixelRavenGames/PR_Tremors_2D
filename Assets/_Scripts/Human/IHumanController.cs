using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHumanController {

	bool ShouldMove();
	float GetMoveMagnitude();
	bool GetJumpButton();
	bool GetCrouchButton();

	bool GetDashButton();

	void Update();

}
