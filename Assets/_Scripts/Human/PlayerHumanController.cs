﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanController : IHumanController {

	protected int controllerNum = 0;
	protected Human human;

	private Vector2 movement = Vector2.zero;
	private bool jumpButton = false;
	private bool dashButton = false;

	public PlayerHumanController(int controllerNum, Human human) {
		this.controllerNum = controllerNum;
		this.human = human;
	}

	public bool GetCrouchButton() {
		return movement.y > 0;
	}

	public bool GetDashButton() {
		return dashButton;
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
		dashButton = Input.GetAxis($"Joystick{controllerNum}button1") != 0;
	}
}
