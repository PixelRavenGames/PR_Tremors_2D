﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReadyCheck : MonoBehaviour {

	[SerializeField]
	private int buttonIndex = 0;

	[SerializeField]
	private int playerNumber = 8;

	[Header("Events")]
	public OnNewReady onNewReady = new OnNewReady();
	public OnReady onReady = new OnReady();

	private bool[] ready;
	private bool needReady = true;

	void Start() {
		ready = new bool[playerNumber];
	}

	void Update() {
		if (!needReady) return;


		string[] joysticks = Input.GetJoystickNames();

		for (int i = 0; i < playerNumber; i++) {
			if (!ready[i] && (Input.GetKeyDown($"joystick {i + 1} button {buttonIndex}") || !JoystickExists(joysticks, i))) {
				onNewReady.Invoke(i + 1);
				Debug.Log($"READY PLAYER {i + 1}");
				ready[i] = true;
			}
		}

		bool isAllReady = true;
		foreach (bool r in ready) if (!r) isAllReady = false;
		if (isAllReady) { onReady.Invoke(); needReady = false; }
	}

	private bool JoystickExists(string[] joystickNames, int joystick) {
		return joystick < joystickNames.Length && !string.IsNullOrEmpty(joystickNames[joystick]);
	}
}

#region
[System.Serializable]
public class OnNewReady : UnityEvent<int> { }
[System.Serializable]
public class OnReady : UnityEvent { }
#endregion
