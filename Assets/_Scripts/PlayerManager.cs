using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager instance = null;

	public BotSettings botSettings;

	public Human[] humans;

	public PlayerWinEvent onPlayerWin = new PlayerWinEvent();

	private string[] previousJoysticks = null;

	private MeteorManager meteorManager;
	private Cinemachine.CinemachineTargetGroup targetGroup;

	private bool winEventConsumed = false;

	private void Awake() {
		if (instance == null) {
			instance = this;
			Physics2D.IgnoreLayerCollision(8, 8);
			Physics2D.IgnoreLayerCollision(8, 9);

			meteorManager = FindObjectOfType<MeteorManager>();
			targetGroup = FindObjectOfType<Cinemachine.CinemachineTargetGroup>();

			foreach (Human human in humans) {
				human.onDeath.AddListener(new UnityAction(() => {
					targetGroup.RemoveMember(human.transform);
				}));

				targetGroup.AddMember(human.transform, 1, 2);
			}
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

		if (GetNumberOfHumans() <= 1) {
			int[] remaining = GetRemainingPlayers();
			onPlayerWin.Invoke(remaining.Length > 0 ? remaining[0] : -1);
			winEventConsumed = true;
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

	public int[] GetRemainingPlayers() {
		List<int> ret = new List<int>();

		for (int i = 0; i < humans.Length; i++) {
			if (humans[i].IsAlive()) ret.Add(i + 1);
		}

		return ret.ToArray();
	}

	public Vector2[] GetMeteorPositions() {

		List<GameObject> meteors = meteorManager.meteors;

		Vector2[] positions = new Vector2[meteors.Count];

		for (int i = 0; i < meteors.Count; i++) {
			positions[i] = meteors[i].transform.position;
		}

		return positions;
	}

	public Vector2[] GetPlayerPositions() {
		List<Vector2> ret = new List<Vector2>();

		for (int i = 0; i < humans.Length; i++) {
			ret.Add(humans[i].transform.position);
		}

		return ret.ToArray();

	}

}

[System.Serializable]
public class PlayerWinEvent : UnityEvent<int> {}
