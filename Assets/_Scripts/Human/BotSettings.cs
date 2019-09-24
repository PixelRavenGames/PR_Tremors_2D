using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Bot Settings", menuName = "Bots/Bot Settings", order = 1)]
public class BotSettings : ScriptableObject {

	public float wanderUpdateDelay = 3f;

	public float minimumIdleDistance = 5f;
	public float idleEndChance = 30f;

	public float wanderEndChance = 30f;
	public float wanderJumpMinimumSpeed = 0.7f;
	public float wanderContinueJumpChance = 90f;

	public float runDistance = 1f;
	public float runDistanceToJump = 0.25f;
	public float runJumpChance = 60f;

	public float movementDeadzone = 0f;
}
