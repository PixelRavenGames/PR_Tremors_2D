using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager instance = null;

	public Human[] humans;
	// TODO WORM

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
	}

	public int GetNumberOfHumans() {
		int ret = 0;
		foreach (Human human in humans) {
			if (human.IsAlive()) ret++;
		}
		return ret;
	}

	public Vector2 GetWormPosition() {
		return Vector2.zero;
	}

	public Vector2[] GetPlayerPositions() {
		List<Vector2> ret = new List<Vector2>();

		// TODO ret.Add(WORM);
		for (int i = 0; i < humans.Length; i++) {
			if (humans[i].IsAlive()) ret.Add(humans[i].transform.position);
		}

		return ret.ToArray();

	}

}
