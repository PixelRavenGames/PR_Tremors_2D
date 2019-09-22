using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanController : IHumanController {

	protected int controllerNum = 0;

	private Vector2 movement = Vector2.zero;
	private bool jumpButton = false;

	public PlayerHumanController(int controllerNum) {
		this.controllerNum = controllerNum;
	}

	public bool GetJumpButton() {
		return jumpButton;
	}

	public float GetMoveMagnitude() {
		return movement.x;
	}

	public bool ShouldMove() {
		return movement.x != 0;
	}

	public void Update() {
		movement = new Vector2(Input.GetAxis($"Joystick{controllerNum}X"), Input.GetAxis($"Joystick{controllerNum}Y"));
		jumpButton = Input.GetAxis($"Joystick{controllerNum}button0") != 0;
	}
}
