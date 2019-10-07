using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMP_EndScript : MonoBehaviour {

	public PlayerManager playerManager;
	public MeteorManager meteorManager;

	void Update() {
		if (playerManager.GetNumberOfHumans() <= 1) {
			meteorManager.enabled = false;

			int[] remaining = playerManager.GetRemainingPlayers();

			if (remaining.Length > 0) {
				Debug.Log($"{remaining[0].ToString()} IS THE WINNER");
			}

		}
	}
}
