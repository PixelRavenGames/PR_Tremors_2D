using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterDashEffect : HumanEffect {

	public float dashSpeed = 60;

	private float prevDashSpeed = 40;

	public override void OnStartEffect(Human human) {
		prevDashSpeed = human.GetDashSpeed();
		human.SetDashSpeed(dashSpeed);
	}

	public override void OnEndEffect(Human human) {
		human.SetDashSpeed(prevDashSpeed);
	}

}
