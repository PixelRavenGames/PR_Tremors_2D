using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShieldEffect : HumanEffect {

	public GameObject displayPrefab;
	public int shieldCount = 1;

	private GameObject displayObject = null;

	public override void OnStartEffect(Human human) {
		human.SetInvulnerable(true);

		if (!displayObject) displayObject = Instantiate(displayPrefab, human.transform);
	}

	public override void OnEndEffect(Human human) {
		human.SetInvulnerable(false);

		if (displayObject) Destroy(displayObject);
		displayObject = null;
	}

	public override void OnDamageTaken(Human human, float damage) {
		shieldCount--;
	}

	public override bool CanContinue() {
		return shieldCount > 0;
	}

}
