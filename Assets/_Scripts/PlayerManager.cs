using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager instance = null;

	public BotSettings botSettings;

	public Human[] humans;
	// TODO WORM

	private string[] previousJoysticks = null;

	private void Awake() {
		if (instance == null) {
			instance = this;
			Physics2D.IgnoreLayerCollision(8, 8);
		}
	}

	private void Update() {
		string[] joysticks = Input.GetJoystickNames();

		if (previousJoysticks == null) {
			previousJoysticks = new string[joysticks.Length];
			for (int i = 0; i < humans.Length; i++) humans[i].ChangeController(new BotHumanController(i + 1, humans[i], botSettings));
		}

		for (int i = 0; i < humans.Length; i++) {
			int controller = i + 1;

			if (!JoystickExists(previousJoysticks, i) && JoystickExists(joysticks, i)) {
				humans[i].ChangeController(new PlayerHumanController(controller, humans[i]));
			}

			if (JoystickExists(previousJoysticks, i) && !JoystickExists(joysticks, i)) {
				humans[i].ChangeController(new BotHumanController(controller, humans[i], botSettings));
			}

		}

		previousJoysticks = joysticks;

	}

	private bool JoystickExists(string[] joystickNames, int joystick) {
		return joystick < joystickNames.Length && !string.IsNullOrEmpty(joystickNames[joystick]);
	}

	public int GetNumberOfHumans() {
		int ret = 0;
		foreach (Human human in humans) {
			if (human.IsAlive()) ret++;
		}
		return ret;
	}

	public Vector2[] GetMeteorPositions() {
		return new Vector2[0];
	}

	public Vector2[] GetPlayerPositions() {
		List<Vector2> ret = new List<Vector2>();

		// TODO ret.Add(WORM);
		for (int i = 0; i < humans.Length; i++) {
			ret.Add(humans[i].transform.position);
		}

		return ret.ToArray();

	}

}
