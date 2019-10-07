using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiJumpEffect : HumanEffect {

	public int jumpCount = 4;

	private int prevJumpCount = 2;

	public override void OnStartEffect(Human human) {
		prevJumpCount = human.GetJumpCount();
		human.SetJumpCount(jumpCount);
	}

	public override void OnEndEffect(Human human) {
		human.SetJumpCount(prevJumpCount);
	}

}
