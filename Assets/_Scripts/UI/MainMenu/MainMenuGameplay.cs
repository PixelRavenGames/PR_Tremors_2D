using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameplay : MonoBehaviour {

	public GameObject levelPrefab;
	public FadeUI fade;

	public PlayerManager playerManager;
	public GameObject levelObj;

	public GameObject[] players;

	private bool isResetting = false;

	public void Reset() {
		if (isResetting) return;
		isResetting = true;

		fade.shouldShow = true;

		Invoke("ResetMid", 1);
	}

	private void ResetMid() {
		// Replace players
		foreach (Human h in playerManager.humans) {
			if (h) Destroy(h.gameObject);
		}

		playerManager.humans = new Human[players.Length];

		for (int i = 0; i < players.Length; i++) {
			playerManager.humans[i] = Instantiate(players[i], new Vector2((2 * i) - 8, 3), Quaternion.identity).GetComponent<Human>();
			playerManager.SetPlayerToBot(i + 1);
		}

		// Replace level
		if (levelObj) Destroy(levelObj);
		levelObj = Instantiate(levelPrefab);

		Invoke("ResetFinal", 1);
	}

	private void ResetFinal() {
		isResetting = false;
		fade.shouldShow = false;
	}

}
